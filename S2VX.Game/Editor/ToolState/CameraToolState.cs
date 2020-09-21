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

        private Vector2 OldMousePositionMove { get; set; }
        private Vector2 OldMousePositionScale { get; set; }
        private Vector2 OldMousePositionRotate { get; set; }

        private const float ScaleSnapMultiplier = 0.1f;
        private const float RotationSnapMultiplier = 90;

        [Resolved]
        private S2VXEditor Editor { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        public override bool OnToolMouseDown(MouseDownEvent e) {
            if (IsRecording) {
                if (e.IsPressed(MouseButton.Left)) {
                    OldMousePositionMove = e.MouseDownPosition;
                }
                if (e.IsPressed(MouseButton.Middle)) {
                    OldMousePositionScale = e.MouseDownPosition;
                }
                if (e.IsPressed(MouseButton.Right)) {
                    OldMousePositionRotate = e.MouseDownPosition;
                }
            }
            return true;
        }

        public override void OnToolMouseMove(MouseMoveEvent e) {
            if (IsRecording) {
                if (e.IsPressed(MouseButton.Left)) {
                    Story.Camera.Position = CalculateCameraPosition(e);
                }
                if (e.IsPressed(MouseButton.Middle)) {
                    Story.Camera.Scale = CalculateCameraScale(e);
                }
                if (e.IsPressed(MouseButton.Right)) {
                    Story.Camera.Rotation = CalculateCameraRotation(e);
                }
            }
        }

        public override void OnToolMouseUp(MouseUpEvent e) {
            if (!IsRecording) {
                return;
            }
            var endTime = Editor.Track.CurrentTime;

            switch (e.Button) {
                case MouseButton.Left: {
                    var endValue = CalculateCameraPosition(e);
                    MoveCommand = endTime > OldTime
                        ? new CameraMoveCommand() {
                            StartTime = OldTime,
                            EndTime = endTime,
                            StartValue = OldPosition,
                            EndValue = endValue
                        }
                        : new CameraMoveCommand() {
                            StartTime = endTime,
                            EndTime = OldTime,
                            StartValue = OldPosition,
                            EndValue = endValue
                        };
                    break;
                }
                case MouseButton.Middle: {
                    var endValue = CalculateCameraScale(e);
                    ScaleCommand = endTime > OldTime
                        ? new CameraScaleCommand() {
                            StartTime = OldTime,
                            EndTime = endTime,
                            StartValue = OldScale,
                            EndValue = endValue
                        }
                        : new CameraScaleCommand() {
                            StartTime = endTime,
                            EndTime = OldTime,
                            StartValue = endValue,
                            EndValue = OldScale
                        };
                    break;
                }
                case MouseButton.Right: {
                    var endValue = CalculateCameraRotation(e);
                    RotateCommand = endTime > OldTime
                        ? new CameraRotateCommand() {
                            StartTime = OldTime,
                            EndTime = endTime,
                            StartValue = OldRotation,
                            EndValue = endValue
                        }
                        : new CameraRotateCommand() {
                            StartTime = endTime,
                            EndTime = OldTime,
                            StartValue = endValue,
                            EndValue = OldRotation
                        };
                    break;
                }
                default:
                    break;
            }
        }

        public override bool OnToolKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.S:
                    if (IsRecording) {
                        // Add a keyframe (i.e. End and Start again)
                        CommitCameraToolActions();
                    }
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
                case Key.Escape:
                    HandleExit();
                    break;
                default:
                    break;
            }
            return false;
        }

        public override string DisplayName() => IsRecording ? "Camera (S)et, (E)nd, (Esc) Cancel" : "Camera (S to start)";

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

        private Vector2 CalculateCameraPosition(MouseEvent e) {
            // Transform mouse position into editor coordinates
            var camera = Story.Camera;
            var oldPosition = OldMousePositionMove;
            var newPosition = e.MousePosition;
            var relativeOldPosition = (oldPosition - Story.DrawSize / 2) / Story.DrawWidth;
            var relativeNewPosition = (newPosition - Story.DrawSize / 2) / Story.DrawWidth;
            var diffPosition = relativeNewPosition - relativeOldPosition;
            var rotatedPosition = S2VXUtils.Rotate(diffPosition, -camera.Rotation);
            var scaledPosition = rotatedPosition * (1 / camera.Scale.X);
            var endValue = OldPosition - scaledPosition;
            if (Editor.SnapDivisor != 0) {
                endValue = new Vector2(
                    (float)(Math.Round(endValue.X * Editor.SnapDivisor) / Editor.SnapDivisor),
                    (float)(Math.Round(endValue.Y * Editor.SnapDivisor) / Editor.SnapDivisor)
                );
            }
            return endValue;
        }

        private Vector2 CalculateCameraScale(MouseEvent e) {
            // Transform mouse position into editor coordinates
            var oldPosition = OldMousePositionScale;
            var newPosition = e.MousePosition;
            var relativeOldPosition = (oldPosition - Story.DrawSize / 2) / Story.DrawWidth;
            var relativeNewPosition = (newPosition - Story.DrawSize / 2) / Story.DrawWidth;
            var diffPosition = relativeNewPosition - relativeOldPosition;
            var length = diffPosition.Length;
            var endValue = new Vector2(length);
            if (Editor.SnapDivisor != 0) {
                // Clamp so that the minimum scaling is not 0, and thus infinite gridlines are drawn 
                endValue = new Vector2(
                    (float)Math.Clamp(
                        Math.Round(endValue.X * Editor.SnapDivisor / ScaleSnapMultiplier) / Editor.SnapDivisor * ScaleSnapMultiplier,
                        1 / Editor.SnapDivisor, 1),
                    (float)Math.Clamp(
                        Math.Round(endValue.Y * Editor.SnapDivisor / ScaleSnapMultiplier) / Editor.SnapDivisor * ScaleSnapMultiplier,
                        1 / Editor.SnapDivisor, 1)
                );
            }
            return endValue;
        }

        private float CalculateCameraRotation(MouseEvent e) {
            // Transform mouse position into editor coordinates
            var oldPosition = OldMousePositionRotate;
            var newPosition = e.MousePosition;
            var relativeOldPosition = (oldPosition - Story.DrawSize / 2) / Story.DrawWidth;
            var relativeNewPosition = (newPosition - Story.DrawSize / 2) / Story.DrawWidth;
            var dot = Vector2.Dot(relativeOldPosition, relativeNewPosition);
            var magnitude = relativeOldPosition.Length * relativeNewPosition.Length;

            // Using cross product to determine angle direction
            // http://stackoverflow.com/questions/11022446/direction-of-shortest-rotation-between-two-vectors
            var cross = Vector3.Cross(
                new Vector3(relativeOldPosition.X, relativeOldPosition.Y, 0),
                new Vector3(relativeNewPosition.X, relativeNewPosition.Y, 0)
            );
            var direction = cross.Z > 0 ? 1 : -1;
            // Clamp to remove floating point calculation error that causes the value to go slightly outside the range of valid inputs
            var radiansBetween = direction * Math.Acos(Math.Clamp(dot / magnitude, -1, 1));
            var degreesBetween = radiansBetween * 180 / Math.PI;
            var endValue = (float)(OldRotation + degreesBetween);
            if (Editor.SnapDivisor != 0) {
                endValue = (float)(Math.Round(endValue * Editor.SnapDivisor / RotationSnapMultiplier) / Editor.SnapDivisor * RotationSnapMultiplier);
            }
            return endValue;
        }

        public override void HandleExit() {
            IsRecording = false;
            Story.ClearActives();
        }
    }
}
