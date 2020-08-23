using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osuTK.Graphics;
using S2VX.Game.Story;
using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osuTK;
//using osuTK.Input;


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

        public override bool OnToolMouseDown(MouseDownEvent _) {
            SelectedNoteToTime.Clear();
            Editor.NoteSelectionIndicators.Clear();
            var mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(_.ScreenSpaceMousePosition), Editor.NotesTimeline.TickBar);
            Console.WriteLine(mousePos);
            foreach (var notes in Editor.NotesTimeline.NoteToTimelineNote) {
                if (MouseIsOnNote(mousePos, notes.Value)) {
                    SelectedNoteToTime[notes.Key] = notes.Key.EndTime;
                    var noteSelection = new RelativeBox {
                        Colour = Color4.LimeGreen.Opacity(0.5f),
                        Width = notes.Key.Size.X + SelectionIndicatorThickness,
                        Height = notes.Key.Size.Y + SelectionIndicatorThickness,
                        RelativePositionAxes = Axes.Both,
                        RelativeSizeAxes = Axes.Both,
                        Rotation = notes.Key.Rotation,
                    };
                    noteSelection.X = notes.Key.Position.X;
                    noteSelection.Y = notes.Key.Position.Y;
                    Editor.NoteSelectionIndicators.Add(noteSelection);
                    NoteSelectionToNote[noteSelection] = notes.Key;
                    //Console.WriteLine(notes.Key.EndTime);
                    Console.WriteLine($"Actual Note Coords: {notes.Key.DrawPosition}");
                    return false;
                }
            }
            return false;
        }

        public override bool OnToolDragStart(DragStartEvent _) {
            var mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(_.ScreenSpaceMousePosition), Editor.NotesTimeline.TickBar);
            var relativeMousePosX = mousePos.X / Editor.NotesTimeline.TickBar.DrawWidth;
            var gameTimeDeltaFromMiddle = (relativeMousePosX - 0.5f) * Editor.NotesTimeline.SectionLength * 1000;
            var gameTimeAtMouse = Story.GameTime + gameTimeDeltaFromMiddle;

            foreach (var noteAndTime in SelectedNoteToTime) {
                var note = noteAndTime.Key;
                NoteToDragPointDelta[note] = note.EndTime - gameTimeAtMouse;
                if (MouseIsOnNote(mousePos, Editor.NotesTimeline.NoteToTimelineNote[note])) {
                    DragTimelineNote = true;
                }
            }
            return true;
        }

        public override void OnToolDrag(DragEvent _) {
            if (DragTimelineNote) {
                var mousePosX = ToSpaceOfOtherDrawable(ToLocalSpace(_.ScreenSpaceMousePosition), Editor.NotesTimeline.TickBar).X;
                mousePosX = Math.Clamp(mousePosX, 0, Editor.NotesTimeline.TickBar.DrawWidth); // temp until NoteTimeline Scroll on drag is implemented
                var relativeMousePosX = mousePosX / Editor.NotesTimeline.TickBar.DrawWidth;
                var gameTimeDeltaFromMiddle = (relativeMousePosX - 0.5f) * Editor.NotesTimeline.SectionLength * 1000;
                var gameTimeAtMouse = Story.GameTime + gameTimeDeltaFromMiddle;

                var selectedNoteToTimeCopy = new Dictionary<Note, double>(SelectedNoteToTime);
                foreach (var noteAndTime in SelectedNoteToTime) {
                    var note = noteAndTime.Key;
                    var newTime = gameTimeAtMouse + NoteToDragPointDelta[note];
                    note.EndTime = newTime;
                    selectedNoteToTimeCopy[note] = newTime;
                }
                SelectedNoteToTime = selectedNoteToTimeCopy;
            }
        }

        public override void OnToolDragEnd(DragEndEvent _) => DragTimelineNote = false;

        //public override bool OnToolKeyDown(KeyDownEvent e) {
        //    //switch (e.Key) {
        //    //    case Key.Delete:
        //    //        break;
        //    //}
        //    Console.WriteLine("");
        //    return true;
        //}

        public override void HandleExit() {
            Editor.NotesTimeline.TickBar.RemoveAll(item => item.Name == "TimelineSelection");
            Editor.NoteSelectionIndicators.Clear();
        }

        protected override void Update() {
            Editor.NotesTimeline.TickBar.RemoveAll(item => item.Name == "TimelineSelection");
            var lowerBound = Story.GameTime - Editor.NotesTimeline.SectionLength * 1000 / 2;
            var upperBound = Story.GameTime + Editor.NotesTimeline.SectionLength * 1000 / 2;
            foreach (var noteAndTime in SelectedNoteToTime) {
                if (lowerBound <= noteAndTime.Value && noteAndTime.Value <= upperBound) {
                    var relativePosition = (noteAndTime.Value - lowerBound) / (Editor.NotesTimeline.SectionLength * 1000);
                    var indication = new RelativeBox {
                        Name = "TimelineSelection",
                        Colour = Color4.LimeGreen.Opacity(0.727f),
                        Width = NotesTimeline.TimelineNoteWidth + 0.009727f,
                        Height = NotesTimeline.TimelineNoteHeight + 0.1f,
                        X = (float)relativePosition,
                        Y = 0.2f,
                        Anchor = Anchor.TopLeft,
                        RelativePositionAxes = Axes.Both,
                        RelativeSizeAxes = Axes.Both,
                    };
                    Editor.NotesTimeline.TickBar.Add(indication);
                    //Console.WriteLine(indication.DrawPosition);
                }
            }
            foreach (var noteSelection in Editor.NoteSelectionIndicators) {
                noteSelection.Rotation = NoteSelectionToNote[noteSelection].Rotation;
                noteSelection.X = NoteSelectionToNote[noteSelection].Position.X;
                noteSelection.Y = NoteSelectionToNote[noteSelection].Position.Y;
            }
        }
    }
}