using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Utils;
using osuTK;
using System.Collections.Generic;
using System.Linq;

namespace S2VX.Game.Story.Note {
    public class HoldApproach : Approach {
        public double EndTime { get; set; }

        public Vector2 EndCoordinates { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; } = null;

        private List<RelativeBox> ReleaseLines { get; } = new List<RelativeBox>()
        {
            new RelativeBox(), // up
            new RelativeBox(), // down
            new RelativeBox(), // right
            new RelativeBox()  // left
        };

        private List<RelativeBox> HoldIndicatorLines { get; set; } = new List<RelativeBox>()
        {
            new RelativeBox(), // top left
            new RelativeBox(), // top right
            new RelativeBox(), // bottom left
            new RelativeBox()  // bottom right
        };

        private Vector2 ReleaseApproachTopLeftCorner { get; set; }
        private Vector2 ReleaseApproachTopRightCorner { get; set; }
        private Vector2 ReleaseApproachBottomLeftCorner { get; set; }
        private Vector2 ReleaseApproachBottomRightCorner { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            AlwaysPresent = true;
            RelativeSizeAxes = Axes.Both;
            var lines = InternalChildren.ToArray();
            ClearInternal(false);
            InternalChildren = lines.Concat(ReleaseLines).Concat(HoldIndicatorLines).ToArray();
        }

        private void SetLineAlpha(float alpha) {
            Lines.ForEach(l => l.Alpha = alpha);
            ReleaseLines.ForEach(l => l.Alpha = alpha);
            HoldIndicatorLines.ForEach(l => l.Alpha = alpha);
        }

        public override void UpdateApproach() {
            UpdateColor();
            UpdatePosition();
        }

        protected override void UpdateColor() {
            var time = Time.Current;
            var notes = Story.Notes;
            float alpha;
            // Fade in time to Show time
            if (time < HitTime - notes.ShowTime) {
                var startTime = HitTime - notes.ShowTime - notes.FadeInTime;
                var endTime = HitTime - notes.ShowTime;
                alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startTime, endTime);
            }
            // Show time to End time
            else if (time < EndTime) {
                alpha = 1;
            }
            // End time to Fade out time
            else if (time < EndTime + notes.FadeOutTime) {
                var startTime = HitTime;
                var endTime = HitTime + notes.FadeOutTime;
                alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, startTime, endTime);
            } else {
                alpha = 0;
            }
            Lines.ForEach(l => l.Alpha = alpha);
            ReleaseLines.ForEach(l => l.Alpha = alpha);
            HoldIndicatorLines.ForEach(l => l.Alpha = alpha);
        }

        protected override void UpdatePosition() {
            var notes = Story.Notes;
            var camera = Story.Camera;
            var approaches = Story.Approaches;
            var position = camera.Position;
            var rotation = camera.Rotation;
            var scale = camera.Scale;
            var thickness = approaches.Thickness;

            var time = Time.Current;
            var clampedTime = MathHelper.Clamp(time, HitTime, EndTime);
            var coordinates = Interpolation.ValueAt(clampedTime, Coordinates, EndCoordinates, HitTime, EndTime);
            UpdateInnerApproachPosition(coordinates);

            // Calculate outer approach values
            var startTime = EndTime - notes.ShowTime - notes.FadeInTime;
            clampedTime = MathHelper.Clamp(time, startTime, EndTime);
            var distance = Interpolation.ValueAt(clampedTime, approaches.Distance, scale.X / 2, startTime, EndTime);
            var rotationX = S2VXUtils.Rotate(new Vector2(distance, 0), rotation);
            var rotationY = S2VXUtils.Rotate(new Vector2(0, distance), rotation);
            // Add extra thickness so corners overlap
            var overlap = distance * 2 + thickness;

            var offset = S2VXUtils.Rotate(coordinates - position, rotation) * scale;
            ReleaseLines[0].Position = offset + rotationY;
            ReleaseLines[0].Rotation = rotation;
            ReleaseLines[0].Size = new Vector2(overlap, thickness);

            ReleaseLines[1].Position = offset - rotationY;
            ReleaseLines[1].Rotation = rotation;
            ReleaseLines[1].Size = new Vector2(overlap, thickness);

            ReleaseLines[2].Position = offset + rotationX;
            ReleaseLines[2].Rotation = rotation;
            ReleaseLines[2].Size = new Vector2(thickness, overlap);

            ReleaseLines[3].Position = offset - rotationX;
            ReleaseLines[3].Rotation = rotation;
            ReleaseLines[3].Size = new Vector2(thickness, overlap);

            ReleaseApproachTopLeftCorner = offset - rotationX - rotationY;
            ReleaseApproachTopRightCorner = offset + rotationX - rotationY;
            ReleaseApproachBottomLeftCorner = offset - rotationX + rotationY;
            ReleaseApproachBottomRightCorner = offset + rotationX + rotationY;

            var topLeftPosition = (ReleaseApproachTopLeftCorner + HitApproachTopLeftCorner) / 2;
            var topRightPosition = (ReleaseApproachTopRightCorner + HitApproachTopRightCorner) / 2;
            var bottomLeftPosition = (ReleaseApproachBottomLeftCorner + HitApproachBottomLeftCorner) / 2;
            var bottomRightPosition = (ReleaseApproachBottomRightCorner + HitApproachBottomRightCorner) / 2;
            var indicatorLength = Vector2.Distance(ReleaseApproachTopLeftCorner, HitApproachTopLeftCorner);

            HoldIndicatorLines[0].Position = topLeftPosition;
            HoldIndicatorLines[0].Rotation = rotation - 45;
            HoldIndicatorLines[0].Size = new Vector2(thickness, indicatorLength);

            HoldIndicatorLines[1].Position = topRightPosition;
            HoldIndicatorLines[1].Rotation = rotation + 45;
            HoldIndicatorLines[1].Size = new Vector2(thickness, indicatorLength);

            HoldIndicatorLines[2].Position = bottomLeftPosition;
            HoldIndicatorLines[2].Rotation = rotation + 45;
            HoldIndicatorLines[2].Size = new Vector2(thickness, indicatorLength);

            HoldIndicatorLines[3].Position = bottomRightPosition;
            HoldIndicatorLines[3].Rotation = rotation - 45;
            HoldIndicatorLines[3].Size = new Vector2(thickness, indicatorLength);
        }
    }
}
