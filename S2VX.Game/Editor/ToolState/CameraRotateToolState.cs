using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osuTK;
using S2VX.Game.Editor.Reversible;
using S2VX.Game.Story;
using System;

namespace S2VX.Game.Editor.ToolState {
    public class CameraRotateToolState : S2VXToolState {
        private double OldTime { get; set; }
        private float OldRotation { get; set; }

        [Resolved]
        private S2VXEditor Editor { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        public override bool OnToolDragStart(DragStartEvent e) {
            OldTime = Editor.Track.CurrentTime;
            OldRotation = Story.Camera.Rotation;
            return true;
        }

        public override void OnToolDragEnd(DragEndEvent e) {
            var endTime = Editor.Track.CurrentTime;

            var oldPosition = e.MouseDownPosition;
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
            var radiansBetween = direction * Math.Acos(dot / magnitude);
            var degreesBetween = radiansBetween * 180 / Math.PI;

            var endValue = (float)(OldRotation + degreesBetween);
            var reversible = new ReversibleAddCommand(Story, new CameraRotateCommand() {
                StartTime = OldTime,
                EndTime = endTime,
                StartValue = OldRotation,
                EndValue = endValue
            });
            Editor.Reversibles.Push(reversible);
        }

        public override string DisplayName() => "Camera Rotate";
    }
}
