using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using S2VX.Game.SongSelection.UserInterface;
using System;

namespace S2VX.Game.SongSelection.Containers {
    public class Border : CompositeDrawable {
        public string CurSelectionPath { get; set; }
        public float InnerBoxRelativeSize { get; set; } = 0.9f;
        private Action OnExit { get; }
        public BorderOuterBox BorderOuter { get; private set; }
        public TextFlowContainer TxtPath { get; private set; }
        public BorderInnerBox BorderInner { get; private set; }

        public Border(string curSelectionPath, Action onExit) {
            CurSelectionPath = curSelectionPath ?? "";
            OnExit = onExit;
        }

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
                BorderOuter = new BorderOuterBox(OnExit),
                TxtPath = new TextFlowContainer(s => s.Font = new FontUsage("default", titleSize)) {
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
                BorderInner = new BorderInnerBox(),
            };
        }

    }
}
