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

        protected List<RelativeBox> Lines { get; } = new List<RelativeBox>()
        {
            new RelativeBox(), // up
            new RelativeBox(), // down
            new RelativeBox(), // right
            new RelativeBox()  // left
        };

        protected Vector2 HitApproachTopLeftCorner { get; set; }
        protected Vector2 HitApproachTopRightCorner { get; set; }
        protected Vector2 HitApproachBottomLeftCorner { get; set; }
        protected Vector2 HitApproachBottomRightCorner { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            Lines.ForEach(l => l.Alpha = 0);
            AlwaysPresent = true;
            RelativeSizeAxes = Axes.Both;
            InternalChildren = Lines;
        }

        /// <summary>
        /// Main entrypoint for an approach's Update functionality
        /// </summary>
        public virtual void UpdateApproach() {
            UpdateColor();
            UpdatePosition();
        }

        /// <summary>
        /// Updates an approach's position/rotation/size
        /// </summary>
        protected virtual void UpdatePosition() {
            var notes = Story.Notes;
            var camera = Story.Camera;
            var approaches = Story.Approaches;
            var position = camera.Position;
            var rotation = camera.Rotation;
            var scale = camera.Scale;
            var thickness = approaches.Thickness;

            var time = Time.Current;
            var startTime = HitTime - notes.ShowTime - notes.FadeInTime;
            var clampedTime = MathHelper.Clamp(time, startTime, HitTime);
            var distance = Interpolation.ValueAt(clampedTime, approaches.Distance, scale.X / 2, startTime, HitTime);
            var rotationX = S2VXUtils.Rotate(new Vector2(distance, 0), rotation);
            var rotationY = S2VXUtils.Rotate(new Vector2(0, distance), rotation);

            // Add extra thickness so corners overlap
            var overlap = distance * 2 + thickness;

            var offset = S2VXUtils.Rotate(Coordinates - position, rotation) * scale;
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
        }

        /// <summary>
        /// Updates an approach's color/alpha
        /// </summary>
        protected virtual void UpdateColor() {
            var time = Time.Current;
            var notes = Story.Notes;
            float alpha;
            // Fade in time to Show time
            if (time < HitTime - notes.ShowTime) {
                var startTime = HitTime - notes.ShowTime - notes.FadeInTime;
                var endTime = HitTime - notes.ShowTime;
                alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startTime, endTime);
            }
            // Show time to Hit time
            else if (time < HitTime) {
                alpha = 1;
            }
            // Hit time to Fade out time
            else if (time < HitTime + notes.FadeOutTime) {
                var startTime = HitTime;
                var endTime = HitTime + notes.FadeOutTime;
                alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, startTime, endTime);
            } else {
                alpha = 0;
            }
            Lines.ForEach(l => l.Alpha = alpha);
        }

        // Sort Approaches from highest end time to lowest end time
        public int CompareTo(Approach other) => other.HitTime.CompareTo(HitTime);
    }
}
