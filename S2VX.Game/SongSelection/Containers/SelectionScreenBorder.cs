using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;
using S2VX.Game.Play;
using S2VX.Game.SongSelection.UserInterface;

namespace S2VX.Game.SongSelection.Containers {
    public class SelectionScreenBorder : CompositeDrawable {
        [Resolved]
        private ScreenStack Screens { get; set; }

        public SongSelectionScreen SongSelectionScreen { get; set; }    // Set in SongSelectionScreen
        public string CurSelectionPath { get; set; }                    // Set in SongSelectionScreen
        public float InnerBoxRelativeSize { get; set; } = 0.9f;
        private BorderOuterBox BorderOuterBox { get; set; }
        private BorderInnerBox BorderInnerBox { get; set; }
        private TextFlowContainer TextContainer { get; set; }


        //protected override bool OnHover(HoverEvent e) {
        //    Border.Colour = Color4.Blue;
        //    return true;
        //}

        //protected override void OnHoverLost(HoverLostEvent e) {
        //    Border.Colour = Color4.Blue;
        //}

        //protected override bool OnClick(ClickEvent e) {
        //    return true;
        //}

        [BackgroundDependencyLoader]
        private void Load() {
            var fullWidth = Screens.DrawWidth;
            var fullHeight = Screens.DrawHeight;
            var borderSize = fullHeight * (1 - InnerBoxRelativeSize) / 2;
            var titleSize = borderSize * 0.5f;
            var spacingMargin = 0.02f;

            Width = fullWidth;
            Height = fullHeight;

            InternalChildren = new Drawable[] {
                BorderOuterBox = new BorderOuterBox {
                    SongSelectionScreen = SongSelectionScreen
                },
                TextContainer = new TextFlowContainer(s => s.Font = new FontUsage("default", titleSize)) {
                    Width = fullWidth,
                    Height = borderSize,
                    Margin = new MarginPadding {
                        Horizontal = fullWidth * spacingMargin,
                    },
                    Text = CurSelectionPath,
                    TextAnchor = Anchor.CentreLeft,
                    Colour = Color4.Black,
                    // Todo, truncate text if it's too long
                },
                BorderInnerBox = new BorderInnerBox {
                    BorderOuterBox = BorderOuterBox,
                    Colour = Color4.Black,
                    Width = InnerBoxRelativeSize,
                    Height = InnerBoxRelativeSize,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
            };
        }

    }
}
