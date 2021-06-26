using osu.Framework.Allocation;
using osu.Framework.Screens;
using S2VX.Game.EndGame;

namespace S2VX.Game.Tests.VisualTests {
    public class EndGameScreenTests : S2VXTestScene {
        [Cached]
        private ScreenStack ScreenStack { get; } = new(new EndGameScreen(new(), ""));

        [BackgroundDependencyLoader]
        private void Load() => Add(ScreenStack);
    }
}
