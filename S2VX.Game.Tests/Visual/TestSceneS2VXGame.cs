using osu.Framework.Allocation;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace S2VX.Game.Tests.Visual
{
    public class TestSceneS2VXGame : TestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        private S2VXGame game;

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            game = new S2VXGame();
            game.SetHost(host);

            Add(game);
        }
    }
}
