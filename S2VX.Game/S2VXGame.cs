using osu.Framework.Allocation;
using osu.Framework.Screens;
using S2VX.Game.Play;

namespace S2VX.Game {
    public class S2VXGame : S2VXGameBase {
        [Cached]
        private ScreenStack Screens { get; set; } = new ScreenStack();

        [BackgroundDependencyLoader]
        private void Load() {
            Screens.Push(new SongSelectionScreen());
            Child = Screens;
        }
    }
}
