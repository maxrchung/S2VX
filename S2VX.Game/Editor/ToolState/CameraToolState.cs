using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;
using S2VX.Game.Story;
using System;

namespace S2VX.Game.Editor.ToolState {
    public class CameraToolState : S2VXToolState {
        private CameraToolDragState DragState = CameraToolDragState.Move;
        private double OldTime { get; set; }

        [Resolved]
        private S2VXEditor Editor { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        public override bool OnToolKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.ControlLeft:
                    DragState = CameraToolDragState.Move;
                    return true;
                case Key.ShiftLeft:
                    DragState = CameraToolDragState.Scale;
                    return true;
                case Key.AltLeft:
                    DragState = CameraToolDragState.Rotate;
                    return true;
            }
            return false;
        }

        public override bool OnToolDragStart(DragStartEvent e) {
            OldTime = Editor.Track.CurrentTime;
            return true;
        }

        public override void OnToolDragEnd(DragEndEvent e) {
            var time = Editor.Track.CurrentTime;
            // Don't add command if at the same time
            if (time == OldTime) {
                return;
            }

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

            switch (DragState) {
                case CameraToolDragState.Move:
                    return;
                case CameraToolDragState.Scale:
                    return;
                case CameraToolDragState.Rotate:
                    return;
            }
        }

        public override string DisplayName() => $"Camera {DragState}";
    }
}
