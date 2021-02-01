using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace S2VX.Game.Editor.Containers {
    public class EditorUI : VisibilityContainer {
        protected override void PopIn() => this.FadeIn(100);

        protected override void PopOut() => this.FadeOut(100);
    }
}
