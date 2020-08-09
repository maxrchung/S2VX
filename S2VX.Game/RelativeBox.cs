using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace S2VX.Game {
    public class RelativeBox : Box {
        public RelativeBox() {
            RelativePositionAxes = Axes.Both;
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }
    }
}
