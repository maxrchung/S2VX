using osu.Framework.Input.Events;
using osuTK.Graphics;
using S2VX.Game.Play;

namespace S2VX.Game.SongSelection.UserInterface {
    public class BorderOuterBox : RelativeBox {

        public SongSelectionScreen SongSelectionScreen { get; set; }

        protected override bool OnHover(HoverEvent e) {
            Colour = Color4.LightGray;
            return false;
        }

        protected override void OnHoverLost(HoverLostEvent e) => Colour = Color4.White;

        protected override bool OnClick(ClickEvent e) {
            SongSelectionScreen.DoExit();
            return true;
        }

    }
}
