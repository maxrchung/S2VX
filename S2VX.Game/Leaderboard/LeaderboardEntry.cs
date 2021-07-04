using System;
using System.Globalization;

namespace S2VX.Game.Leaderboard {
    public class LeaderboardEntry : IComparable<LeaderboardEntry> {
        public string Name { get; set; }
        public string Score { get; set; }

        public LeaderboardEntry(string name, string score) {
            Name = name;
            Score = score;
        }

        // Compare two entries by ascending score first, then by name
        public int CompareTo(LeaderboardEntry other) {
            var scoreCompare = int.Parse(Score, CultureInfo.InvariantCulture).CompareTo(int.Parse(other.Score, CultureInfo.InvariantCulture));
            return scoreCompare == 0 ? string.Compare(Name, other.Name, StringComparison.Ordinal) : scoreCompare;
        }
    }
}
