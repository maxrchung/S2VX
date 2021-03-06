using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Editor;
using S2VX.Game.Play;
using S2VX.Game.Story;
using S2VX.Game.Story.Settings;
using System;
using System.IO;

namespace S2VX.Game.SongSelection.Containers {
    public class SongPreview : CompositeDrawable {
        public IconButton BtnEdit { get; private set; }
        public IconButton BtnPlay { get; private set; }

        private const string MetadataPath = "metadata.json";

        [Resolved]
        private ScreenStack Screens { get; set; }

        [Resolved]
        private AudioManager Audio { get; set; }

        private string StoryDirectory { get; set; }
        private string StoryPath { get; set; }
        private string AudioPath { get; set; }
        private Texture ThumbnailTexture { get; set; }
        private TextFlowContainer TextContainer { get; set; }

        public SongPreview(
            string storyDirectory,
            string storyFileName,
            string audioFileName,
            Texture thumbnailTexture = null
        ) {
            StoryDirectory = storyDirectory;
            StoryPath = Path.Combine(storyDirectory, storyFileName);
            AudioPath = Path.Combine(storyDirectory, audioFileName);
            ThumbnailTexture = thumbnailTexture;
        }

        private void AddSongMetadata() {
            var metadataPath = Path.Combine(StoryDirectory, MetadataPath);
            var metadata = new MetadataSettings();
            if (File.Exists(metadataPath)) {
                var text = File.ReadAllText(metadataPath);
                metadata = JsonConvert.DeserializeObject<MetadataSettings>(text);
            }

            TextContainer.AddParagraph($"Title: {metadata.SongTitle}");
            TextContainer.AddParagraph($"Artist: {metadata.SongArtist}");
            TextContainer.AddParagraph($"Author: {metadata.StoryAuthor}");
            TextContainer.AddParagraph($"Description: {metadata.MiscDescription}");
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
                BorderColour = Color4.Red,
                Masking = true,
                Action = LoadEditor
            };
            BtnPlay = new IconButton {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Size = btnSize,
                Icon = FontAwesome.Solid.Play,
                BorderColour = Color4.Red,
                Masking = true,
                Action = LoadPlay
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

        private void LoadEditor() {
            try {
                var story = new S2VXStory(StoryPath, true);
                var track = S2VXTrack.Open(AudioPath, Audio);
                Screens.Push(new EditorScreen(story, track));
            } catch (Exception exception) {
                BtnEdit.BorderThickness = 5;
                Console.WriteLine(exception);
            }
        }

        private void LoadPlay() {
            try {
                var story = new S2VXStory(StoryPath, false);
                var track = S2VXTrack.Open(AudioPath, Audio);
                Screens.Push(new PlayScreen(false, story, track));
            } catch (Exception exception) {
                BtnPlay.BorderThickness = 5;
                Console.WriteLine(exception);
            }
        }
    }
}
