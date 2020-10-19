using osu.Framework.Input.Events;

namespace S2VX.Game.SongSelection.UserInterface {
    public class BorderInnerBox : RelativeBox {

        // Capture OnHover to trigger the OuterBox's OnHoverLost
        protected override bool OnHover(HoverEvent e) => true;

        // Capture OnClick so as to not trigger the OuterBox's
        protected override bool OnClick(ClickEvent e) => true;
    }
}
