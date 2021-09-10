using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace S2VX.Game {
    public class S2VXVisibilityContainer : VisibilityContainer {
        [BackgroundDependencyLoader]
        private void Load() => RelativeSizeAxes = Axes.Both;

        protected override void PopIn() => Alpha = 1;

        protected override void PopOut() => Alpha = 0;
    }
}
