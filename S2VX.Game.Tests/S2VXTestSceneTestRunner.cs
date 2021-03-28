using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using osu.Framework.Testing;
using S2VX.Game.Configuration;
using S2VX.Resources;

namespace S2VX.Game.Tests {
    /// <summary>
    /// Tests ran through Test Explorer or the GitHub pipeline use this class.
    /// Tests ran through the osu!framework visual test runner do not use this
    /// class.
    /// </summary>
    public class S2VXTestSceneTestRunner : TestSceneTestRunner {
        /// <summary>
        /// S2VXStory resolves an S2VXCursor as one of its dependencies. To
        /// avoid having to manually cache a Cursor into many visual tests, we
        /// can cache it here.
        /// </summary>
        [Cached]
        private S2VXCursor Cursor { get; set; } = new();

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
