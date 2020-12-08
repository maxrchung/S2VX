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
            SetReleaseAndIndicatorLineAlpha(0);
            AlwaysPresent = true;
            RelativeSizeAxes = Axes.Both;
            var lines = InternalChildren.ToArray();
            ClearInternal(false);
            InternalChildren = lines.Concat(ReleaseLines).Concat(HoldIndicatorLines).ToArray();
        }

        private void SetReleaseAndIndicatorLineAlpha(float alpha) {
            ReleaseLines.ForEach(l => l.Alpha = alpha);
            HoldIndicatorLines.ForEach(l => l.Alpha = alpha);
        }

        public override void UpdateApproach() {
            var time = Time.Current;
            var clampedTime = MathHelper.Clamp(time, HitTime, EndTime);
            var coordinates = Interpolation.ValueAt(clampedTime, Coordinates, EndCoordinates, HitTime, EndTime);
            UpdatePlacement(coordinates);

            var notes = Story.Notes;
            var camera = Story.Camera;
            var approaches = Story.Approaches;

            var endFadeOut = EndTime + notes.FadeOutTime;

            if (time >= endFadeOut) {
                SetReleaseAndIndicatorLineAlpha(0);
                // Return early to save some calculations
                return;
            }

            if (time >= HitTime) {
                // Keep the hit approach visible for the duration of the hold
                Lines.ForEach(l => l.Alpha = 1);
            }

            var startIndicatorTime = HitTime - notes.ShowTime;
            var startIndicatorFadeIn = startIndicatorTime - notes.FadeInTime;
            var startReleaseTime = EndTime - notes.ShowTime;
            var startReleaseFadeIn = startReleaseTime - notes.FadeInTime;

            var position = camera.Position;
            var rotation = camera.Rotation;
            var scale = camera.Scale;
            var thickness = approaches.Thickness;

            var offset = S2VXUtils.Rotate(coordinates - position, rotation) * scale;

            clampedTime = MathHelper.Clamp(time, startReleaseFadeIn, EndTime);
            var distance = Interpolation.ValueAt(clampedTime, approaches.Distance, scale.X / 2, startReleaseFadeIn, EndTime);
            var rotationX = S2VXUtils.Rotate(new Vector2(distance, 0), rotation);
            var rotationY = S2VXUtils.Rotate(new Vector2(0, distance), rotation);

            // Add extra thickness so corners overlap
            var overlap = distance * 2 + thickness;

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

            float alpha;
            if (time >= EndTime) {
                alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, EndTime, endFadeOut);
            } else if (time >= startIndicatorTime) {
                alpha = 1;
            } else {
                alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startIndicatorFadeIn, startIndicatorTime);
            }
            HoldIndicatorLines.ForEach(l => l.Alpha = alpha);
            ReleaseLines.ForEach(l => l.Alpha = alpha);
        }
    }
}
