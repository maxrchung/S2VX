using osu.Framework.Allocation;
using S2VX.Game.Editor;

namespace S2VX.Game {
    public class S2VXGame : S2VXGameBase {
        [Cached]
        private readonly S2VXEditor Editor = new S2VXEditor();

        [BackgroundDependencyLoader]
        private void Load() => Child = Editor;
    }
}
