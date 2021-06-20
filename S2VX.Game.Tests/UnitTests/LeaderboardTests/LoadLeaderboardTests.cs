using NUnit.Framework;
using S2VX.Game.Leaderboard;
using System.IO;

namespace S2VX.Game.Tests.UnitTests.LeaderboardTests {
    [TestFixture]
    public class LoadLeaderboardTests {

        private readonly string StoryPath = Path.Combine("UnitTests", "LeaderboardTests", "story.s2ry");

        [Test]
        public void NoStoryboardFile_HasZeroEntries() {
            var input = "doesNotExist.json";
            var leaderboard = new LeaderboardContainer(StoryPath, leaderboardFileName: input);
            Assert.AreEqual(0, leaderboard.EntryCount);
        }

        [Test]
        public void InvalidStoryboardFile_HasMinusOneEntries() {
            var input = "invalid.json";
            var leaderboard = new LeaderboardContainer(StoryPath, leaderboardFileName: input);
            Assert.AreEqual(-1, leaderboard.EntryCount);
        }

        [Test]
        public void ValidStoryboardFile_HasThreeEntries() {
            var input = "valid.json";
            var leaderboard = new LeaderboardContainer(StoryPath, leaderboardFileName: input);
            Assert.AreEqual(3, leaderboard.EntryCount);
        }
    }
}
