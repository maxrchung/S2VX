using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK;
using S2VX.Game.Editor;

namespace S2VX.Game.Play {
    public class SongSelectionScreen : Screen {
        private static Vector2 InputSize { get; } = new Vector2(100, 30);

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
                    BtnPlay
                }
            };
        }
    }
}
