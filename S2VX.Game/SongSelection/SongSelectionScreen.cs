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
        private static Vector2 Spacing { get; } = new Vector2(15);

        [Cached]
        private EditorScreen Editor { get; set; } = new EditorScreen();

        [Resolved]
        private ScreenStack Screens { get; set; }

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
            BtnEdit.Action = () => Screens.Push(Editor);
            BtnPlay.Action = () => Screens.Push(new PlayScreen());
            InternalChild = new FillFlowContainer {
                Direction = FillDirection.Horizontal,
                Spacing = Spacing,
                Children = new Drawable[]
                {
                    BtnEdit,
                    BtnPlay
                }
            };
        }
    }
}
