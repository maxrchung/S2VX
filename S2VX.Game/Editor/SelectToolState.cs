using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osuTK.Graphics;
using S2VX.Game.Story;
using System;
using System.Collections.Generic;
using osu.Framework.Graphics;

namespace S2VX.Game.Editor {
    public class SelectToolState : ToolState {
        [Resolved]
        private S2VXStory Story { get; set; } = null;
        [Resolved]
        private S2VXEditor Editor { get; set; } = null;
        private List<double> SelectedNoteTimes { get; set; } = new List<double>();

        public override bool OnToolClick(ClickEvent _) {
            SelectedNoteTimes.Clear();
            var mousePos = ToSpaceOfOtherDrawable(ToLocalSpace(_.ScreenSpaceMousePosition), Editor.NotesTimeline.TickBar);
            Console.WriteLine(mousePos);
            foreach (var note in Editor.NotesTimeline.TimeToVisibleNotes) {
                var leftBound = note.Value.DrawPosition.X - note.Value.DrawSize.X / 2;
                var rightBound = note.Value.DrawPosition.X + note.Value.DrawSize.X / 2;
                var topBound = note.Value.DrawPosition.Y - note.Value.DrawSize.Y / 2;
                var bottomBound = note.Value.DrawPosition.Y + note.Value.DrawSize.Y / 2;

                var mouseInXRange = leftBound <= mousePos.X && mousePos.X <= rightBound;
                var mouseInYRange = topBound <= mousePos.Y && mousePos.Y <= bottomBound;
                if (mouseInXRange && mouseInYRange) {
                    SelectedNoteTimes.Add(note.Key);
                    Console.WriteLine(note.Key);
                    return false;
                }
            }
            return false;
        }

        protected override void Update() {
            Editor.NotesTimeline.TickBar.RemoveAll(item => item.Name == "Selection");
            var lowerBound = Story.GameTime - Editor.NotesTimeline.SectionLength * 1000 / 2;
            var upperBound = Story.GameTime + Editor.NotesTimeline.SectionLength * 1000 / 2;
            foreach (var time in SelectedNoteTimes) {
                if (lowerBound <= time && time <= upperBound) {
                    var relativePosition = (time - lowerBound) / (Editor.NotesTimeline.SectionLength * 1000);
                    var indication = new RelativeBox {
                        Name = "Selection",
                        Colour = Color4.Red,
                        Width = 1.0f / 7.5f,
                        Height = .8f,
                        X = (float)relativePosition,
                        Y = 0.2f,
                        Anchor = Anchor.TopLeft,
                        RelativePositionAxes = Axes.Both,
                        RelativeSizeAxes = Axes.Both,
                    };
                    Editor.NotesTimeline.TickBar.Add(indication);
                    Console.WriteLine(indication.DrawPosition);
                }
            }
        }
    }
}