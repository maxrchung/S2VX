using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
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

        protected Vector2 HitApproachTopLeftCorner { get; private set; }
        protected Vector2 HitApproachTopRightCorner { get; private set; }
        protected Vector2 HitApproachBottomLeftCorner { get; private set; }
        protected Vector2 HitApproachBottomRightCorner { get; private set; }

        [BackgroundDependencyLoader]
        private void Load() {
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
        /// Updates an approach's color/alpha
        /// </summary>
        protected virtual void UpdateColor() {
            var time = Time.Current;
            var notes = Story.Notes;
            // Fade in time to Show time
            if (time < HitTime - notes.ShowTime) {
                var startTime = HitTime - notes.ShowTime - notes.FadeInTime;
                var endTime = HitTime - notes.ShowTime;
                Alpha = S2VXUtils.ClampedInterpolation(time, 0.0f, 1.0f, startTime, endTime);
            }
            // Show time to Hit time
            else if (time < HitTime) {
                Alpha = 1;
            }
            // Hit time to Fade out time
            else if (time < HitTime + notes.FadeOutTime) {
                var startTime = HitTime;
                var endTime = HitTime + notes.FadeOutTime;
                Alpha = S2VXUtils.ClampedInterpolation(time, 1.0f, 0.0f, startTime, endTime);
            } else {
                Alpha = 0;
            }
        }

        /// <summary>
        /// Updates an approach's position/rotation/size
        /// </summary>
        protected virtual void UpdatePosition() => UpdateInnerApproachPosition(Coordinates);

        /// <summary>
        /// Both Approach and HoldApproach use this helper to set the inner approach's position
        /// </summary>
        /// <param name="coordinates">S2VX coordinates that the approach is closing onto</param>
        protected void UpdateInnerApproachPosition(Vector2 coordinates) {
            var notes = Story.Notes;
            var camera = Story.Camera;
            var approaches = Story.Approaches;
            var position = camera.Position;
            var rotation = camera.Rotation;
            var scale = camera.Scale;
            var thickness = approaches.Thickness;
            var time = Time.Current;

            var startTime = HitTime - notes.ShowTime - notes.FadeInTime;
            var distance = S2VXUtils.ClampedInterpolation(time, approaches.Distance, scale.X / 2, startTime, HitTime);
            var rotationX = S2VXUtils.Rotate(new Vector2(distance, 0), rotation);
            var rotationY = S2VXUtils.Rotate(new Vector2(0, distance), rotation);
            // Add extra thickness so corners overlap
            var overlap = distance * 2 + thickness;

            var offset = S2VXUtils.Rotate(coordinates - position, rotation) * scale;
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

        // Sort Approaches from highest end time to lowest end time
        public int CompareTo(Approach other) => other.HitTime.CompareTo(HitTime);
    }
}
