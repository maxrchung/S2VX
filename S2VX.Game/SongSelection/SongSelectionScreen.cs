using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
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
<<<<<<< HEAD
        [Resolved]
        private ScreenStack Screens { get; set; }
        [Resolved]
        private AudioManager Audio { get; set; }

        public string CurSelectionPath { get; set; } = "Stories";

        private TextFlowContainer CurSelectionPathTxt { get; set; }
        private NativeStorage Storage { get; set; }
        private StorageBackedResourceStore CurLevelResourceStore { get; set; }

        private List<Drawable> CreateSelectionItems() {
            var selectionItems = new List<Drawable>();
            var dirs = Storage.GetDirectories("");
            foreach (var dir in dirs) {
                selectionItems.Add(new SelectedItemDisplay {
                    ItemName = dir,
                    SongSelectionScreen = this,
                });
            }
            return selectionItems;
        }

        private (bool, string, string) DirectoryContainsStory(string dir) {
            var story = Storage.GetFiles(dir, "*.s2ry");
            var song = Storage.GetFiles(dir, "audio.mp3");
            return (story.Count() == 1 && song.Count() == 1, story.FirstOrDefault(), song.FirstOrDefault());
        }

        private bool DirectoryContainsDirectories(string dir) => Storage.GetDirectories(dir).Any();

        // Go up one level by exiting and thus popping ourself out from the ScreenStack
        public void DoExit() {
            // Unless we're already at root level
            if (CurSelectionPath != "Stories") {
                Audio.Samples.Get("menuback").Play();
                this.Exit();
            }
        }

        protected override bool OnKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.Escape:
                    DoExit();
                    return true;
                default:
                    break;
            }
            return false;
        }
=======
        private static Vector2 BtnSize { get; } = new Vector2(100, 30);

        private Button BtnEdit { get; } = new BasicButton() {
            Size = BtnSize,
            Text = "Edit"
        };

        private Button BtnPlay { get; } = new BasicButton() {
            Size = BtnSize,
            Text = "Play"
        };
>>>>>>> master

        [BackgroundDependencyLoader]
        private void Load() {
            Audio.Samples.Get("menuhit").Play();
            Storage = new NativeStorage(CurSelectionPath);
            // Create the Stories root level directory if it does not exist
            if (CurSelectionPath == "Stories" && !Storage.Exists("")) {
                Directory.CreateDirectory(CurSelectionPath);
            }
            CurLevelResourceStore = new StorageBackedResourceStore(Storage);

            var fullWidth = Screens.DrawWidth;
            var fullHeight = Screens.DrawHeight;
            var innerSize = 0.9f;
            var spacingMargin = 0.1f;

            if (DirectoryContainsDirectories("")) {
                InternalChildren = new Drawable[] {
                    new Border {
                        Width = fullWidth,
                        Height = fullHeight,
                        InnerBoxRelativeSize = innerSize,
                        SongSelectionScreen = this,
                        CurSelectionPath = CurSelectionPath,
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
                            Height = fullHeight * innerSize,
                            Direction = FillDirection.Full,
                            Children = CreateSelectionItems(),
                        },
                    },
                };
            } else {
                var (directoryContainsStory, storyDir, audioDir) = DirectoryContainsStory("");
                if (!directoryContainsStory) {
                    // Empty directory, show red border
                    InternalChildren = new Drawable[] {
                        new Border {
                            Width = fullWidth,
                            Height = fullHeight,
                            InnerBoxRelativeSize = innerSize,
                            SongSelectionScreen = this,
                            CurSelectionPath = CurSelectionPath,
                            Colour = Color4.Red,
                        },
                    };
                } else {
                    InternalChildren = new Drawable[] {
                        new Border {
                            Width = fullWidth,
                            Height = fullHeight,
                            InnerBoxRelativeSize = innerSize,
                            SongSelectionScreen = this,
                            CurSelectionPath = CurSelectionPath,
                        },
                        new SongPreview {
                            Width = fullWidth * innerSize,
                            Height = fullHeight * innerSize,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            SongSelectionScreen = this,
                            CurSelectionPath = CurSelectionPath,
                            StoryDir = storyDir,
                            AudioDir = audioDir,
                            CurLevelResourceStore = CurLevelResourceStore,
                        },
                    };
                }
            }
        }
    }
}
