using System.Collections.Generic;

namespace S2VX.Game.Leaderboard {
    public class LeaderboardEntries {
        public class LeaderboardEntry {
            public string Name { get; set; }
            public string Score { get; set; }

            public LeaderboardEntry(string name, string score) {
                Name = name;
                Score = score;
            }
        }

        public IEnumerable<LeaderboardEntry> Entries { get; set; }

        public LeaderboardEntries() { }

    }
}
