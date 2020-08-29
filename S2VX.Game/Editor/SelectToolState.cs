using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using S2VX.Game.Story;
using System;
using System.Collections.Generic;

namespace S2VX.Game.Editor {
    public class SelectToolState : ToolState {
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

        // https://gamedev.stackexchange.com/a/86784
        private Vector2 GetRotatedPoint(Vector2 point, Vector2 origin, float rotation) {
            // origin is centered at (0,0). I convert it so (0,0) is top left
            var originX = origin.X + Editor.DrawWidth / 2;
            var originY = origin.Y + Editor.DrawHeight / 2;

            Console.WriteLine($"Starting Point: {point}");
            var translatedX = point.X - originX;
            var translatedY = point.Y - originY;

            var rotatedX = translatedX * (float)Math.Cos(rotation) - translatedY * (float)Math.Sin(rotation);
            var rotatedY = translatedX * (float)Math.Sin(rotation) + translatedY * (float)Math.Cos(rotation);

            var final = new Vector2(rotatedX + originX, rotatedY + originY);
            Console.WriteLine($"Ending Point: {final}");
            return final;
        }

        private bool MouseIsOnNote(Vector2 mousePos, Note note) {
            var leftTimeBound = note.EndTime - Story.Notes.ShowTime - Story.Notes.FadeInTime;
            var rightTimeBound = note.EndTime + Story.Notes.FadeOutTime;
            var noteVisibleOnEditor = leftTimeBound <= Time.Current && Time.Current <= rightTimeBound;

            if (!noteVisibleOnEditor) {
                return false;
            }

            mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(mousePos), Editor);
            var storyNote = note.SquareNote;

            // https://gamedev.stackexchange.com/a/110233
            var pointA = GetRotatedPoint(storyNote.BoundingBox.TopLeft, storyNote.DrawPosition, storyNote.Rotation);
            var pointB = GetRotatedPoint(storyNote.BoundingBox.TopRight, storyNote.DrawPosition, storyNote.Rotation);
            var pointC = GetRotatedPoint(storyNote.BoundingBox.BottomLeft, storyNote.DrawPosition, storyNote.Rotation);

            Console.WriteLine($"mousePos: {mousePos}");
            Console.WriteLine($"A: {pointA}, B: {pointB}, C: {pointC}");

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

        public override bool OnToolMouseDown(MouseDownEvent e) {
            SelectedNoteToTime.Clear();
            Editor.NoteSelectionIndicators.Clear();
            var mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(e.ScreenSpaceMousePosition), Editor.NotesTimeline.TickBarNoteSelections);
            foreach (var notes in Editor.NotesTimeline.NoteToTimelineNote) {
                if (MouseIsOnNote(mousePos, notes.Value) || MouseIsOnNote(e.ScreenSpaceMousePosition, notes.Key)) {
                    var note = notes.Key;
                    SelectedNoteToTime[notes.Key] = notes.Key.EndTime;
                    var noteSelection = new RelativeBox {
                        Colour = Color4.LimeGreen.Opacity(0.5f),
                        Width = note.SquareNote.Size.X + SelectionIndicatorThickness,
                        Height = note.SquareNote.Size.Y + SelectionIndicatorThickness,
                        RelativePositionAxes = Axes.Both,
                        RelativeSizeAxes = Axes.Both,
                        Rotation = note.SquareNote.Rotation,
                    };
                    noteSelection.X = note.SquareNote.Position.X;
                    noteSelection.Y = note.SquareNote.Position.Y;
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
                    note.EndTime = newTime;
                    selectedNoteToTimeCopy[note] = newTime;
                }
                SelectedNoteToTime = selectedNoteToTimeCopy;
            }
        }

        public override void OnToolDragEnd(DragEndEvent _) => DragTimelineNote = false;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "<Pending>")]
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
