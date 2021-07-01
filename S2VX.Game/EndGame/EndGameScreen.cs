using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using S2VX.Game.EndGame.UserInterface;
using S2VX.Game.Leaderboard;
using S2VX.Game.Play.Score;
using S2VX.Game.SongSelection.Containers;

// Using "EndGame" namespace because "End" is a reserved word in C#
namespace S2VX.Game.EndGame {
    public class EndGameScreen : Screen {
        private ScoreStatistics ScoreStatistics { get; }
        private string StoryDirectory { get; }
        public Border Border { get; private set; }
        public ScoreStatisticsDisplay ScoreStatisticsDisplay { get; private set; }
        public LeaderboardContainer LeaderboardContainer { get; private set; }

        public EndGameScreen(ScoreStatistics scoreStatistics, string storyDirectory) {
            ScoreStatistics = scoreStatistics;
            StoryDirectory = storyDirectory;
        }

        [BackgroundDependencyLoader]
        private void Load() =>
            InternalChildren = new Drawable[] {
                // EndGameScreen is pushed on top of the PlayScreen, so to get
                // back to the song preview screen we need to go up the parent
                // chain twice
                Border = new Border(StoryDirectory, () => this.GetParentScreen().GetParentScreen().MakeCurrent()),
                ScoreStatisticsDisplay = new ScoreStatisticsDisplay(ScoreStatistics) {
                    Y = 450,
                    Size = new(450)
                },
                LeaderboardContainer = new LeaderboardContainer(StoryDirectory, scoreStatistics: ScoreStatistics) {
                    Width = 450,
                    Height = 650,
                    X = 500,
                    Y = 250
                },
            };
    }
}
