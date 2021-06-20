namespace S2VX.Game.Leaderboard {
    public class LeaderboardEntry {
        public string Name { get; set; }
        public string Score { get; set; }

        public LeaderboardEntry(string name, string score) {
            Name = name;
            Score = score;
        }
    }
}
