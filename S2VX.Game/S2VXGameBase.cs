using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using S2VX.Resources;

namespace S2VX.Game {
    public class S2VXGameBase : osu.Framework.Game {
        // Anything in this class is shared between the test browser and the game implementation.
        // It allows for caching global dependencies that should be accessible to tests, or changing
        // the screen scaling for all components including the test browser and framework overlays.

        protected override Container<Drawable> Content { get; }

        private DllResourceStore ResourceStore { get; set; }

        protected S2VXGameBase() => base.Content.Add(Content = new SquareContainer());

        [BackgroundDependencyLoader]
        private void Load() => Resources.AddStore(ResourceStore = new DllResourceStore(S2VXResources.ResourceAssembly));

        protected override void Dispose(bool isDisposing) {
            ResourceStore.Dispose();
            base.Dispose(isDisposing);
        }
    }
}
