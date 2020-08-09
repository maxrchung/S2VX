using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osuTK;

namespace S2VX.Game.Story {
    public class Camera : Drawable {
        [BackgroundDependencyLoader]
        private void Load() => Scale = new Vector2(0.1f);
    }
}
