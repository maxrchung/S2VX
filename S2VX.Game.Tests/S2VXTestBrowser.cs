using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace S2VX.Game.Tests {
    /// <summary>
    /// Tests ran  through the osu!framework visual test runner use this class.
    /// Tests ran through Test Explorer or the GitHub pipeline do not use this
    /// class.
    /// </summary>
    public class S2VXTestBrowser : S2VXGameBase {
        /// <summary>
        /// S2VXStory resolves an S2VXCursor as one of its dependencies. To
        /// avoid having to manually cache a Cursor into many visual tests, we
        /// can cache it here.
        /// </summary>
        [Cached]
        private S2VXCursor Cursor { get; set; } = new();

        protected override void LoadComplete() {
            base.LoadComplete();

            AddRange(new Drawable[]
            {
                new TestBrowser("S2VX"),
                Cursor
            });
        }

        public override void SetHost(GameHost host) {
            base.SetHost(host);
            host.Window.CursorState |= CursorState.Hidden;
        }
    }
}
