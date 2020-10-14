using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;

namespace S2VX.Game.SongSelection.UserInterface {
    public class SelectedItemDisplay : CompositeDrawable {
        [Resolved]
        private ScreenStack Screens { get; set; }

        public bool IsSelected { get; set; }
        public string Text { get; set; }
        public RelativeBox SelectedIndicatorBox { get; private set; }
        private RelativeBox ThumbnailBox { get; set; }
        private RelativeBox TextShadowBox { get; set; }
        private TextFlowContainer TextContainer { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            var boxSize = Screens.DrawWidth / 4;
            Width = boxSize;
            Height = boxSize;
            Margin = new MarginPadding {
                Vertical = boxSize / 10,
                Horizontal = boxSize / 10
            };
            InternalChildren = new Drawable[] {
                SelectedIndicatorBox = new RelativeBox {
                    Width = 1.2f,
                    Height = 1.2f,
                    Alpha = IsSelected ? 1 : 0
                },
                ThumbnailBox = new RelativeBox {
                    Colour = Color4.White.Opacity(0.9f),
                },
                TextShadowBox = new RelativeBox {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Height = 0.2f,
                    Colour = Color4.Black.Opacity(0.5f),
                },
                TextContainer = new TextFlowContainer(s => s.Font = new FontUsage("default", Screens.DrawWidth / 40)) {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Y = 0.8f,
                    Height = 0.2f,
                    TextAnchor = Anchor.TopCentre,
                    Text = Text,
                },
            };
        }

    }
}
