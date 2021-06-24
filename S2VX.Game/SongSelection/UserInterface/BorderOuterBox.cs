using osu.Framework.Allocation;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Graphics;

namespace S2VX.Game.SongSelection.UserInterface {
    public class BorderOuterBox : Box {
        [Resolved]
        private ScreenStack Screens { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            Colour = Color4.White;
            Size = new(1000);
        }

        protected override bool OnHover(HoverEvent e) {
            Colour = Color4.LightGray;
            return false;
        }

        protected override void OnHoverLost(HoverLostEvent e) => Colour = Color4.White;

        protected override bool OnClick(ClickEvent e) {
            Screens.CurrentScreen.Exit();
            return true;
        }

    }
}
