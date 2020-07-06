using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace S2VX.Game
{
    public class S2VXGame : S2VXGameBase
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            // Add your top-level game components here.
            // A screen stack and sample screen has been provided for convenience, but you can replace it if you don't want to use screens.
            Child = new ScreenStack(new MainScreen()) { RelativeSizeAxes = Axes.Both };
        }
    }
}
