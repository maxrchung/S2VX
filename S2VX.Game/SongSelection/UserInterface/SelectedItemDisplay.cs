﻿using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Play;

namespace S2VX.Game.SongSelection.UserInterface {
    public class SelectedItemDisplay : CompositeDrawable {
        [Resolved]
        private ScreenStack Screens { get; set; }


        public SongSelectionScreen SongSelectionScreen { get; set; }    // Set in SongSelectionScreen
        public string ItemName { get; set; }                            // Set in SongSelectionScreen
        public RelativeBox SelectedIndicatorBox { get; private set; }
        private IconButton Thumbnail { get; set; }

        protected override bool OnHover(HoverEvent e) {
            SelectedIndicatorBox.Alpha = 1;
            Thumbnail.Alpha = 0.7f;
            return false;
        }

        protected override void OnHoverLost(HoverLostEvent e) {
            SelectedIndicatorBox.Alpha = 0;
            Thumbnail.Alpha = 1;
        }

        protected override bool OnClick(ClickEvent e) {
            Screens.Push(new SongSelectionScreen {
                CurSelectionPath = SongSelectionScreen.CurSelectionPath + "/" + ItemName,
            });
            return true;
        }

        [BackgroundDependencyLoader]
        private void Load() {
            var boxSize = Screens.DrawWidth / 4;
            var textSize = Screens.DrawWidth / 40;

            Width = boxSize;
            Height = boxSize;
            Margin = new MarginPadding {
                Vertical = boxSize / 10,
                Horizontal = boxSize / 10
            };

            InternalChildren = new Drawable[] {
                SelectedIndicatorBox = new RelativeBox {
                    Width = 1.1f,
                    Height = 1.1f,
                    Alpha = 0
                },
                Thumbnail = new IconButton {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(boxSize),
                    TextureName = "logo",
                    Action = () => Screens.Push(new SongSelectionScreen {
                        CurSelectionPath = SongSelectionScreen.CurSelectionPath + "/" + ItemName,
                    }),
                },
                // TextShadowBox
                new RelativeBox {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Height = 0.2f,
                    Colour = Color4.Black.Opacity(0.5f),
                },
                // TextContainer
                new TextFlowContainer(s => s.Font = new FontUsage("default", textSize)) {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Y = 0.8f,
                    Height = 0.2f,
                    TextAnchor = Anchor.TopCentre,
                    Text = ItemName,
                },
            };
        }

    }
}