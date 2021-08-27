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
using System.Globalization;
using System.IO;
using System.Linq;

namespace S2VX.Game.SongSelection {
    public class SongSelectionScreen : Screen {
        [Resolved]
        private S2VXGameBase GameBase { get; set; }

        [Resolved]
        private ScreenStack Screens { get; set; }

        [Resolved]
        private AudioManager Audio { get; set; }

        [Resolved]
        private S2VXCursor Cursor { get; set; }

        public string CurSelectionPath { get; set; } = "Stories";

        private NativeStorage Storage { get; set; }
        private StorageBackedResourceStore CurLevelResourceStore { get; set; }
        private SongPreview SongPreview { get; set; }
        private List<Drawable> SelectionItems { get; set; } = new();

        private void Clear() => SelectionItems.Clear();

        private void CreateSelectionItems(string deletedDir = null) {
            var dirs = Storage.GetDirectories("");
            foreach (var dir in dirs) {
                if (dir == deletedDir) { continue; }

                var thumbnailPath = Storage.GetFiles(dir, "thumbnail.*").FirstOrDefault();
                SelectionItems.Add(new SelectedItemDisplay(
                    () => DeleteSelectionItem(dir),
                    dir,
                    CurSelectionPath,
                    string.IsNullOrEmpty(thumbnailPath) ? null : Texture.FromStream(CurLevelResourceStore.GetStream(thumbnailPath))
                ));
            }
        }

        public void DeleteSelectionItem(string dir) {
            Clear();
            TryDeleteDirectory(dir);
            CreateSelectionItems(dir);
            LoadSelectionScreen();
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

        /// <summary>
        /// Creates a new story from template with the specified filepath
        /// </summary>
        /// <param name="filePath">The mp3 audio file to use for this new story</param>
        public void Import(string filePath) {
            // Only handle the import if this screen is on top
            if (Screens.CurrentScreen == this) {
                var templatePath = "Template";
                var targetPath = Path.Combine(CurSelectionPath, Path.GetFileNameWithoutExtension(filePath));

                // Create new directory
                var dupNumber = 1;
                while (Directory.Exists(targetPath)) {
                    targetPath = Path.Combine(CurSelectionPath,
                        Path.GetFileNameWithoutExtension(filePath) + dupNumber.ToString(CultureInfo.InvariantCulture));
                    dupNumber++;
                }
                Directory.CreateDirectory(targetPath);

                // Copy dragged MP3 and rename it audio.mp3
                File.Copy(filePath, Path.Combine(targetPath, "audio.mp3"));

                // Copy other files from template
                foreach (var file in Directory.GetFiles(templatePath)) {
                    File.Copy(file, Path.Combine(targetPath, Path.GetFileName(file)));
                }

                Audio.Samples.Get("menuhit").Play();

                // Reload selection items
                Clear();
                CreateSelectionItems();
                Schedule(LoadSelectionScreen);  // Only the update thread can mutate InternalChildren
            }
        }

        [BackgroundDependencyLoader]
        private void Load() {
            Storage = new NativeStorage(CurSelectionPath);
            // Create the Stories root level directory if it does not exist
            if (CurSelectionPath == "Stories" && !Storage.Exists("")) {
                Directory.CreateDirectory(CurSelectionPath);
            }
            CurLevelResourceStore = new StorageBackedResourceStore(Storage);
            GameBase.RegisterImportHandler(this);
            CreateSelectionItems();
            LoadSelectionScreen();
        }

        private void LoadSelectionScreen() {
            var fullWidth = S2VXGameBase.GameWidth;
            var fullHeight = S2VXGameBase.GameWidth;
            var innerSize = 0.9f;
            var spacingMargin = 0.1f;

            if (DirectoryContainsDirectories("")) {
                InternalChildren = new Drawable[] {
                    new Border(CurSelectionPath, () => this.Exit()) {
                        Width = fullWidth,
                        Height = fullHeight,
                        InnerBoxRelativeSize = innerSize,
                    },
                    new S2VXScrollContainer {
                        Width = fullWidth * innerSize,
                        Height = fullHeight * innerSize,
                        Margin = new MarginPadding {
                            Horizontal = Width * spacingMargin,
                            Vertical = Height * spacingMargin,
                        },
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Child = new FillFlowContainer {
                            Width = fullWidth * innerSize,
                            AutoSizeAxes = Axes.Y,
                            Direction = FillDirection.Full,
                            Children = SelectionItems
                        },
                    },
                };
            } else {
                var (directoryContainsStory, storyPath, audioPath, thumbnailTexture) = DirectoryContainsStory("");
                if (!directoryContainsStory) {
                    // Empty directory, show red border
                    InternalChildren = new Drawable[] {
                        new Border(CurSelectionPath, () => this.Exit()) {
                            Width = fullWidth,
                            Height = fullHeight,
                            InnerBoxRelativeSize = innerSize,
                            Colour = Color4.Red,
                        },
                    };
                } else {
                    InternalChildren = new Drawable[] {
                        new Border(CurSelectionPath, () => this.Exit()) {
                            Width = fullWidth,
                            Height = fullHeight,
                            InnerBoxRelativeSize = innerSize,
                        },
                        SongPreview = new SongPreview (CurSelectionPath, storyPath, audioPath, thumbnailTexture) {
                            Width = fullWidth * innerSize,
                            Height = fullHeight * innerSize,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                        },
                    };
                }
            }
        }

        protected override void Dispose(bool isDisposing) {
            base.Dispose(isDisposing);
            GameBase?.UnregisterImportHandler(this);
        }
    }
}
