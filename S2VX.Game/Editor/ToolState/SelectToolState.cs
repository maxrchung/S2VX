﻿using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using S2VX.Game.Editor.Containers;
using S2VX.Game.Editor.Reversible;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;
using System;
using System.Collections.Generic;
using System.Linq;

namespace S2VX.Game.Editor.ToolState {
    public class SelectToolState : S2VXToolState {
        [Resolved]
        private S2VXStory Story { get; set; } = null;
        [Resolved]
        private EditorScreen Editor { get; set; } = null;
        private NotesTimeline NotesTimeline { get; set; }

        private Dictionary<S2VXNote, double> TimelineNoteToDragPointDelta { get; set; } = new Dictionary<S2VXNote, double>();
        private Dictionary<S2VXNote, Vector2> NoteToDragPointDelta { get; set; } = new Dictionary<S2VXNote, Vector2>();
        private SelectToolDragState ToDrag { get; set; } = SelectToolDragState.None;
        private bool DelayDrag { get; set; }

        private const float SelectionIndicatorThickness = 0.025f;

        private double OldEndTime { get; set; }
        private Vector2 OldCoords { get; set; }

        [BackgroundDependencyLoader]
        private void Load() => NotesTimeline = Editor.NotesTimeline;

        private static bool IsMouseOnTimelineNote(Vector2 mousePos, RelativeBox timelineNote) {
            var leftBound = timelineNote.DrawPosition.X - timelineNote.DrawSize.X / 2;
            var rightBound = timelineNote.DrawPosition.X + timelineNote.DrawSize.X / 2;
            var topBound = timelineNote.DrawPosition.Y - timelineNote.DrawSize.Y / 2;
            var bottomBound = timelineNote.DrawPosition.Y + timelineNote.DrawSize.Y / 2;

            var mouseInXRange = leftBound <= mousePos.X && mousePos.X <= rightBound;
            var mouseInYRange = topBound <= mousePos.Y && mousePos.Y <= bottomBound;
            return mouseInXRange && mouseInYRange;
        }

        private bool IsMouseOnNote(Vector2 mousePos, S2VXNote note) {
            mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(mousePos), Editor);

            // DrawPosition is centered at (0,0). I convert it so (0,0) starts top left
            var convertedCenterPoint = new Vector2(note.DrawPosition.X + Editor.DrawWidth / 2, note.DrawPosition.Y + Editor.DrawHeight / 2);
            var topLeft = new Vector2(convertedCenterPoint.X - note.DrawWidth / 2, convertedCenterPoint.Y - note.DrawHeight / 2);
            var topRight = new Vector2(convertedCenterPoint.X + note.DrawWidth / 2, convertedCenterPoint.Y - note.DrawHeight / 2);
            var bottomLeft = new Vector2(convertedCenterPoint.X - note.DrawWidth / 2, convertedCenterPoint.Y + note.DrawHeight / 2);

            // https://gamedev.stackexchange.com/a/110233
            var pointA = S2VXUtils.Rotate(new Vector2(topLeft.X - convertedCenterPoint.X, topLeft.Y - convertedCenterPoint.Y), note.Rotation);
            var pointB = S2VXUtils.Rotate(new Vector2(topRight.X - convertedCenterPoint.X, topRight.Y - convertedCenterPoint.Y), note.Rotation);
            var pointC = S2VXUtils.Rotate(new Vector2(bottomLeft.X - convertedCenterPoint.X, bottomLeft.Y - convertedCenterPoint.Y), note.Rotation);

            // Shift origin back after rotating
            pointA = new Vector2(pointA.X + convertedCenterPoint.X, pointA.Y + convertedCenterPoint.Y);
            pointB = new Vector2(pointB.X + convertedCenterPoint.X, pointB.Y + convertedCenterPoint.Y);
            pointC = new Vector2(pointC.X + convertedCenterPoint.X, pointC.Y + convertedCenterPoint.Y);

            var v0 = pointC - pointA;
            var v1 = pointB - pointA;
            var v2 = mousePos - pointA;

