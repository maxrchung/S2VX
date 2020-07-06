using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osuTK;

namespace S2VX.Game
{
    public class Camera : Drawable
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            Scale = new Vector2(0.1f);
        }
    }
}
