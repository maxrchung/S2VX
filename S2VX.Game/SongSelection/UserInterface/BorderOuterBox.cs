using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Graphics;
using S2VX.Game.Play;

namespace S2VX.Game.SongSelection.UserInterface {
    public class BorderOuterBox : RelativeBox {
        [Resolved]
        private ScreenStack Screens { get; set; }

        public SongSelectionScreen SongSelectionScreen { get; set; }

        // Is called by InnerBox when mouse hovers over InnerBox (and thus should not be considered over OuterBox)
        public void LoseHover() => Colour = Color4.White;

        protected override bool OnHover(HoverEvent e) {
            Colour = Color4.LightGray;
            return false;
        }

        protected override void OnHoverLost(HoverLostEvent e) => LoseHover();

        protected override bool OnClick(ClickEvent e) {
            SongSelectionScreen.DoExit();
            return true;
        }

    }
}
