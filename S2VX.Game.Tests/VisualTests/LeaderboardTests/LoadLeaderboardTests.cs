using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using S2VX.Game.Leaderboard;
using System.IO;

namespace S2VX.Game.Tests.VisualTests.LeaderboardTests {
    public class LoadLeaderboardTests : S2VXTestScene {

        private readonly string StoryDirectory = Path.Combine("VisualTests", "LeaderboardTests");
        private LeaderboardContainer Leaderboard { get; set; }

        [SetUpSteps]
        public void SetUpSteps() => AddStep("Clear all", Clear);

        [Test]
        public void NoStoryboardFile_HasZeroEntries() {
            var input = "doesNotExist.json";
            AddStep($"Add leaderboard {input}", () => {
                Leaderboard = new LeaderboardContainer(StoryDirectory, input) {
                    Width = 540,
                    Height = 360,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                };
                Add(Leaderboard);
            });
            AddAssert($"No leaderboard file has 0 entries", () => Leaderboard.EntryCount == 0);
        }

        [Test]
        public void InvalidStoryboardFile_HasMinusOneEntries() {
            var input = "invalid.json";
            AddStep($"Add leaderboard {input}", () => {
                Leaderboard = new LeaderboardContainer(StoryDirectory, input) {
                    Width = 540,
                    Height = 360,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                };
                Add(Leaderboard);
            });
            AddAssert($"Invalid leaderboard has -1 entries", () => Leaderboard.EntryCount == -1);
        }

        [Test]
        public void ValidStoryboardFile_HasThreeEntries() {
            var input = "valid.json";
            AddStep($"Add leaderboard {input}", () => {
                Leaderboard = new LeaderboardContainer(StoryDirectory, input) {
                    Width = 540,
                    Height = 360,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                };
                Add(Leaderboard);
            });
            AddAssert($"Valid leaderboard with 3 entries", () => Leaderboard.EntryCount == 3);
        }
    }
}
