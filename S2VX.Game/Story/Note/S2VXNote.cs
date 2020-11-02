using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;

namespace S2VX.Game.Story.Note {
    public abstract class S2VXNote : CompositeDrawable, IComparable<S2VXNote> {
        public double EndTime { get; set; }
        public Vector2 Coordinates { get; set; } = Vector2.Zero;

        public Approach Approach { get; set; }

        public RelativeBox BoxOuter { get; } = new RelativeBox();
        public RelativeBox BoxInner { get; } = new RelativeBox();

        [Resolved]
        private S2VXStory Story { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            Alpha = 1;
            RelativePositionAxes = Axes.Both;
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            InternalChildren = new Drawable[] {
                BoxOuter,
                BoxInner
            };
        }

        // These Update setters modify both the Note and a corresponding Approach
        public void UpdateEndTime(double endTime) {
            Approach.EndTime = endTime;
            EndTime = endTime;
            Story.Notes.Sort();
        }

        public void UpdateCoordinates(Vector2 coordinates) {
            Approach.Coordinates = coordinates;
            Coordinates = coordinates;
        }

        // Sort Notes from highest end time to lowest end time
        public int CompareTo(S2VXNote other) => other.EndTime.CompareTo(EndTime);
    }
}
