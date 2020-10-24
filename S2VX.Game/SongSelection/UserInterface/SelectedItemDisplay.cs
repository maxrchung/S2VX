using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Play;

namespace S2VX.Game.SongSelection.UserInterface {
    public class SelectedItemDisplay : CompositeDrawable {
        [Resolved]
        private ScreenStack Screens { get; set; }
        [Resolved]
        private AudioManager Audio { get; set; }

        public string ItemName { get; set; }
        public string CurSelectionPath { get; set; }
        public Texture ThumbnailTexture { get; set; }
        public RelativeBox SelectedIndicatorBox { get; private set; }
        private IconButton Thumbnail { get; set; }

        public SelectedItemDisplay(string itemName, string curSelectionPath, Texture thumbnailTexture = null) {
            ItemName = itemName;
            CurSelectionPath = curSelectionPath;
            ThumbnailTexture = thumbnailTexture;
        }

        protected override bool OnHover(HoverEvent e) {
            SelectedIndicatorBox.Alpha = 1;
            Thumbnail.Alpha = 0.7f;
            return false;
        }

        protected override void OnHoverLost(HoverLostEvent e) {
            SelectedIndicatorBox.Alpha = 0;
            Thumbnail.Alpha = 1;
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
                    Texture = ThumbnailTexture,
                    TextureName = "logo",
                    Action = () => {
                        Audio.Samples.Get("menuhit").Play();
                        Screens.Push(new SongSelectionScreen { CurSelectionPath = CurSelectionPath + "/" + ItemName });
                    },
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
