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
            Child = new Story();
        }
    }
}
