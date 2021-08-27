using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK.Graphics;
using osuTK.Input;
using S2VX.Game.SongSelection.Containers;
using S2VX.Game.SongSelection.UserInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace S2VX.Game.SongSelection {
    public class SongSelectionScreen : Screen {
        [Resolved]
        private AudioManager Audio { get; set; }

        [Resolved]
        private S2VXCursor Cursor { get; set; }

        public string CurSelectionPath { get; set; } = "Stories";

        private NativeStorage Storage { get; set; }
        private StorageBackedResourceStore CurLevelResourceStore { get; set; }
        private SongPreview SongPreview { get; set; }
        private List<SelectedItemDisplay> StoryList { get; set; } = new();

        private const float FullWidth = S2VXGameBase.GameWidth;
        private const float FullHeight = S2VXGameBase.GameWidth;
        private const float InnerSize = 0.9f;

        private void Clear() => StoryList.Clear();

        private void CreateSelectionItems(string deletedDir = null) {
            var dirs = Storage.GetDirectories("");

            foreach (var dir in dirs) {
                if (dir == deletedDir) { continue; }

                var thumbnailPath = Storage.GetFiles(dir, "thumbnail.*").FirstOrDefault();
                StoryList.Add(new SelectedItemDisplay(
                    () => {
                        Clear();
                        TryDeleteDirectory(dir);
                        CreateSelectionItems(dir);
                        LoadSelectionScreen();
                    },
                    dir,
                    CurSelectionPath,
                    string.IsNullOrEmpty(thumbnailPath) ? null : Texture.FromStream(CurLevelResourceStore.GetStream(thumbnailPath))
                ));
            }
        }

        private void TryDeleteDirectory(string dir) {
            var hasException = true;
            while (hasException) {
                try {
                    Storage.DeleteDirectory(dir);
                    hasException = false;
                } catch (Exception ex) {
                    // Don't really know how to handle this weird async stuff
                    Console.WriteLine(ex);
                }
            }
        }

        private (bool, string, string, Texture) DirectoryContainsStory(string dir) {
            var story = Storage.GetFiles(dir, "*.s2ry");
            var song = Storage.GetFiles(dir, "audio.mp3");
            var thumbnailPath = Storage.GetFiles(dir, "thumbnail.*").FirstOrDefault();
            var thumbnail = string.IsNullOrEmpty(thumbnailPath) ? null : Texture.FromStream(CurLevelResourceStore.GetStream(thumbnailPath));
            return (story.Count() == 1 && song.Count() == 1, story.FirstOrDefault(), song.FirstOrDefault(), thumbnail);
        }

        private bool DirectoryContainsDirectories(string dir) => Storage.GetDirectories(dir).Any();

        // Go up one level by exiting and thus popping ourself out from the ScreenStack
        public override bool OnExiting(IScreen next) {
            // Unless we're already at root level
            if (CurSelectionPath != "Stories") {
                Audio.Samples.Get("menuback").Play();
                return false;
            }
            // Block Exiting
            return true;
        }

        public override void OnResuming(IScreen last) => SongPreview?.LeaderboardContainer?.LoadLeaderboard();

        protected override bool OnKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.Escape:
                    this.Exit();
                    return true;
                default:
                    break;
            }
            return false;
        }

        [BackgroundDependencyLoader]
        private void Load() {
            Storage = new NativeStorage(CurSelectionPath);
            // Create the Stories root level directory if it does not exist
            if (CurSelectionPath == "Stories" && !Storage.Exists("")) {
                Directory.CreateDirectory(CurSelectionPath);
            }
            CurLevelResourceStore = new StorageBackedResourceStore(Storage);

            CreateSelectionItems();
            LoadSelectionScreen();
        }

        private void LoadSelectionScreen() {
            var spacingMargin = 0.1f;

            if (DirectoryContainsDirectories("")) {
                InternalChildren = new Drawable[] {
                    new Border(CurSelectionPath, () => this.Exit()) {
                        Width = FullWidth,
                        Height = FullHeight,
                        InnerBoxRelativeSize = InnerSize,
                    },
                    new S2VXScrollContainer {
                        Width = FullWidth * InnerSize,
                        Height = FullHeight * InnerSize,
                        Margin = new MarginPadding {
                            Horizontal = Width * spacingMargin,
                            Vertical = Height * spacingMargin,
                        },
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Child = new FillFlowContainer {
                            Width = FullWidth * InnerSize,
                            AutoSizeAxes = Axes.Y,
                            Direction = FillDirection.Full,
                            Children = StoryList
                        },
                    },
                };
            } else {
                var (directoryContainsStory, storyPath, audioPath, thumbnailTexture) = DirectoryContainsStory("");
                if (!directoryContainsStory) {
                    // Empty directory, show red border
                    InternalChildren = new Drawable[] {
                        new Border(CurSelectionPath, () => this.Exit()) {
                            Width = FullWidth,
                            Height = FullHeight,
                            InnerBoxRelativeSize = InnerSize,
                            Colour = Color4.Red,
                        },
                    };
                } else {
                    InternalChildren = new Drawable[] {
                        new Border(CurSelectionPath, () => this.Exit()) {
                            Width = FullWidth,
                            Height = FullHeight,
                            InnerBoxRelativeSize = InnerSize,
                        },
                        SongPreview = new SongPreview (CurSelectionPath, storyPath, audioPath, thumbnailTexture) {
                            Width = FullWidth * InnerSize,
                            Height = FullHeight * InnerSize,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                        },
                    };
                }
            }
        }
    }
}
