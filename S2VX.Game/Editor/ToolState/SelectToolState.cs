using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using S2VX.Game.Editor.Containers;
using S2VX.Game.Story;
using System;
using System.Collections.Generic;
using S2VX.Game.Editor.Reversible;

namespace S2VX.Game.Editor.ToolState {
    public enum DragState {
        None,
        DragTimelineNote,
        DragNote,
    }

    public class SelectToolState : S2VXToolState {
        [Resolved]
        private S2VXStory Story { get; set; } = null;
        [Resolved]
        private S2VXEditor Editor { get; set; } = null;
        private Dictionary<Note, double> SelectedNoteToTime { get; set; } = new Dictionary<Note, double>();
        private Dictionary<Drawable, Note> NoteSelectionToNote { get; set; } = new Dictionary<Drawable, Note>();

        private Dictionary<Note, double> TimelineNoteToDragPointDelta { get; set; } = new Dictionary<Note, double>();
        private Dictionary<Note, Vector2> NoteToDragPointDelta { get; set; } = new Dictionary<Note, Vector2>();
        private DragState ToDrag { get; set; } = DragState.None;

        private const float SelectionIndicatorThickness = 0.025f;

        private static bool IsMouseOnTimelineNote(Vector2 mousePos, RelativeBox timelineNote) {
            var leftBound = timelineNote.DrawPosition.X - timelineNote.DrawSize.X / 2;
            var rightBound = timelineNote.DrawPosition.X + timelineNote.DrawSize.X / 2;
            var topBound = timelineNote.DrawPosition.Y - timelineNote.DrawSize.Y / 2;
            var bottomBound = timelineNote.DrawPosition.Y + timelineNote.DrawSize.Y / 2;

            var mouseInXRange = leftBound <= mousePos.X && mousePos.X <= rightBound;
            var mouseInYRange = topBound <= mousePos.Y && mousePos.Y <= bottomBound;
            return mouseInXRange && mouseInYRange;
        }

        private bool IsMouseOnNote(Vector2 mousePos, Note note) {
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

        private List<Note> GetVisibleStoryNotes() {
            var visibleStoryNotes = new List<Note>();
            foreach (var note in Story.Notes.Children) {
                var leftTimeBound = note.EndTime - Story.Notes.ShowTime - Story.Notes.FadeInTime;
                var rightTimeBound = note.EndTime + Story.Notes.FadeOutTime;
                var noteVisibleOnEditor = leftTimeBound <= Time.Current && Time.Current <= rightTimeBound;

                if (noteVisibleOnEditor) {
                    visibleStoryNotes.Add(note);
                }
            }
            return visibleStoryNotes;
        }

        private void AddNoteSelection(Note note) {
            SelectedNoteToTime[note] = note.EndTime;
            var noteSelection = new RelativeBox {
                Colour = Color4.LimeGreen.Opacity(0.5f),
                Width = note.Size.X + SelectionIndicatorThickness,
                Height = note.Size.Y + SelectionIndicatorThickness,
                Rotation = note.Rotation,
            };
            noteSelection.X = note.Position.X;
            noteSelection.Y = note.Position.Y;
            Editor.NoteSelectionIndicators.Add(noteSelection);
            NoteSelectionToNote[noteSelection] = note;
        }

        public override bool OnToolMouseDown(MouseDownEvent e) {
            SelectedNoteToTime.Clear();
            Editor.NoteSelectionIndicators.Clear();
            var mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(e.ScreenSpaceMousePosition), Editor.NotesTimeline.TickBarNoteSelections);

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
            var mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(e.ScreenSpaceMousePosition), Editor.NotesTimeline.TickBarNoteSelections);
            var selectedNoteTime = 0d;
            var selectedNoteCoord = Vector2.Zero;

