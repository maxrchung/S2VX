using osu.Framework.Lists;

namespace S2VX.Game.Play.Score {
    // Separating this from ScoreProcessor to simplify EndGameScreen's dependency to score
    public class ScoreStatistics {
        // Score should be a double type because during a drag, score may add
        // very small values that are between 0 and 1. If we int cast or round
        // this drag value, we'll always get 0.
        public double Score { get; set; }
        public int PerfectCount { get; set; }
        public int EarlyCount { get; set; }
        public int LateCount { get; set; }
        public int MissCount { get; set; }
        public SortedList<double> Scores { get; } = new();
        public int Combo { get; set; }
        public int MaxCombo { get; set; }
        public double Accuracy => Scores.Count == 0 ? 0 : (double)PerfectCount / Scores.Count;

        public double Median() {
            if (Scores.Count == 0) {
                return 0;
            }

            if (Scores.Count % 2 == 1) {
                return Scores[Scores.Count / 2];
            } else {
                var left = Scores[Scores.Count / 2 - 1];
                var right = Scores[Scores.Count / 2];
                return (left + right) / 2;
            }
        }
    }
}
