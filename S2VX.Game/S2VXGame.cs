using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using S2VX.Game.SongSelection;
using System;

[assembly: CLSCompliant(false)]
namespace S2VX.Game {
    public class S2VXGame : S2VXGameBase {
        [Cached]
        private ScreenStack Screens { get; set; } = new ScreenStack();

        [Cached]
        private GlobalVolumeDisplay VolumeDisplay { get; set; } = new();

        protected override bool OnScroll(ScrollEvent e) {
            if (e.AltPressed) {
                if (e.ScrollDelta.Y > 0) {
                    VolumeDisplay.VolumeIncrease();
                } else {
                    VolumeDisplay.VolumeDecrease();
                }
                VolumeDisplay.UpdateDisplay();
            }
            return false;
        }

        [BackgroundDependencyLoader]
        private void Load() {
            Screens.Push(new SongSelectionScreen());
            Child = new SquareContainer {
                Screens,
                VolumeDisplay,
                Cursor
            };
        }
    }
}
