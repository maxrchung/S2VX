using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using S2VX.Game.Editor.Containers;
using S2VX.Game.Story;
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

        private static bool MouseIsOnTimelineNote(Vector2 mousePos, RelativeBox timelineNote) {
            var leftBound = timelineNote.DrawPosition.X - timelineNote.DrawSize.X / 2;
            var rightBound = timelineNote.DrawPosition.X + timelineNote.DrawSize.X / 2;
            var topBound = timelineNote.DrawPosition.Y - timelineNote.DrawSize.Y / 2;
            var bottomBound = timelineNote.DrawPosition.Y + timelineNote.DrawSize.Y / 2;

            var mouseInXRange = leftBound <= mousePos.X && mousePos.X <= rightBound;
            var mouseInYRange = topBound <= mousePos.Y && mousePos.Y <= bottomBound;
            return mouseInXRange && mouseInYRange;
        }

        // https://gamedev.stackexchange.com/a/86784
        private static Vector2 GetRotatedPoint(Vector2 point, Vector2 center, float rotation) {
            var translatedX = point.X - center.X;
            var translatedY = point.Y - center.Y;

            var rotatedX = translatedX * (float)Math.Cos(rotation) - translatedY * (float)Math.Sin(rotation);
            var rotatedY = translatedX * (float)Math.Sin(rotation) + translatedY * (float)Math.Cos(rotation);

            return new Vector2(rotatedX + center.X, rotatedY + center.Y);
        }

        private bool MouseIsOnNote(Vector2 mousePos, Note note) {
            mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(mousePos), Editor);
            var storyNote = note.SquareNote;

            // DrawPosition is centered at (0,0). I convert it so (0,0) starts top left
            var convertedCenterPoint = new Vector2(storyNote.DrawPosition.X + Editor.DrawWidth / 2, storyNote.DrawPosition.Y + Editor.DrawHeight / 2);
            var topLeft = new Vector2(convertedCenterPoint.X - storyNote.DrawWidth / 2, convertedCenterPoint.Y - storyNote.DrawHeight / 2);
            var topRight = new Vector2(convertedCenterPoint.X + storyNote.DrawWidth / 2, convertedCenterPoint.Y - storyNote.DrawHeight / 2);
            var bottomLeft = new Vector2(convertedCenterPoint.X - storyNote.DrawWidth / 2, convertedCenterPoint.Y + storyNote.DrawHeight / 2);

            var rotationRadians = MathUtils.DegreesToRadians(storyNote.Rotation);

            // https://gamedev.stackexchange.com/a/110233
            var pointA = GetRotatedPoint(topLeft, convertedCenterPoint, rotationRadians);
            var pointB = GetRotatedPoint(topRight, convertedCenterPoint, rotationRadians);
            var pointC = GetRotatedPoint(bottomLeft, convertedCenterPoint, rotationRadians);

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
                Width = note.SquareNote.Size.X + SelectionIndicatorThickness,
                Height = note.SquareNote.Size.Y + SelectionIndicatorThickness,
                Rotation = note.SquareNote.Rotation,
            };
            noteSelection.X = note.SquareNote.Position.X;
            noteSelection.Y = note.SquareNote.Position.Y;
            Editor.NoteSelectionIndicators.Add(noteSelection);
            NoteSelectionToNote[noteSelection] = note;
        }

        public override bool OnToolMouseDown(MouseDownEvent e) {
            SelectedNoteToTime.Clear();
            Editor.NoteSelectionIndicators.Clear();
            var mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(e.ScreenSpaceMousePosition), Editor.NotesTimeline.TickBarNoteSelections);

            foreach (var notes in Editor.NotesTimeline.NoteToTimelineNote) {
                if (MouseIsOnTimelineNote(mousePos, notes.Value)) {
                    AddNoteSelection(notes.Key);
                    return false;
                }
            }
            foreach (var note in GetVisibleStoryNotes()) {
                if (MouseIsOnNote(e.ScreenSpaceMousePosition, note)) {
                    AddNoteSelection(note);
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
                if (Editor.NotesTimeline.NoteToTimelineNote.ContainsKey(note) && MouseIsOnTimelineNote(mousePos, Editor.NotesTimeline.NoteToTimelineNote[note])) {
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
                    note.EndTime = newTime;
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
                        Story.DeleteNote(note);
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
                noteSelection.Rotation = NoteSelectionToNote[noteSelection].SquareNote.Rotation;
                noteSelection.X = NoteSelectionToNote[noteSelection].SquareNote.Position.X;
                noteSelection.Y = NoteSelectionToNote[noteSelection].SquareNote.Position.Y;
            }
        }
        public override string DisplayName() => "Select";
    }
}