            var dot00 = Vector2.Dot(v0, v0);
            var dot01 = Vector2.Dot(v0, v1);
            var dot02 = Vector2.Dot(v0, v2);
            var dot11 = Vector2.Dot(v1, v1);
            var dot12 = Vector2.Dot(v1, v2);

            var invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
            var u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            var v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            return u >= 0 && v >= 0 && u <= 1 && v <= 1;
        }

        private List<S2VXNote> GetVisibleStoryNotes() {
            var visibleStoryNotes = new List<S2VXNote>();
            foreach (var note in Story.Notes.Children) {
                if (note.Alpha > 0) {
                    visibleStoryNotes.Add(note);
                }
            }
            return visibleStoryNotes;
        }

        public void AddNoteSelection(S2VXNote note) {
            NotesTimeline.AddNoteTimelineSelection(note);
            var noteSelection = new RelativeBox {
                Colour = Color4.LimeGreen.Opacity(0.5f),
                Width = note.Size.X + SelectionIndicatorThickness,
                Height = note.Size.Y + SelectionIndicatorThickness,
                Rotation = note.Rotation,
            };
            noteSelection.X = note.Position.X;
            noteSelection.Y = note.Position.Y;
            Editor.NoteSelectionIndicators.Add(noteSelection);
        }

        public void ClearNoteSelection() {
            NotesTimeline.ClearNoteTimelineSelection();
            Editor.NoteSelectionIndicators.Clear();
        }

        public override bool OnToolMouseDown(MouseDownEvent e) {
            ClearNoteSelection();
            var mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(e.ScreenSpaceMousePosition), Editor.NotesTimeline.NoteSelectionIndicators);

