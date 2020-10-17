using osu.Framework.Input.Events;

namespace S2VX.Game.SongSelection.UserInterface {
    public class BorderInnerBox : RelativeBox {
        public BorderOuterBox BorderOuterBox { get; set; }

        protected override bool OnHover(HoverEvent e) {
            BorderOuterBox.LoseHover();
            return true;
        }

        // Capture OnClick so as to not trigger the OuterBox's
        protected override bool OnClick(ClickEvent e) => true;
    }
}
