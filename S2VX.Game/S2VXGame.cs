using osu.Framework.Allocation;
using osu.Framework.Screens;
using S2VX.Game.Editor;

namespace S2VX.Game {
    public class S2VXGame : S2VXGameBase {
        [Cached]
        private EditorScreen Editor { get; set; } = new EditorScreen();

        [Cached]
        private ScreenStack Screens { get; set; } = new ScreenStack();

        [BackgroundDependencyLoader]
        private void Load() {
            Screens.Push(Editor);
            Child = Screens;
        }
    }
}
