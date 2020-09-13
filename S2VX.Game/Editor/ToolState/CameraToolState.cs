using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;
using S2VX.Game.Editor.Reversible;
using S2VX.Game.Story;
using System;

namespace S2VX.Game.Editor.ToolState {
    public class CameraToolState : S2VXToolState {
        private CameraToolDragState DragState = CameraToolDragState.Move;

        private double OldTime { get; set; }
        private Vector2 OldPosition { get; set; }
        private Vector2 OldScale { get; set; }
        private float OldRotation { get; set; }

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

            var camera = Story.Camera;
            OldPosition = camera.Position;
            OldScale = camera.Scale;
            OldRotation = camera.Rotation;
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

            switch (DragState) {
                case CameraToolDragState.Move: {
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
                case CameraToolDragState.Scale: {
                    var length = diffPosition.Length;
                    var endValue = new Vector2(length);
                    var reversible = new ReversibleAddCommand(Story, new CameraScaleCommand() {
                        StartTime = startTime,
                        EndTime = endTime,
                        StartValue = OldScale,
                        EndValue = endValue
                    });
                    Editor.Reversibles.Push(reversible);
                    return;
                }
                case CameraToolDragState.Rotate: {
                    var dot = Vector2.Dot(relativeOldPosition, relativeNewPosition);
                    var magnitude = relativeOldPosition.Length * relativeNewPosition.Length;

                    // Using cross product to determine angle direction
                    // http://stackoverflow.com/questions/11022446/direction-of-shortest-rotation-between-two-vectors
                    var cross = Vector3.Cross(
                        new Vector3(relativeOldPosition.X, relativeOldPosition.Y, 0),
                        new Vector3(relativeNewPosition.X, relativeNewPosition.Y, 0)
                    );
                    var direction = cross.Z > 0 ? 1 : -1;
                    var radiansBetween = direction * Math.Acos(dot / magnitude);
                    var degreesBetween = radiansBetween * 180 / Math.PI;

                    var endValue = (float)(OldRotation + degreesBetween);
                    var reversible = new ReversibleAddCommand(Story, new CameraRotateCommand() {
                        StartTime = startTime,
                        EndTime = endTime,
                        StartValue = OldRotation,
                        EndValue = endValue
                    });
                    Editor.Reversibles.Push(reversible);
                    return;
                }
            }
        }

        public override string DisplayName() => $"Camera {DragState}";
    }
}
