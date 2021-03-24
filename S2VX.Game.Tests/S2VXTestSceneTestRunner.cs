using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using osu.Framework.Testing;
using S2VX.Game.Configuration;
using S2VX.Resources;

namespace S2VX.Game.Tests {
    /// <summary>
    /// We need to create this for headless tests. Headless tests do not run
    /// with a GameHost, so we need to replicate that behavior here.
    /// </summary>
    public class S2VXTestSceneTestRunner : TestSceneTestRunner {
        /// <summary>
        /// Caching a global Cursor here lets us avoid having to create and load
        /// a Cursor into all of our separate tests. If we do need to hook onto
        /// a specific Cursor, we can recache the Cursor in the test. See
        /// S2VXCursorTests for more details.
        /// </summary>
        [Cached]
        private S2VXCursor Cursor { get; set; } = new S2VXCursor();

        public S2VXTestSceneTestRunner() : base() { }

        private new DependencyContainer Dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            Dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        [BackgroundDependencyLoader]
        private void Load() {
            // This allows headless tests to access the S2VXResources assembly to obtain
            // correct references for things like hit sounds.
            Resources.AddStore(new DllResourceStore(S2VXResources.ResourceAssembly));
            Add(Cursor);
            Dependencies.CacheAs(new S2VXConfigManager(Host.Storage));
        }
    }
}
