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
        protected override Container<Drawable> Content { get; }

        public S2VXTestScene() => base.Content.Add(Content = new SquareContainer());

        protected override ITestSceneTestRunner CreateRunner() => new S2VXTestSceneTestRunner();
    }
}
