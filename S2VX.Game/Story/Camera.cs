using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osuTK;

namespace S2VX.Game.Story {
    public class Camera : Drawable {
        [BackgroundDependencyLoader]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Load() => Scale = new Vector2(0.1f);
    }
}
