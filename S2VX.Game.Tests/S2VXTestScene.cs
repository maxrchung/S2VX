using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Testing;
using osuTK;

namespace S2VX.Game.Tests {
    /// <summary>
    /// <see cref="S2VXGame"/> constrains its content to a square container. To
    /// replicate the same effect in Visual Tests, we need to also apply this
    /// logic within our own TestScene class.
    /// </summary>
    public class S2VXTestScene : ManualInputManagerTestScene {
        [Cached]
        private S2VXGameBase GameBase { get; } = new();

        [Cached]
        private GlobalVolumeDisplay VolumeDisplay { get; set; } = new();

        protected override Container<Drawable> Content { get; } = new DrawSizePreservingSquareContainer();

        protected override ITestSceneTestRunner CreateRunner() => new S2VXTestSceneTestRunner();

        /// <summary>
        /// The osu!framework provided MoveMouseTo transforms the
        /// drawable's position to screen space but does not transform the
        /// offset. It assumes that the offset is in screen space. Practically
        /// however, whenever we move the mouse and apply an offset, that offset
        /// is usually in local coordinates. This helper function applies that
        /// logic as we expect.
        /// </summary>
        protected void MoveMouseTo(Drawable drawable, Vector2? offset = null) =>
            InputManager.MoveMouseTo(drawable.ToScreenSpace(drawable.LayoutRectangle.Centre + (offset ?? Vector2.Zero)));
    }
}
