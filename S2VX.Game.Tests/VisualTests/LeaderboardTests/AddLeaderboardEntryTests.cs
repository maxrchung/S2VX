using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using S2VX.Game.Leaderboard;
using S2VX.Game.Play.Score;
using System.IO;
using System.Linq;

namespace S2VX.Game.Tests.VisualTests.LeaderboardTests {
    public class AddLeaderboardEntryTests : S2VXTestScene {

        private readonly string StoryDirectory = Path.Combine("VisualTests", "LeaderboardTests");
        private LeaderboardContainer Leaderboard { get; set; }

        [SetUpSteps]
        public void SetUpSteps() => AddStep("Clear all", Clear);

        [Test]
        public void AddEntry_IncrementsEntryCountByOne() {
            var input = "increment.json";
            ScoreStatistics scoreStatistics = new();
            var oldEntryCount = -1;
            AddStep($"Add leaderboard {input}", () => {
                Leaderboard = new LeaderboardContainer(StoryDirectory, input, scoreStatistics) {
                    Width = 540,
                    Height = 360,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                };
                Add(Leaderboard);
            });
            AddStep($"Save old EntryCount", () => oldEntryCount = Leaderboard.EntryCount);
            AddStep($"Add entry", () => Leaderboard.AddEntry("test", 123));
            AddAssert($"New EntryCount is one more", () => Leaderboard.EntryCount == oldEntryCount + 1);
        }

        [Test]
        public void AddEntry_FloatingPointScore_IsRoundedDown() {
            var input = "empty.json";
            AddStep($"Add leaderboard {input}", () => Add(Leaderboard = new LeaderboardContainer(StoryDirectory, input, new())));
            AddStep("Add entry", () => Leaderboard.AddEntry("test", 100.001));
            AddAssert($"Is rounded down", () => Leaderboard.LeaderboardData.First().Score == "100");
        }

        [Test]
        public void AddEntry_FloatingPointScore_IsRoundedUp() {
            var input = "empty.json";
            AddStep($"Add leaderboard {input}", () => Add(Leaderboard = new LeaderboardContainer(StoryDirectory, input, new())));
            AddStep("Add entry", () => Leaderboard.AddEntry("test", 99.999));
            AddAssert($"Is rounded up", () => Leaderboard.LeaderboardData.First().Score == "100");
        }
    }
}
