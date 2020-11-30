using osu.Framework.Allocation;
using osuTK;

namespace S2VX.Game.Story.Note {
    public abstract class HoldNote : S2VXNote {
        public double EndTime { get; set; }
        public Vector2 EndCoordinates { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }
    }
}
