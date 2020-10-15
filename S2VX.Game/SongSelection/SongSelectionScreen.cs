using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using osuTK.Input;
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
        //private static StorageBackedResourceStore CurLevelResourceStore { get; } = new StorageBackedResourceStore(RootLevelStorage);

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
            var story = Storage.GetFiles(dir, "story.json");
            //var song = RootLevelStorage.GetFiles(dir, "*.mp3");
            return story.Count() == 1;
        }

        private static bool DirectoryContainsDirectories(string dir) => Storage.GetDirectories(dir).Any();

        protected override bool OnKeyDown(KeyDownEvent e) {
            switch (e.Key) {
                case Key.Escape:
                    // Up one level, unless we're already at root level
                    if (CurSelectionPath != "Stories") {
                        this.Exit();
                        return true;
                    }
                    break;
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

            var fullWidth = Screens.DrawWidth;
            var fullHeight = Screens.DrawHeight;
            var titleSize = fullHeight / 30;

            SelectionItems = new FillFlowContainer {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Width = 1.0f,
                Height = 1.0f,
                Direction = FillDirection.Full,
                Children = CreateSelectionItems(),
            };

            InternalChild = new FillFlowContainer {
                Width = fullWidth,
                Height = fullHeight,
                Direction = FillDirection.Full,
                Children = new Drawable[]
                {
                    CurSelectionPathTxt = new TextFlowContainer(s => s.Font = new FontUsage("default", titleSize)) {
                        Width = fullWidth,
                        Height = titleSize,
                        Text = CurSelectionPath,
                    },
                    new BasicScrollContainer {
                        Width = Screens.DrawWidth,
                        Height = Screens.DrawHeight,
                        Margin = new MarginPadding {
                            Vertical = Width / 10,
                            Horizontal = Width / 10
                        },
                        Child = SelectionItems
                    },
                }
            };
        }

    }
}
