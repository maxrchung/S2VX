using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace S2VX.Game.Editor.Containers {
    /// <summary>
    /// Reduces some redundancy and ensures that our overlay containers (e.g.
    /// command panel, metadata panel, tap panel) stay consistent
    /// </summary>
    public class S2VXOverlayContainer : OverlayContainer {
        protected override void PopIn() => this.FadeIn(100);

        protected override void PopOut() => this.FadeOut(100);
    }
}
