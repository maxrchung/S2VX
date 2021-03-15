using osu.Framework.Allocation;
using osu.Framework.Screens;
using S2VX.Game.SongSelection;
using System;

[assembly: CLSCompliant(false)]
namespace S2VX.Game {
    public class S2VXGame : S2VXGameBase {
        [Cached]
        private ScreenStack Screens { get; set; } = new ScreenStack();

        [Cached]
        private S2VXCursor Cursor { get; set; } = new S2VXCursor();

        [BackgroundDependencyLoader]
        private void Load() {
            Screens.Push(new SongSelectionScreen());
            Child = new SquareContainer {
                Screens,
                Cursor
            };
        }
    }
}
