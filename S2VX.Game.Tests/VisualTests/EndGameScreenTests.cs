using osu.Framework.Allocation;
using osu.Framework.Screens;
using S2VX.Game.EndGame;
using S2VX.Game.Play.Score;

namespace S2VX.Game.Tests.VisualTests {
    public class EndGameScreenTests : S2VXTestScene {
        private ScoreStatistics ScoreStatistics { get; } = new();

        [BackgroundDependencyLoader]
        private void Load() => Add(new ScreenStack(new EndGameScreen(ScoreStatistics, "")));
    }
}
