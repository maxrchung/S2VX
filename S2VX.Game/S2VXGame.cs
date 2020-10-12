using osu.Framework.Allocation;
using osu.Framework.Screens;
using S2VX.Game.Play;

namespace S2VX.Game {
    public class S2VXGame : S2VXGameBase {
        [BackgroundDependencyLoader]
        private void Load() => Child = new ScreenStack(new SongSelectionScreen());
    }
}
