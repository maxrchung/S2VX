using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using System.Globalization;

namespace S2VX.Game.Play.UserInterface {
    public class HitErrorDisplay : CompositeDrawable {
        public bool IsInitiallySelected { get; set; }
        public RelativeBox IndicatorBox { get; private set; }
        private RelativeBox HitErrorBox { get; set; }
        private TextFlowContainer TxtHitError { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            var boxSize = S2VXGameBase.GameWidth / 20;
            Width = boxSize;
            Height = boxSize;
            Margin = new MarginPadding {
                Vertical = boxSize / 10,
                Horizontal = boxSize / 10
            };
            InternalChildren = new Drawable[] {
                IndicatorBox = new RelativeBox {
                    Width = 1.2f,
                    Height = 1.2f,
                    Alpha = IsInitiallySelected ? 1 : 0
                },
                HitErrorBox = new RelativeBox {
                    Colour = Color4.Red.Opacity(0.5f)
                },
                TxtHitError = new TextFlowContainer(s => s.Font = new FontUsage("default", S2VXGameBase.GameWidth / 40)) {
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
