using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using S2VX.Game.SongSelection.UserInterface;

namespace S2VX.Game.SongSelection.Containers {
    public class Border : CompositeDrawable {
        public string CurSelectionPath { get; set; }
        public float InnerBoxRelativeSize { get; set; } = 0.9f;

        public Border(string curSelectionPath) => CurSelectionPath = curSelectionPath;

        [BackgroundDependencyLoader]
        private void Load() {
            var fullWidth = S2VXGameBase.GameWidth;
            var fullHeight = S2VXGameBase.GameWidth;
            var borderSize = fullHeight * (1 - InnerBoxRelativeSize) / 2;
            var titleSize = borderSize * 0.5f;
            var spacingMargin = 0.02f;

            Width = fullWidth;
            Height = fullHeight;

            InternalChildren = new Drawable[] {
                // BorderOuterBox
                new BorderOuterBox { },
                // TextContainer
                new TextFlowContainer(s => s.Font = new FontUsage("default", titleSize)) {
                    Width = fullWidth,
                    Height = borderSize,
                    Margin = new MarginPadding {
                        Horizontal = fullWidth * spacingMargin,
                    },
                    Text = CurSelectionPath,
                    TextAnchor = Anchor.CentreLeft,
                    Colour = Color4.Black,
                    // TODO: truncate text if it's too long
                },
                // BorderInnerBox
                new BorderInnerBox {
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
