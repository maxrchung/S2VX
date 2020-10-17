using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Editor;
using S2VX.Game.Play;

namespace S2VX.Game.SongSelection.Containers {
    public class SongPreview : CompositeDrawable {
        [Resolved]
        private ScreenStack Screens { get; set; }
        [Resolved]
        private TextureStore TextureStore { get; set; }

        public SongSelectionScreen SongSelectionScreen { get; set; }    // Set in SongSelectionScreen
        public string CurSelectionPath { get; set; }                    // Set in SongSelectionScreen
        private TextFlowContainer TextContainer { get; set; }
        private IconButton Thumbnail { get; set; }
        private IconButton BtnEdit { get; set; }
        private IconButton BtnPlay { get; set; }

        private void AddSongMetadata() {
            TextContainer.AddParagraph("SongTitle");
            TextContainer.AddParagraph("SongArtist");
            TextContainer.AddParagraph("StoryAuthor");
            TextContainer.AddParagraph("SlowestBPM ~ FastestBPM");
            TextContainer.AddParagraph("StoryLength");
            TextContainer.AddParagraph("CommandCount");
            TextContainer.AddParagraph("NoteCount");
            TextContainer.AddParagraph("MiscDescription");
        }

        [BackgroundDependencyLoader]
        private void Load() {
            var fullWidth = Width;
            var fullHeight = Height;
            var songInfoHeight = fullHeight * 0.6f;
            var btnHeight = fullHeight * 0.4f;
            var spacingMargin = 0.05f;
            var textSize = Height * 0.05f;
            var thumbnailSize = fullWidth * 0.3f;
            var btnSize = new Vector2(fullWidth / 5, fullHeight / 10);

            BtnEdit = new IconButton() {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Size = btnSize,
                Icon = FontAwesome.Solid.Edit,
                Action = () => SongSelectionScreen.Push(new EditorScreen()),
            };
            BtnPlay = new IconButton {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Size = btnSize,
                Icon = FontAwesome.Solid.Play,
                Action = () => SongSelectionScreen.Push(new PlayScreen(false)),
            };

            InternalChildren = new Drawable[] {
                // TODO: add blurred background
                new FillFlowContainer {
                    Width = fullWidth,
                    Height = songInfoHeight,
                    Children = new Drawable[] {
                        Thumbnail = new IconButton {
                            Size = new Vector2(thumbnailSize),
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            Margin = new MarginPadding {
                                Horizontal = fullWidth * spacingMargin,
                                Vertical = fullHeight * spacingMargin,
                            },
                            TextureName = "logo",
                        },
                        TextContainer = new TextFlowContainer(s => s.Font = new FontUsage("default", textSize)) {
                            Width = fullWidth - thumbnailSize - fullWidth * spacingMargin * 2,
                            Height = songInfoHeight,
                            Margin = new MarginPadding {
                                Vertical = fullHeight * spacingMargin,
                            },
                            TextAnchor = Anchor.TopLeft,
                            Colour = Color4.White,
                            // TODO: truncate text if it's too long
                        },
                    }
                },
                new FillFlowContainer {
                    Width = fullWidth,
                    Height = btnHeight,
                    Y = songInfoHeight,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Direction = FillDirection.Horizontal,
                    Children = new Drawable[] {
                        BtnEdit,
                        BtnPlay,
                    }
                }
            };

            AddSongMetadata();
        }

    }
}
