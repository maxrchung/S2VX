using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Utils;
using osuTK;
using System;

namespace S2VX.Game.Story {
    public class Approach : CompositeDrawable {
        public double EndTime { get; set; } = 0;
        public Vector2 Coordinates { get; set; } = Vector2.Zero;

        [Resolved]
        private S2VXStory Story { get; set; } = null;

        private RelativeBox[] Lines { get; set; } = new RelativeBox[4]
        {
            new RelativeBox(), // up
            new RelativeBox(), // down
            new RelativeBox(), // right
            new RelativeBox() // left
        };

        [BackgroundDependencyLoader]
        private void Load() {
            Alpha = 0;
            AlwaysPresent = true;
            RelativeSizeAxes = Axes.Both;
            InternalChildren = Lines;
        }

        protected override void Update() {
            var notes = Story.Notes;
            var camera = Story.Camera;
            var approaches = Story.Approaches;

            var time = Story.GameTime;
            var endFadeOut = EndTime + notes.FadeOutTime;

            if (time >= endFadeOut) {
                Alpha = 0;
                // Return early to save some calculations
                return;
            }

            var startTime = EndTime - notes.ShowTime;
            var startFadeIn = startTime - notes.FadeInTime;

            var position = camera.Position;
            var rotation = camera.Rotation;
            var scale = camera.Scale;
            var thickness = approaches.Thickness;

            var offset = Utils.Rotate(Coordinates - position, rotation) * scale;

            var distance = time < EndTime
                ? Interpolation.ValueAt(time, approaches.Distance, scale.X / 2, startFadeIn, EndTime)
                : scale.X / 2;
            var rotationX = Utils.Rotate(new Vector2(distance, 0), rotation);
            var rotationY = Utils.Rotate(new Vector2(0, distance), rotation);

            // Add extra thickness so corners overlap
            var overlap = distance * 2 + thickness;

            Lines[0].Position = offset + rotationY;
            Lines[0].Rotation = rotation;
            Lines[0].Size = new Vector2(overlap, thickness);

            Lines[1].Position = offset - rotationY;
            Lines[1].Rotation = rotation;
            Lines[1].Size = new Vector2(overlap, thickness);

            Lines[2].Position = offset + rotationX;
            Lines[2].Rotation = rotation;
            Lines[2].Size = new Vector2(thickness, overlap);

            Lines[3].Position = offset - rotationX;
            Lines[3].Rotation = rotation;
            Lines[3].Size = new Vector2(thickness, overlap);

            if (time >= EndTime) {
                var alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, EndTime, endFadeOut);
                Alpha = alpha;
            } else if (time >= startTime) {
                Alpha = 1;
            } else {
                var alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startFadeIn, startTime);
                Alpha = alpha;
            }
        }
    }
}
