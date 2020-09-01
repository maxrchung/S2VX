﻿using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using S2VX.Game.Story;
using S2VX.Game.Editor.Containers;
using System;
using System.Collections.Generic;

namespace S2VX.Game.Editor.ToolState {
    public class SelectToolState : S2VXToolState {
        [Resolved]
        private S2VXStory Story { get; set; } = null;
        [Resolved]
        private S2VXEditor Editor { get; set; } = null;
        private Dictionary<Note, double> SelectedNoteToTime { get; set; } = new Dictionary<Note, double>();
        private Dictionary<Drawable, Note> NoteSelectionToNote { get; set; } = new Dictionary<Drawable, Note>();

        private const float SelectionIndicatorThickness = 0.025f;

        private bool DragTimelineNote { get; set; }
        private Dictionary<Note, double> NoteToDragPointDelta { get; set; } = new Dictionary<Note, double>();

        private static bool MouseIsOnNote(Vector2 mousePos, RelativeBox timelineNote) {
            var leftBound = timelineNote.DrawPosition.X - timelineNote.DrawSize.X / 2;
            var rightBound = timelineNote.DrawPosition.X + timelineNote.DrawSize.X / 2;
            var topBound = timelineNote.DrawPosition.Y - timelineNote.DrawSize.Y / 2;
            var bottomBound = timelineNote.DrawPosition.Y + timelineNote.DrawSize.Y / 2;

            var mouseInXRange = leftBound <= mousePos.X && mousePos.X <= rightBound;
            var mouseInYRange = topBound <= mousePos.Y && mousePos.Y <= bottomBound;
            return mouseInXRange && mouseInYRange;
        }

        public override bool OnToolMouseDown(MouseDownEvent e) {
            SelectedNoteToTime.Clear();
            Editor.NoteSelectionIndicators.Clear();
            var mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(e.ScreenSpaceMousePosition), Editor.NotesTimeline.TickBarNoteSelections);
            foreach (var notes in Editor.NotesTimeline.NoteToTimelineNote) {
                if (MouseIsOnNote(mousePos, notes.Value)) {
                    var note = notes.Key;
                    SelectedNoteToTime[notes.Key] = notes.Key.EndTime;
                    var noteSelection = new RelativeBox {
                        Colour = Color4.LimeGreen.Opacity(0.5f),
                        Width = note.Size.X + SelectionIndicatorThickness,
                        Height = note.Size.Y + SelectionIndicatorThickness,
                        RelativePositionAxes = Axes.Both,
                        RelativeSizeAxes = Axes.Both,
                        Rotation = note.Rotation,
                    };
                    noteSelection.X = note.Position.X;
                    noteSelection.Y = note.Position.Y;
                    Editor.NoteSelectionIndicators.Add(noteSelection);
                    NoteSelectionToNote[noteSelection] = note;
                    return false;
                }
            }
            return false;
        }

        public override bool OnToolDragStart(DragStartEvent e) {
            var mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(e.ScreenSpaceMousePosition), Editor.NotesTimeline.TickBarNoteSelections);
            var selectedNoteTime = 0d;

            foreach (var noteAndTime in SelectedNoteToTime) {
                var note = noteAndTime.Key;
                if (MouseIsOnNote(mousePos, Editor.NotesTimeline.NoteToTimelineNote[note])) {
                    DragTimelineNote = true;
                    selectedNoteTime = note.EndTime;
                    break;
                }
            }
            if (DragTimelineNote) {
                foreach (var noteAndTime in SelectedNoteToTime) {
                    var note = noteAndTime.Key;
                    NoteToDragPointDelta[note] = note.EndTime - selectedNoteTime;
                }
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

        public override void OnToolDrag(DragEvent e) {
            if (DragTimelineNote) {
                var mousePosX = ToSpaceOfOtherDrawable(ToLocalSpace(e.ScreenSpaceMousePosition), Editor.NotesTimeline.TickBarNoteSelections).X;
                mousePosX = Math.Clamp(mousePosX, 0, Editor.NotesTimeline.TickBarNoteSelections.DrawWidth); // temp until NoteTimeline Scroll on drag is implemented
                var relativeMousePosX = mousePosX / Editor.NotesTimeline.TickBarNoteSelections.DrawWidth;
                var gameTimeDeltaFromMiddle = (relativeMousePosX - 0.5f) * Editor.NotesTimeline.SectionLength * NotesTimeline.SecondsToMS;
                var gameTimeAtMouse = Time.Current + gameTimeDeltaFromMiddle;

                var selectedNoteToTimeCopy = new Dictionary<Note, double>(SelectedNoteToTime);
                foreach (var noteAndTime in SelectedNoteToTime) {
                    var note = noteAndTime.Key;
                    var newTime = GetClosestTickTime(gameTimeAtMouse) + NoteToDragPointDelta[note];
                    note.UpdateEndTime(newTime);
                    selectedNoteToTimeCopy[note] = newTime;
                }
                SelectedNoteToTime = selectedNoteToTimeCopy;
            }
        }

        public override void OnToolDragEnd(DragEndEvent _) => DragTimelineNote = false;

        public override bool OnToolKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.Delete:
                    Editor.NoteSelectionIndicators.Clear();
                    foreach (var noteAndTime in SelectedNoteToTime) {
                        var note = noteAndTime.Key;
                        Story.RemoveNote(note);
                    }
                    SelectedNoteToTime.Clear();
                    return true;
                default:
                    break;
            }
            return false;
        }

        public override void HandleExit() {
            Editor.NotesTimeline.TickBarNoteSelections.Clear();
            Editor.NoteSelectionIndicators.Clear();
        }

        protected override void Update() {
            Editor.NotesTimeline.TickBarNoteSelections.Clear();
            var lowerBound = Time.Current - Editor.NotesTimeline.SectionLength * NotesTimeline.SecondsToMS / 2;
            var upperBound = Time.Current + Editor.NotesTimeline.SectionLength * NotesTimeline.SecondsToMS / 2;
            foreach (var noteAndTime in SelectedNoteToTime) {
                if (lowerBound <= noteAndTime.Value && noteAndTime.Value <= upperBound) {
                    var relativePosition = (noteAndTime.Value - lowerBound) / (Editor.NotesTimeline.SectionLength * NotesTimeline.SecondsToMS);
                    var indication = new RelativeBox {
                        Colour = Color4.LimeGreen.Opacity(0.727f),
                        Width = NotesTimeline.TimelineNoteWidth + 0.009727f,
                        Height = NotesTimeline.TimelineNoteHeight + 0.1f,
                        X = (float)relativePosition,
                        Y = 0.2f,
                        Anchor = Anchor.TopLeft,
                        RelativePositionAxes = Axes.Both,
                        RelativeSizeAxes = Axes.Both,
                    };
                    Editor.NotesTimeline.TickBarNoteSelections.Add(indication);
                }
            }
            foreach (var noteSelection in Editor.NoteSelectionIndicators) {
                noteSelection.Rotation = NoteSelectionToNote[noteSelection].Rotation;
                noteSelection.X = NoteSelectionToNote[noteSelection].Position.X;
                noteSelection.Y = NoteSelectionToNote[noteSelection].Position.Y;
            }
        }
        public override string DisplayName() => "Select";
    }
}