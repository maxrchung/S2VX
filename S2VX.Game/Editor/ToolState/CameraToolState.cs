using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;
using S2VX.Game.Editor.Reversible;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;
using System;

namespace S2VX.Game.Editor.ToolState {
    public class CameraToolState : S2VXToolState {
        private double OldTime { get; set; }
        private Vector2 OldPosition { get; set; }
        private Vector2 OldScale { get; set; }
        private float OldRotation { get; set; }
        private bool IsRecording { get; set; }
        private CameraMoveCommand MoveCommand { get; set; }
        private CameraScaleCommand ScaleCommand { get; set; }
        private CameraRotateCommand RotateCommand { get; set; }

        [Resolved]
        private S2VXEditor Editor { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }


        public override bool OnToolMouseDown(MouseDownEvent e) => true;

        public override void OnToolMouseMove(MouseMoveEvent e) { }

        public override void OnToolMouseUp(MouseUpEvent e) {
            if (!IsRecording) {
                return;
            }

            var endTime = Editor.Track.CurrentTime;

            // Transform mouse position into editor coordinates
            var camera = Story.Camera;
            var oldPosition = e.MouseDownPosition;
            var newPosition = e.MousePosition;
            var relativeOldPosition = (oldPosition - Story.DrawSize / 2) / Story.DrawWidth;
            var relativeNewPosition = (newPosition - Story.DrawSize / 2) / Story.DrawWidth;
            var diffPosition = relativeNewPosition - relativeOldPosition;

            switch (e.Button) {
                case MouseButton.Left:
                    UpdateCameraMoveCommand(camera, diffPosition, endTime);
                    break;
                case MouseButton.Middle:
                    UpdateCameraScaleCommand(diffPosition, endTime);
                    break;
                case MouseButton.Right:
                    UpdateCameraRotateCommand(relativeOldPosition, relativeNewPosition, endTime);
                    break;
                default:
                    break;
            }
        }

        public override bool OnToolKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.S:
                    IsRecording = true;
                    InitializeStartParams();
                    return true;
                case Key.E:
                    if (!IsRecording) {
                        return true;
                    }
                    IsRecording = false;
                    CommitCameraToolActions();
                    return true;
                default:
                    break;
            }
            return false;
        }

        public override string DisplayName() => IsRecording ? "Camera (E to end)" : "Camera (S to start)";

        private void InitializeStartParams() {
            MoveCommand = null;
            ScaleCommand = null;
            RotateCommand = null;

            var camera = Story.Camera;
            OldTime = Editor.Track.CurrentTime;
            OldPosition = camera.Position;
            OldScale = camera.Scale;
            OldRotation = camera.Rotation;
        }

        private void CommitCameraToolActions() {
            var reversible = new ReversibleCameraToolActions(Story, MoveCommand, ScaleCommand, RotateCommand);
            Editor.Reversibles.Push(reversible);
        }

        private void UpdateCameraMoveCommand(Camera camera, Vector2 diffPosition, double endTime) {
            var rotatedPosition = S2VXUtils.Rotate(diffPosition, -camera.Rotation);
            var scaledPosition = rotatedPosition * (1 / camera.Scale.X);
            var endValue = OldPosition + scaledPosition;
            MoveCommand = new CameraMoveCommand() {
                StartTime = OldTime,
                EndTime = endTime,
                StartValue = OldPosition,
                EndValue = endValue
            };
        }

        private void UpdateCameraScaleCommand(Vector2 diffPosition, double endTime) {
            var length = diffPosition.Length;
            var endValue = new Vector2(length);
            ScaleCommand = new CameraScaleCommand() {
                StartTime = OldTime,
                EndTime = endTime,
                StartValue = OldScale,
                EndValue = endValue
            };
        }

        private void UpdateCameraRotateCommand(Vector2 relativeOldPosition, Vector2 relativeNewPosition, double endTime) {
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
            RotateCommand = new CameraRotateCommand() {
                StartTime = OldTime,
                EndTime = endTime,
                StartValue = OldRotation,
                EndValue = endValue
            };
        }
    }
}
