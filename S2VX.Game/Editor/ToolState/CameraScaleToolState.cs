using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osuTK;
using S2VX.Game.Editor.Reversible;
using S2VX.Game.Story;

namespace S2VX.Game.Editor.ToolState {
    public class CameraScaleToolState : S2VXToolState {
        private double OldTime { get; set; }
        private Vector2 OldScale { get; set; }

        [Resolved]
        private S2VXEditor Editor { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        public override bool OnToolDragStart(DragStartEvent e) {
            OldTime = Editor.Track.CurrentTime;
            OldScale = Story.Camera.Scale;
            return true;
        }

        public override void OnToolDragEnd(DragEndEvent e) {
            var time = Editor.Track.CurrentTime;
            var startTime = OldTime;
            var endTime = time;
            // Allows commands when going backwards or forwards
            if (time < OldTime) {
                startTime = time;
                endTime = OldTime;
            }

            var oldPosition = e.MouseDownPosition;
            var newPosition = e.MousePosition;
            var relativeOldPosition = (oldPosition - Story.DrawSize / 2) / Story.DrawWidth;
            var relativeNewPosition = (newPosition - Story.DrawSize / 2) / Story.DrawWidth;
            var diffPosition = relativeNewPosition - relativeOldPosition;

            var length = diffPosition.Length;
            var endValue = new Vector2(length);
            var reversible = new ReversibleAddCommand(Story, new CameraScaleCommand() {
                StartTime = startTime,
                EndTime = endTime,
                StartValue = OldScale,
                EndValue = endValue
            });
            Editor.Reversibles.Push(reversible);
        }

        public override string DisplayName() => "Camera Scale";
    }
}
