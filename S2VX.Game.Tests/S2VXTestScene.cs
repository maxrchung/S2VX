using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Testing;

namespace S2VX.Game.Tests {
    public class S2VXTestScene : TestScene {
        protected override Container<Drawable> Content { get; }

        public S2VXTestScene() => base.Content.Add(Content = new SquareContainer());
    }
}
