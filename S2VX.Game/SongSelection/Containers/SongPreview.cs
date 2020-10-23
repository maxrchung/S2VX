using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Editor;
using S2VX.Game.Play;

namespace S2VX.Game.SongSelection.Containers {
    public class SongPreview : CompositeDrawable {

        [Resolved]
        private ScreenStack Screens { get; set; }

        public string CurSelectionPath { get; set; }
        public string StoryPath { get; set; }
        public string AudioPath { get; set; }
        public StorageBackedResourceStore CurLevelResourceStore { get; set; }
        public Texture ThumbnailTexture { get; set; }
        private TextFlowContainer TextContainer { get; set; }
        private IconButton BtnEdit { get; set; }
        private IconButton BtnPlay { get; set; }

        public SongPreview(
            string curSelectionPath,
            string storyPath,
            string audioPath,
            StorageBackedResourceStore curLevelResourceStore,
            Texture thumbnailTexture = null
        ) {
            CurSelectionPath = curSelectionPath;
            StoryPath = storyPath;
            AudioPath = audioPath;
            CurLevelResourceStore = curLevelResourceStore;
            ThumbnailTexture = thumbnailTexture;
        }

        private void AddSongMetadata() {
            // TODO: Connect these to .s2ry's metadata and play a section of the song
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
                Action = () =>
                    Screens.Push(new EditorScreen(CurSelectionPath, StoryPath, CurLevelResourceStore, AudioPath)),
            };
            BtnPlay = new IconButton {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Size = btnSize,
                Icon = FontAwesome.Solid.Play,
                Action = () =>
                    Screens.Push(new PlayScreen(false, CurSelectionPath, StoryPath, CurLevelResourceStore, AudioPath)),
            };

            InternalChildren = new Drawable[] {
                // TODO: add blurred background
                new FillFlowContainer {
                    Width = fullWidth,
                    Height = songInfoHeight,
                    Children = new Drawable[] {
                        // Thumbnail
                        new IconButton {
                            Size = new Vector2(thumbnailSize),
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            Margin = new MarginPadding {
                                Horizontal = fullWidth * spacingMargin,
                                Vertical = fullHeight * spacingMargin,
                            },
                            Texture = ThumbnailTexture,
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
