using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
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
        private ScreenStack Screens { get; set; }

        public string CurSelectionPath { get; set; } = "Stories";

        private TextFlowContainer CurSelectionPathTxt { get; set; }

        public static NativeStorage Storage { get; set; }
        //private static StorageBackedResourceStore CurLevelResourceStore { get; } = new StorageBackedResourceStore(Storage);

        private FillFlowContainer SelectionItems { get; set; }

        private List<Drawable> CreateSelectionItems() {
            var selectionItems = new List<Drawable>();
            var dirs = Storage.GetDirectories("");
            foreach (var dir in dirs) {
                if (DirectoryContainsStory(dir) || DirectoryContainsDirectories(dir)) {
                    selectionItems.Add(new SelectedItemDisplay {
                        ItemName = dir,
                        SongSelectionScreen = this,
                    });
                }
            }
            return selectionItems;
        }

        private static bool DirectoryContainsStory(string dir) {
            var story = Storage.GetFiles(dir, "*.s2ry");
            var song = Storage.GetFiles(dir, "audio.mp3");
            return story.Any() && song.Count() == 1;
        }

        private static bool DirectoryContainsDirectories(string dir) => Storage.GetDirectories(dir).Any();

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

        // Go up one level by exiting and thus popping ourself out from the ScreenStack
        public void DoExit() {
            // Unless we're already at root level
            if (CurSelectionPath != "Stories") {
                this.Exit();
            }
        }

        [BackgroundDependencyLoader]
        private void Load() {
            Storage = new NativeStorage(CurSelectionPath);
            // Create the Stories root level directory if it does not exist
            if (CurSelectionPath == "Stories" && !Storage.Exists("")) {
                Directory.CreateDirectory(CurSelectionPath);
            }

            var fullWidth = Screens.DrawWidth;
            var fullHeight = Screens.DrawHeight;
            var innerSize = 0.9f;
            var spacingMargin = 0.1f;

            SelectionItems = new FillFlowContainer {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Width = 1.0f,
                Height = 1.0f,
                Direction = FillDirection.Full,
                Children = CreateSelectionItems(),
            };

            InternalChildren = new Drawable[] {
                new SelectionScreenBorder {
                    Width = fullWidth,
                    Height = fullHeight,
                    InnerBoxRelativeSize = innerSize,
                    SongSelectionScreen = this,
                    CurSelectionPath = CurSelectionPath,
                },
                new FillFlowContainer {
                    Width = fullWidth * innerSize,
                    Height = fullHeight * innerSize,
                    Direction = FillDirection.Full,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        new BasicScrollContainer {
                            Width = fullWidth * innerSize,
                            Height = fullHeight * innerSize,
                            Margin = new MarginPadding {
                                Horizontal = Width *spacingMargin,
                                Vertical = Height * spacingMargin,
                            },
                            Child = SelectionItems
                        },
                    }
                },
            };
        }

    }
}
