using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osuTK.Graphics;
using S2VX.Game.Play.Score;
using System.Diagnostics.CodeAnalysis;

// Using "EndGame" because "End" is a reserved word in C#
namespace S2VX.Game.EndGame {
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members")]
    public class EndGameScreen : Screen {
        private ScoreStatistics ScoreStatistics { get; }
        private string LeaderboardPath { get; }

        public EndGameScreen(ScoreStatistics scoreStatistics, string leaderboardPath) {
            ScoreStatistics = scoreStatistics;
            LeaderboardPath = leaderboardPath;
        }

        [BackgroundDependencyLoader]
        private void Load() =>
            InternalChildren = new Drawable[] {
                new Box {
                    Colour = Color4.Red,
                    Width = 300,
                    Height = 300
                }
            };
    }
}
