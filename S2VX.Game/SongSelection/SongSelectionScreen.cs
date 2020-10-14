using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using S2VX.Game.Editor;
using S2VX.Game.SongSelection.UserInterface;
using System.Collections.Generic;
using System.IO;

namespace S2VX.Game.Play {
    public class SongSelectionScreen : Screen {
        [Resolved]
        private ScreenStack Screens { get; set; }
        private static Vector2 InputSize { get; } = new Vector2(100, 30);

        private static NativeStorage RootLevelStorage { get; } = new NativeStorage("Stories");
        //private static StorageBackedResourceStore CurLevelResourceStore { get; } = new StorageBackedResourceStore(RootLevelStorage);

        private FillFlowContainer SelectionItems { get; set; }

        private static List<Drawable> CreateSelectionItems() {
            var selectionItems = new List<Drawable>();
            var dirs = RootLevelStorage.GetDirectories("");
            foreach (var dir in dirs) {
                selectionItems.Add(new SelectedItemDisplay {
                    Text = dir
                });
            }
            return selectionItems;
        }

        private Button BtnEdit { get; } = new BasicButton() {
            Size = InputSize,
            Text = "Edit"
        };

        private Button BtnPlay { get; } = new BasicButton() {
            Size = InputSize,
            Text = "Play"
        };

        [BackgroundDependencyLoader]
        private void Load() {
            // Create the Stories directory if it does not exist
            if (!RootLevelStorage.Exists("")) {
                Directory.CreateDirectory("Stories");
            }

            BtnEdit.Action = () => this.Push(new EditorScreen());
            BtnPlay.Action = () => this.Push(new PlayScreen(false));

            SelectionItems = new FillFlowContainer {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Width = 1.0f,
                Height = 1.0f,
                Direction = FillDirection.Full,
                Children = CreateSelectionItems(),
            };

            InternalChild = new FillFlowContainer {
                Width = Screens.DrawWidth,
                Height = Screens.DrawHeight,
                Direction = FillDirection.Full,
                Children = new Drawable[]
                {
                    BtnEdit,
                    BtnPlay,
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
