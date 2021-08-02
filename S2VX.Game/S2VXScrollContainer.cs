using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;

namespace S2VX.Game {
    public class S2VXScrollContainer : BasicScrollContainer {
        public S2VXScrollContainer() { }

        protected override bool OnScroll(ScrollEvent e) {
            base.OnScroll(e);
            return false;
        }
    }
}