            foreach (var notes in Editor.NotesTimeline.NoteToTimelineNote) {
                if (IsMouseOnTimelineNote(mousePos, notes.Value)) {
                    AddNoteSelection(notes.Key);
                    return false;
                }
            }
            foreach (var note in GetVisibleStoryNotes()) {
                if (IsMouseOnNote(e.ScreenSpaceMousePosition, note)) {
                    AddNoteSelection(note);
                    return false;
                }
            }
            return false;
        }

        public override bool OnToolDragStart(DragStartEvent e) {
            var mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(e.ScreenSpaceMousePosition), Editor.NotesTimeline.NoteSelectionIndicators);
            var selectedNoteTime = 0d;
            var selectedNoteCoord = Vector2.Zero;

            foreach (var noteAndTime in NotesTimeline.SelectedNoteToTime) {
                var note = noteAndTime.Key;
                var noteToTimelineNote = Editor.NotesTimeline.NoteToTimelineNote;
                if (noteToTimelineNote.ContainsKey(note) && IsMouseOnTimelineNote(mousePos, noteToTimelineNote[note])) {
                    ToDrag = SelectToolDragState.DragTimelineNote;
                    OldEndTime = selectedNoteTime = note.HitTime;
                    break;
                }
            }
            if (ToDrag == SelectToolDragState.None) {
                foreach (var note in GetVisibleStoryNotes()) {
                    if (IsMouseOnNote(e.ScreenSpaceMousePosition, note)) {
                        ToDrag = SelectToolDragState.DragNote;
                        OldCoords = selectedNoteCoord = note.Coordinates;
                        break;
                    }
                }
            }

            switch (ToDrag) {
                case SelectToolDragState.DragTimelineNote:
                    foreach (var noteAndTime in NotesTimeline.SelectedNoteToTime) {
                        var note = noteAndTime.Key;
                        TimelineNoteToDragPointDelta[note] = note.HitTime - selectedNoteTime;
                    }
                    break;
                case SelectToolDragState.DragNote:
                    foreach (var noteAndTime in NotesTimeline.SelectedNoteToTime) {
                        var note = noteAndTime.Key;
                        NoteToDragPointDelta[note] = note.Coordinates - selectedNoteCoord;
                    }
                    break;
                case SelectToolDragState.None:
                    break;
            }
            return true;
        }

        private double GetClosestTickTime(double gameTime) {
            var numTicks = Story.BPM / 60f * (Editor.Track.Length / NotesTimeline.SecondsToMS) * Editor.NotesTimeline.Divisor;
            var timeBetweenTicks = Editor.Track.Length / numTicks;
            var leftOffset = (gameTime - Story.Offset) % timeBetweenTicks;
            var rightOffset = timeBetweenTicks - leftOffset;
            return gameTime + (leftOffset <= rightOffset ? -leftOffset : rightOffset);
        }

        private double GetGameTimeAtMouse(Vector2 localMousePos) {
            var noteSelectionIndicators = Editor.NotesTimeline.NoteSelectionIndicators;
            var mousePosX = ToSpaceOfOtherDrawable(ToLocalSpace(localMousePos), noteSelectionIndicators).X;
            // temp until NoteTimeline Scroll on drag is implemented
            mousePosX = Math.Clamp(mousePosX, 0, noteSelectionIndicators.DrawWidth);
            var relativeMousePosX = mousePosX / noteSelectionIndicators.DrawWidth;
            var gameTimeDeltaFromMiddle = (relativeMousePosX - 0.5f) * Editor.NotesTimeline.SectionLength * NotesTimeline.SecondsToMS;
            return Time.Current + gameTimeDeltaFromMiddle;
        }

        public override void OnToolDrag(DragEvent e) {
            if (!DelayDrag) {
                switch (ToDrag) {
                    case SelectToolDragState.DragTimelineNote: {
                        var gameTimeAtMouse = GetGameTimeAtMouse(e.ScreenSpaceMousePosition);
                        foreach (var note in NotesTimeline.SelectedNoteToTime.Keys.ToList()) {
                            var newTime = GetClosestTickTime(gameTimeAtMouse) + TimelineNoteToDragPointDelta[note];
                            note.UpdateHitTime(newTime);
                            NotesTimeline.AddNoteTimelineSelection(note);
                        }
                        break;
                    }
                    case SelectToolDragState.DragNote: {
                        var mousePos = Editor.MousePosition;

                        foreach (var noteAndTime in NotesTimeline.SelectedNoteToTime) {
                            var note = noteAndTime.Key;
                            var newPos = mousePos + NoteToDragPointDelta[note];
                            note.UpdateCoordinates(newPos);
                        }
                        break;
                    }
                    case SelectToolDragState.None:
                        break;
                }
                DelayDrag = true;
            }
        }

        public override void OnToolDragEnd(DragEndEvent e) {
            switch (ToDrag) {
                case SelectToolDragState.DragTimelineNote: {
                    var gameTimeAtMouse = GetGameTimeAtMouse(e.ScreenSpaceMousePosition);

                    foreach (var note in NotesTimeline.SelectedNoteToTime.Keys.ToList()) {
                        var newTime = GetClosestTickTime(gameTimeAtMouse) + TimelineNoteToDragPointDelta[note];
                        Editor.Reversibles.Push(new ReversibleUpdateNoteHitTime(note, OldEndTime, newTime, Editor));
                    }
                    break;
                }
                case SelectToolDragState.DragNote: {
                    var mousePos = Editor.MousePosition;

                    foreach (var noteAndTime in NotesTimeline.SelectedNoteToTime) {
                        var note = noteAndTime.Key;
                        var newPos = mousePos + NoteToDragPointDelta[note];
                        Editor.Reversibles.Push(new ReversibleUpdateNoteCoordinates(note, OldCoords, newPos));
                    }

                    break;
                }
                case SelectToolDragState.None:
                    return;
            }

            ToDrag = SelectToolDragState.None;
        }

        public override bool OnToolKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.Delete:
                    foreach (var noteAndTime in NotesTimeline.SelectedNoteToTime) {
                        var note = noteAndTime.Key;
                        note.ReversibleRemove(Story, Editor);
                    }
                    ClearNoteSelection();
                    return true;
                default:
                    break;
            }
            return false;
        }

        public override void HandleExit() => ClearNoteSelection();

        protected override void Update() => DelayDrag = false;

        public override string DisplayName() => "Select";
    }
}
