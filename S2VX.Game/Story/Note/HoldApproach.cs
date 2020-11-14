using osu.Framework.Allocation;
using osu.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace S2VX.Game.Story.Note {
    public class HoldApproach : Approach {
        public double EndTime { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; } = null;

        public List<RelativeBox> ReleaseLines { get; } = new List<RelativeBox>()
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
    }
}
