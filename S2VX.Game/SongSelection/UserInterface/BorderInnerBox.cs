using osu.Framework.Allocation;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace S2VX.Game.SongSelection.UserInterface {
    public class BorderInnerBox : Box {
        [BackgroundDependencyLoader]
        private void Load() {
            Colour = Color4.Black;
            Position = new(50);
            Size = new(900);
        }

        // Capture OnHover to trigger the OuterBox's OnHoverLost
        protected override bool OnHover(HoverEvent e) => true;

        // Capture OnClick so as to not trigger the OuterBox's
        protected override bool OnClick(ClickEvent e) => true;
    }
}
