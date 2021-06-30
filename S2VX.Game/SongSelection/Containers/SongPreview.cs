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
using S2VX.Game.Leaderboard;
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
            var width = Width;
            var songInfoHeight = width * 0.4f;
            var leaderboardWidth = width * 0.6f;
            var leaderboardHeight = width * 0.4f;
            var btnHeight = width * 0.2f;
            var spacingMargin = 0.05f;
            var textSize = SizeConsts.TextSize1;
            var thumbnailSize = width * 0.3f;
            var btnSize = new Vector2(width / 5, width / 10);

            BtnEdit = new() {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Size = btnSize,
                Icon = FontAwesome.Solid.Edit,
                BorderColour = Color4.Red,
                Masking = true,
                Action = LoadEditor
            };
            BtnPlay = new() {
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
                    Width = width,
                    Height = songInfoHeight,
                    Children = new Drawable[] {
                        // Thumbnail
                        new IconButton {
                            Size = new Vector2(thumbnailSize),
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            Margin = new MarginPadding {
                                Horizontal = width * spacingMargin,
                                Vertical = width * spacingMargin,
                            },
                            Texture = ThumbnailTexture,
                            TextureName = "logo",
                        },
                        TextContainer = new TextFlowContainer(s => s.Font = new FontUsage("default", textSize)) {
                            Width = width - thumbnailSize - width * spacingMargin * 2,
                            Height = songInfoHeight,
                            Margin = new MarginPadding {
                                Vertical = width * spacingMargin,
                            },
                            TextAnchor = Anchor.TopLeft,
                            Colour = Color4.White,
                            // TODO: truncate text if it's too long
                        },
                    }
                },
                new LeaderboardContainer(StoryDirectory) {
                    Width = leaderboardWidth,
                    Height = leaderboardHeight,
                    Y = songInfoHeight,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                },
                new FillFlowContainer {
                    Width = width,
                    Height = btnHeight,
                    Y = songInfoHeight + leaderboardHeight,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Direction = FillDirection.Horizontal,
                    Children = new[] {
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
