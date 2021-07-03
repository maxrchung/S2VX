using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Input;
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
        public ScoreGrade ScoreGrade { get; private set; }
        public LeaderboardContainer LeaderboardContainer { get; private set; }

        public EndGameScreen(ScoreStatistics scoreStatistics, string storyDirectory) {
            ScoreStatistics = scoreStatistics;
            StoryDirectory = storyDirectory;
        }

        protected override bool OnKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.Escape:
                    this.GetParentScreen().GetParentScreen().MakeCurrent();
                    return true;
                default:
                    break;
            }
            return false;
        }

        [BackgroundDependencyLoader]
        private void Load() =>
            InternalChildren = new Drawable[] {
                // EndGameScreen is pushed on top of the PlayScreen, so to get
                // back to the song preview screen we need to go up the parent
                // chain twice
                Border = new Border(StoryDirectory, () => this.GetParentScreen().GetParentScreen().MakeCurrent()),
                ScoreStatisticsDisplay = new ScoreStatisticsDisplay(ScoreStatistics),
                ScoreGrade = new ScoreGrade(ScoreStatistics.Accuracy, ScoreStatistics.IsFullCombo),
                LeaderboardContainer = new LeaderboardContainer(StoryDirectory, scoreStatistics: ScoreStatistics) {
                    Width = 450,
                    Height = 750,
                    X = 500,
                    Y = 150
                },
            };
    }
}
