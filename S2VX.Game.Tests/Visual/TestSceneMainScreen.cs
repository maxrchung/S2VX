using osu.Framework.Testing;
using S2VX.Game.Story;

namespace S2VX.Game.Tests.Visual {
    public class TestSceneMainScreen : TestScene {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        public TestSceneMainScreen() => Add(new S2VXStory());
    }
}
