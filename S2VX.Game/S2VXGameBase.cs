using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osuTK;
using S2VX.Game.Configuration;
using S2VX.Game.SongSelection;
using S2VX.Resources;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace S2VX.Game {
    public class S2VXGameBase : osu.Framework.Game {
        // Anything in this class is shared between the test browser and the game implementation.
        // It allows for caching global dependencies that should be accessible to tests, or changing
        // the screen scaling for all components including the test browser and framework overlays.

        // The "default" resolution from which things are scaled and positioned
        // THIS SHOULD NEVER BE CHANGED
        public const float GameWidth = 1000.0f;

        [Cached]
        protected S2VXCursor Cursor { get; } = new();

        private new DependencyContainer Dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            Dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        private DllResourceStore ResourceStore { get; set; }

        protected override Container<Drawable> Content { get; }

        private List<SongSelectionScreen> FileImporters { get; set; } = new();

        // Ensure game and tests scale with window size and screen DPI.
        public S2VXGameBase() => base.Content.Add(Content = new DrawSizePreservingFillContainer {
            // You may want to change TargetDrawSize to your "default"
            // resolution, which will decide how things scale and position when
            // using absolute coordinates.
            TargetDrawSize = new Vector2(GameWidth, GameWidth)
        });

        [BackgroundDependencyLoader]
        private void Load() {
            Resources.AddStore(ResourceStore = new(S2VXResources.ResourceAssembly));
            Dependencies.CacheAs(new S2VXConfigManager(Host.Storage));
            Dependencies.CacheAs(this);
        }

        protected override void Dispose(bool isDisposing) {
            ResourceStore.Dispose();
            base.Dispose(isDisposing);
        }

        public override void SetHost(GameHost host) {
            base.SetHost(host);
            switch (host.Window) {
                case SDL2DesktopWindow window:
                    window.CursorState |= CursorState.Hidden;
                    window.Title = "S2VX";
                    window.DragDrop += f => FileDrop(new[] { f });
                    break;
            }
        }

        private void FileDrop(string[] filePaths) {
            // Currently only supports dragging in one mp3 file
            var filePath = filePaths.First();
            var extension = Path.GetExtension(filePath)?.ToUpperInvariant();
            if (extension == ".MP3") {
                FileImporters.First()?.Import(filePath);
            }
        }

        /// <summary>
        /// Register a global handler for file imports. Most recently registered will have precedence.
        /// </summary>
        /// <param name="handler">The handler to register.</param>
        public void RegisterImportHandler(SongSelectionScreen handler) => FileImporters.Insert(0, handler);

        /// <summary>
        /// Unregister a global handler for file imports.
        /// </summary>
        /// <param name="handler">The previously registered handler.</param>
        public void UnregisterImportHandler(SongSelectionScreen handler) => FileImporters.Remove(handler);
    }
}
