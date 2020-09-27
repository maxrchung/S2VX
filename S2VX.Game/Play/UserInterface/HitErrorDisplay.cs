using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;
using System.Globalization;

namespace S2VX.Game.Play.UserInterface {
    public class HitErrorDisplay : CompositeDrawable {
        [Resolved]
        private ScreenStack Screens { get; set; }

        public int HitError { get; set; }
        private RelativeBox HitErrorBox { get; set; }
        private TextFlowContainer TxtHitError { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            var boxSize = Screens.DrawWidth / 20;
            Width = boxSize;
            Height = boxSize;
            Margin = new MarginPadding {
                Vertical = boxSize / 10,
                Horizontal = boxSize / 10
            };
            InternalChildren = new Drawable[]
            {
                HitErrorBox = new RelativeBox { },
                TxtHitError = new TextFlowContainer(s => s.Font = new FontUsage("default", Screens.DrawWidth / 40)) {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    TextAnchor = Anchor.Centre,
                },
            };
        }

        public void UpdateHitError(int hitError) {
            TxtHitError.Text = hitError.ToString(CultureInfo.InvariantCulture);
            HitErrorBox.Colour = hitError > 0 ? Color4.Red.Opacity(0.5f) : Color4.Blue.Opacity(0.5f);
        }

    }
}
