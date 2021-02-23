using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using osu.Framework.Testing;
using S2VX.Resources;

namespace S2VX.Game.Tests {
    public class S2VXTestSceneTestRunner : TestSceneTestRunner {
        public S2VXTestSceneTestRunner() : base() { }

        [BackgroundDependencyLoader]
        private void Load() =>
            Resources.AddStore(new DllResourceStore(S2VXResources.ResourceAssembly));
    }
}
