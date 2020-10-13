using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using S2VX.Game.Editor;
using System.Globalization;
using System.IO;
using System.Linq;

namespace S2VX.Game.Play {
    public class SongSelectionScreen : Screen {
        private static Vector2 InputSize { get; } = new Vector2(100, 30);

        private static NativeStorage StoriesStorage { get; } = new NativeStorage("Stories");

        private TextFlowContainer Storybooks { get; set; }

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
            BtnEdit.Action = () => this.Push(new EditorScreen());
            BtnPlay.Action = () => this.Push(new PlayScreen(false));
            InternalChild = new FillFlowContainer {
                Direction = FillDirection.Horizontal,
                Children = new Drawable[]
                {
                    BtnEdit,
                    BtnPlay,
                    Storybooks = new TextFlowContainer(s => s.Font = new FontUsage("default", 30)),
                }
            };

            // Will create the Stories directory if it does not exist
            if (!StoriesStorage.Exists("")) {
                Directory.CreateDirectory("Stories");
            }
            var numStorybooks = StoriesStorage.GetDirectories("").Count();
            Storybooks.Text = numStorybooks.ToString(CultureInfo.InvariantCulture);
        }

    }
}
