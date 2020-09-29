using osu.Framework.Allocation;
using S2VX.Game.Play.UserInterface;

namespace S2VX.Game.Play {
    public class S2VXScore {
        public int Value { get; set; }

        [Resolved]
        public ScoreDisplay ScoreDisplay { get; set; }

        public void Add(int value) {
            Value += value;
            ScoreDisplay.UpdateScore(value);
        }
    }
}
