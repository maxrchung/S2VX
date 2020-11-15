using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Utils;
using osuTK;
using System;
using System.Collections.Generic;

namespace S2VX.Game.Story.Note {
    public class Approach : CompositeDrawable, IComparable<Approach> {
        public double HitTime { get; set; }
        public Vector2 Coordinates { get; set; } = Vector2.Zero;

        [Resolved]
        private S2VXStory Story { get; set; } = null;

        public List<RelativeBox> Lines { get; } = new List<RelativeBox>()
        {
            new RelativeBox(), // up
            new RelativeBox(), // down
            new RelativeBox(), // right
            new RelativeBox()  // left
        };

        public Vector2 HitApproachTopLeftCorner { get; set; }
        public Vector2 HitApproachTopRightCorner { get; set; }
        public Vector2 HitApproachBottomLeftCorner { get; set; }
        public Vector2 HitApproachBottomRightCorner { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            Lines.ForEach(l => l.Alpha = 0);
            AlwaysPresent = true;
            RelativeSizeAxes = Axes.Both;
            InternalChildren = Lines;
        }

        protected override void Update() {
            var notes = Story.Notes;
            var camera = Story.Camera;
            var approaches = Story.Approaches;

            var time = Time.Current;
            var endFadeOut = HitTime + notes.FadeOutTime;

            var startTime = HitTime - notes.ShowTime;
            var startFadeIn = startTime - notes.FadeInTime;

            var position = camera.Position;
            var rotation = camera.Rotation;
            var scale = camera.Scale;
            var thickness = approaches.Thickness;

            var offset = S2VXUtils.Rotate(Coordinates - position, rotation) * scale;

            var distance = time < HitTime
                ? Interpolation.ValueAt(time, approaches.Distance, scale.X / 2, startFadeIn, HitTime)
                : scale.X / 2;
            var rotationX = S2VXUtils.Rotate(new Vector2(distance, 0), rotation);
            var rotationY = S2VXUtils.Rotate(new Vector2(0, distance), rotation);

            // Add extra thickness so corners overlap
            var overlap = distance * 2 + thickness;

            HitApproachTopLeftCorner = offset - rotationX - rotationY;
            HitApproachTopRightCorner = offset + rotationX - rotationY;
            HitApproachBottomLeftCorner = offset - rotationX + rotationY;
            HitApproachBottomRightCorner = offset + rotationX + rotationY;

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

            float alpha;
            if (time >= HitTime) {
                alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, HitTime, endFadeOut);
            } else if (time >= startTime) {
                alpha = 1;
            } else {
                alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startFadeIn, startTime);
            }
            Lines.ForEach(l => l.Alpha = alpha);

        }

        // Sort Approaches from highest end time to lowest end time
        public int CompareTo(Approach other) => other.HitTime.CompareTo(HitTime);
    }
}
