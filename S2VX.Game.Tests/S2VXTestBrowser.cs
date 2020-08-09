using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace S2VX.Game.Tests {
    public class S2VXTestBrowser : S2VXGameBase {
        protected override void LoadComplete() {
            base.LoadComplete();

            AddRange(new Drawable[]
            {
                new TestBrowser("S2VX"),
                new CursorContainer()
            });
        }

        public override void SetHost(GameHost host) {
            base.SetHost(host);
            host.Window.CursorState |= CursorState.Hidden;
        }
    }
}
