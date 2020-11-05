using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Utils;
using osuTK;
using System.Collections.Generic;
using System.Linq;

namespace S2VX.Game.Story.Note {
    public class HoldApproach : Approach {
        public double EndTime { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; } = null;

        private List<RelativeBox> ReleaseLines { get; set; } = new List<RelativeBox>()
        {
            new RelativeBox(), // up
            new RelativeBox(), // down
            new RelativeBox(), // right
            new RelativeBox()  // left
        };

        [BackgroundDependencyLoader]
        private void Load() {
            ReleaseLines.ForEach(l => l.Alpha = 0);
            AlwaysPresent = true;
            RelativeSizeAxes = Axes.Both;
            var lines = InternalChildren.ToArray();
            ClearInternal(false);
            InternalChildren = lines.Concat(ReleaseLines).ToArray();
        }

        protected override void Update() {
            base.Update();

            var notes = Story.Notes;
            var camera = Story.Camera;
            var approaches = Story.Approaches;

            var time = Time.Current;
            var endFadeOut = EndTime + notes.FadeOutTime;

            if (time >= endFadeOut) {
                ReleaseLines.ForEach(l => l.Alpha = 0);
                // Return early to save some calculations
                return;
            }

            var startTime = EndTime - notes.ShowTime;
            var startFadeIn = startTime - notes.FadeInTime;

            var position = camera.Position;
            var rotation = camera.Rotation;
            var scale = camera.Scale;
            var thickness = approaches.Thickness;

            var offset = S2VXUtils.Rotate(Coordinates - position, rotation) * scale;

            var distance = time < EndTime
                ? Interpolation.ValueAt(time, approaches.Distance, scale.X / 2, startFadeIn, EndTime)
                : scale.X / 2;
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

            float alpha;
            if (time >= EndTime) {
                alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, EndTime, endFadeOut);
            } else if (time >= startTime) {
                alpha = 1;
            } else {
                alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startFadeIn, startTime);
            }
            ReleaseLines.ForEach(l => l.Alpha = alpha);
        }

    }
}
