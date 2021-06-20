using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osuTK.Graphics;
using S2VX.Game.EndGame.UserInterface;
using S2VX.Game.Play.Score;
using System.Diagnostics.CodeAnalysis;

// Using "EndGame" because "End" is a reserved word in C#
namespace S2VX.Game.EndGame {
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members")]
    public class EndGameScreen : Screen {
        private ScoreStatisticsDisplay ScoreStatisticsDisplay { get; }
        private string LeaderboardPath { get; }

        public EndGameScreen(ScoreStatistics scoreStatistics, string leaderboardPath) {
            ScoreStatisticsDisplay = new ScoreStatisticsDisplay(scoreStatistics) {
                Width = 500,
                Height = 500
            };
            LeaderboardPath = leaderboardPath;
        }

        [BackgroundDependencyLoader]
        private void Load() =>
            InternalChildren = new Drawable[] {
                new Box {
                    Colour = Color4.DarkBlue,
                    Width = 500,
                    Height = 500
                },
                ScoreStatisticsDisplay,
                //new TableContainer {
                //    Colour = Color4.White,
                //    Width = 500,
                //    Height = 500,
                //    Content = new Drawable[,] {
                //        {
                //            new Box { Anchor = Anchor.CentreRight, Origin = Anchor.CentreRight, Colour = Color4.Red, Width = 50, Height = 50 },
                //            new Box { Anchor = Anchor.CentreLeft, Origin = Anchor.CentreLeft,  Colour = Color4.Blue, Width = 100, Height = 100 }
                //        },
                //        {
                //            new Box { Anchor = Anchor.CentreRight, Origin = Anchor.CentreRight, Colour = Color4.Blue, Width = 100, Height = 100 },
                //            new Box { Anchor = Anchor.CentreLeft, Origin = Anchor.CentreLeft, Colour = Color4.Red, Width = 50, Height = 50 }
                //        },
                //        {
                //            new Box { Anchor = Anchor.CentreRight, Origin = Anchor.CentreRight, Colour = Color4.Blue, Width = 100, Height = 100 },
                //            new Box { Anchor = Anchor.CentreLeft, Origin = Anchor.CentreLeft, Colour = Color4.Red, Width = 50, Height = 50 }
                //        },
                //    }
                //}
            };
    }
}
