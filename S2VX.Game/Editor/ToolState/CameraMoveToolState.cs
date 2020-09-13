using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osuTK;
using S2VX.Game.Editor.Reversible;
using S2VX.Game.Story;

namespace S2VX.Game.Editor.ToolState {
    public class CameraMoveToolState : S2VXToolState {
        private double OldTime { get; set; }
        private Vector2 OldPosition { get; set; }

        [Resolved]
        private S2VXEditor Editor { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        public override bool OnToolDragStart(DragStartEvent e) {
            OldTime = Editor.Track.CurrentTime;
            OldPosition = Story.Camera.Position;
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

            var camera = Story.Camera;
            var oldPosition = e.MouseDownPosition;
            var newPosition = e.MousePosition;
            var relativeOldPosition = (oldPosition - Story.DrawSize / 2) / Story.DrawWidth;
            var relativeNewPosition = (newPosition - Story.DrawSize / 2) / Story.DrawWidth;
            var diffPosition = relativeNewPosition - relativeOldPosition;

            var rotatedPosition = S2VXUtils.Rotate(diffPosition, -camera.Rotation);
            var scaledPosition = rotatedPosition * (1 / camera.Scale.X);
            var endValue = OldPosition + scaledPosition;
            var reversible = new ReversibleAddCommand(Story, new CameraMoveCommand() {
                StartTime = startTime,
                EndTime = endTime,
                StartValue = OldPosition,
                EndValue = endValue
            });
            Editor.Reversibles.Push(reversible);
            return;
        }

        public override string DisplayName() => "Camera Move";
    }
}
