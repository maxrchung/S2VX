using osu.Framework.Allocation;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace S2VX.Game.Tests.VisualTests {
    public class S2VXGameTests : TestScene {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        private S2VXGame Game;

        [BackgroundDependencyLoader]
        private void Load(GameHost host) {
            Game = new S2VXGame();
            Game.SetHost(host);

            Add(Game);
        }
    }
}