            foreach (var noteAndTime in SelectedNoteToTime) {
                var note = noteAndTime.Key;
                var noteToTimelineNote = Editor.NotesTimeline.NoteToTimelineNote;
                if (noteToTimelineNote.ContainsKey(note) && IsMouseOnTimelineNote(mousePos, noteToTimelineNote[note])) {
                    ToDrag = DragState.DragTimelineNote;
                    selectedNoteTime = note.EndTime;
                    break;
                }
            }
            if (ToDrag == DragState.None) {
                foreach (var note in GetVisibleStoryNotes()) {
                    if (IsMouseOnNote(e.ScreenSpaceMousePosition, note)) {
                        ToDrag = DragState.DragNote;
                        selectedNoteCoord = note.Coordinates;
                        break;
                    }
                }
            }

            switch (ToDrag) {
                case DragState.DragTimelineNote:
                    foreach (var noteAndTime in SelectedNoteToTime) {
                        var note = noteAndTime.Key;
                        TimelineNoteToDragPointDelta[note] = note.EndTime - selectedNoteTime;
                    }
                    break;
                case DragState.DragNote:
                    foreach (var noteAndTime in SelectedNoteToTime) {
                        var note = noteAndTime.Key;
                        NoteToDragPointDelta[note] = note.Coordinates - selectedNoteCoord;
                    }
                    break;
                case DragState.None:
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

        public override void OnToolDrag(DragEvent e) {
            switch (ToDrag) {
                case DragState.DragTimelineNote: {
                    var tickBarNoteSelections = Editor.NotesTimeline.TickBarNoteSelections;
                    var mousePosX = ToSpaceOfOtherDrawable(ToLocalSpace(e.ScreenSpaceMousePosition), tickBarNoteSelections).X;
                    // temp until NoteTimeline Scroll on drag is implemented
                    mousePosX = Math.Clamp(mousePosX, 0, tickBarNoteSelections.DrawWidth);
                    var relativeMousePosX = mousePosX / tickBarNoteSelections.DrawWidth;
                    var gameTimeDeltaFromMiddle = (relativeMousePosX - 0.5f) * Editor.NotesTimeline.SectionLength * NotesTimeline.SecondsToMS;
                    var gameTimeAtMouse = Time.Current + gameTimeDeltaFromMiddle;

                    var selectedNoteToTimeCopy = new Dictionary<Note, double>(SelectedNoteToTime);
                    foreach (var noteAndTime in SelectedNoteToTime) {
                        var note = noteAndTime.Key;
                        var newTime = GetClosestTickTime(gameTimeAtMouse) + TimelineNoteToDragPointDelta[note];
                        note.UpdateEndTime(newTime);
                        selectedNoteToTimeCopy[note] = newTime;
                    }
                    SelectedNoteToTime = selectedNoteToTimeCopy;
                    break;
                }
                case DragState.DragNote: {
                    var mousePos = Editor.MousePosition;
                    //var mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(e.ScreenSpaceMousePosition), Editor);

                    var selectedNoteToTimeCopy = new Dictionary<Note, double>(SelectedNoteToTime);
                    foreach (var noteAndTime in SelectedNoteToTime) {
                        var note = noteAndTime.Key;
                        var delta = NoteToDragPointDelta[note];
                        //Console.WriteLine($"Delta: {delta}");
                        var newPos = mousePos + delta;
                        Console.WriteLine($"Current Position: {note.Coordinates}");
                        Console.WriteLine($"New Position: {newPos}");
                        note.UpdateCoordinates(newPos);
                    }
                    SelectedNoteToTime = selectedNoteToTimeCopy;
                    break;
                }
                case DragState.None:
                    break;
            }
        }

        public override void OnToolDragEnd(DragEndEvent _) => ToDrag = DragState.None;

        public override bool OnToolKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.Delete:
                    Editor.NoteSelectionIndicators.Clear();
                    foreach (var noteAndTime in SelectedNoteToTime) {
                        var note = noteAndTime.Key;
                        Editor.Reversibles.Push(new ReversibleRemoveNote(Story, note));
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
