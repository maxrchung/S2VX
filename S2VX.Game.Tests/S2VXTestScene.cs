using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Testing;

namespace S2VX.Game.Tests {
    /// <summary>
    /// <see cref="S2VXGame"/> constrains its content to a square container. To
    /// replicate the same effect in Visual Tests, we need to also apply this
    /// logic within our own TestScene class.
    /// </summary>
    public class S2VXTestScene : ManualInputManagerTestScene {
        [Cached]
        private GlobalVolumeDisplay VolumeDisplay { get; set; } = new();

        protected override Container<Drawable> Content { get; } = new SquareContainer();

        protected override ITestSceneTestRunner CreateRunner() => new S2VXTestSceneTestRunner();
    }
}
