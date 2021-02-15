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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace S2VX.Game.Play {
    public class SongSelectionScreen : Screen {
        [Resolved]
        private AudioManager Audio { get; set; }

        public string CurSelectionPath { get; set; } = "Stories";

        private NativeStorage Storage { get; set; }
        private StorageBackedResourceStore CurLevelResourceStore { get; set; }

        private List<Drawable> CreateSelectionItems() {
            var selectionItems = new List<Drawable>();
            var dirs = Storage.GetDirectories("");
            foreach (var dir in dirs) {
                var thumbnailPath = Storage.GetFiles(dir, "thumbnail.*").FirstOrDefault();
                selectionItems.Add(new SelectedItemDisplay(
                    dir,
                    CurSelectionPath,
                    string.IsNullOrEmpty(thumbnailPath) ? null : Texture.FromStream(CurLevelResourceStore.GetStream(thumbnailPath))
                ));
            }
            return selectionItems;
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

            var fullWidth = S2VXGameBase.GameWidth;
            var fullHeight = S2VXGameBase.GameWidth;
            var innerSize = 0.9f;
            var spacingMargin = 0.1f;

            if (DirectoryContainsDirectories("")) {
                InternalChildren = new Drawable[] {
                    new Border(CurSelectionPath) {
                        Width = fullWidth,
                        Height = fullHeight,
                        InnerBoxRelativeSize = innerSize,
                    },
                    new BasicScrollContainer {
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
                            Children = CreateSelectionItems()
                        },
                    },
                };
            } else {
                var (directoryContainsStory, storyPath, audioPath, thumbnailTexture) = DirectoryContainsStory("");
                if (!directoryContainsStory) {
                    // Empty directory, show red border
                    InternalChildren = new Drawable[] {
                        new Border(CurSelectionPath) {
                            Width = fullWidth,
                            Height = fullHeight,
                            InnerBoxRelativeSize = innerSize,
                            Colour = Color4.Red,
                        },
                    };
                } else {
                    InternalChildren = new Drawable[] {
                        new Border(CurSelectionPath) {
                            Width = fullWidth,
                            Height = fullHeight,
                            InnerBoxRelativeSize = innerSize,
                        },
                        new SongPreview (CurSelectionPath, storyPath, audioPath, thumbnailTexture) {
                            Width = fullWidth * innerSize,
                            Height = fullHeight * innerSize,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                        },
                    };
                }
            }
        }
    }
}
