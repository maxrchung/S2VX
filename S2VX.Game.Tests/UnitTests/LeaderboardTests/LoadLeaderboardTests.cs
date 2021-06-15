using NUnit.Framework;
using S2VX.Game.Leaderboard;
using System.IO;

namespace S2VX.Game.Tests.UnitTests.LeaderboardTests {
    [TestFixture]
    public class LoadLeaderboardTests {

        private readonly string StoryPath = Path.Combine("UnitTests", "LeaderboardTests", "story.s2ry");

        [Test]
        public void NoStoryboardFile_Has0Entries() {
            var input = "doesNotExist.json";
            var leaderboard = new LeaderboardContainer(StoryPath, input);
            Assert.AreEqual(0, leaderboard.EntryCount);
        }

        //[Test]
        //public void InvalidStoryboardFile_HasErrorMessage() {
        //    var input = "invalid.json";
        //    var leaderboard = new LeaderboardContainer(StoryPath, input);
        //    Assert.AreEqual(0, leaderboard.EntryCount);
        //}

        //[Test]
        //public void ValidStoryboardFile_Has3Entries() {
        //    var input = "valid.json";
        //    var leaderboard = new LeaderboardContainer(StoryPath, input);
        //    Assert.AreEqual(3, leaderboard.EntryCount);
        //}
    }
}
