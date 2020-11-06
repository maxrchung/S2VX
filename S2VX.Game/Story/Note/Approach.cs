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

        public List<RelativeBox> Lines { get; } = new List<RelativeBox>()
        {
            new RelativeBox(), // up
            new RelativeBox(), // down
            new RelativeBox(), // right
            new RelativeBox()  // left
        };

        [BackgroundDependencyLoader]
        private void Load() {
            Lines.ForEach(l => l.Alpha = 0);
            AlwaysPresent = true;
            RelativeSizeAxes = Axes.Both;
            InternalChildren = Lines;
        }

        // Sort Approaches from highest end time to lowest end time
        public int CompareTo(Approach other) => other.HitTime.CompareTo(HitTime);
    }
}
