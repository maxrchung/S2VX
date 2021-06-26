using osu.Framework.Allocation;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK.Graphics;
using System;

namespace S2VX.Game.SongSelection.UserInterface {
    public class BorderOuterBox : Box {
        private Action OnExit { get; set; }

        public BorderOuterBox(Action onExit) => OnExit = onExit;

        [BackgroundDependencyLoader]
        private void Load(Action onExit) {
            Colour = Color4.White;
            Size = new(1000);
        }

        protected override bool OnHover(HoverEvent e) {
            Colour = Color4.LightGray;
            return false;
        }

        protected override void OnHoverLost(HoverLostEvent e) => Colour = Color4.White;

        protected override bool OnClick(ClickEvent e) {
            OnExit();
            return true;
        }
    }
}
