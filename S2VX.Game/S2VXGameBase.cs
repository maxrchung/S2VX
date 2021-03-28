using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osuTK;
using S2VX.Resources;

namespace S2VX.Game {
    public class S2VXGameBase : osu.Framework.Game {
        // Anything in this class is shared between the test browser and the game implementation.
        // It allows for caching global dependencies that should be accessible to tests, or changing
        // the screen scaling for all components including the test browser and framework overlays.

        // The "default" resolution from which things are scaled and positioned
        public const float GameWidth = 1000.0f;

        [Cached]
        protected S2VXCursor Cursor { get; } = new();

        private DllResourceStore ResourceStore { get; set; }

        protected override Container<Drawable> Content { get; }

        public override void SetHost(GameHost host) {
            base.SetHost(host);
            host.Window.CursorState |= CursorState.Hidden;
        }

        // Ensure game and tests scale with window size and screen DPI.
        protected S2VXGameBase() => base.Content.Add(Content = new DrawSizePreservingFillContainer {
            // You may want to change TargetDrawSize to your "default"
            // resolution, which will decide how things scale and position when
            // using absolute coordinates.
            TargetDrawSize = new Vector2(GameWidth, GameWidth)
        });

        [BackgroundDependencyLoader]
        private void Load() => Resources.AddStore(ResourceStore = new DllResourceStore(S2VXResources.ResourceAssembly));

        protected override void Dispose(bool isDisposing) {
            ResourceStore.Dispose();
            base.Dispose(isDisposing);
        }
    }
}
