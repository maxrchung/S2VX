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

        public int CompareTo(LeaderboardEntry other) =>
            int.Parse(Score, CultureInfo.InvariantCulture).CompareTo(int.Parse(other.Score, CultureInfo.InvariantCulture));
    }
}
