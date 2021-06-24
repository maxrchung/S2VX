using osu.Framework.Allocation;
using osu.Framework.Screens;
using S2VX.Game.EndGame;
using S2VX.Game.Play.Score;

namespace S2VX.Game.Tests.VisualTests {
    public class EndGameScreenTests : S2VXTestScene {
        [Cached]
        private ScreenStack ScreenStack { get; } = new(new EndGameScreen(new(), ""));

        private ScoreStatistics ScoreStatistics { get; } = new();

        [BackgroundDependencyLoader]
        private void Load() => Add(ScreenStack);
    }
}
