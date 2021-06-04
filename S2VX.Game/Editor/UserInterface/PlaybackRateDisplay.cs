using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using System.Globalization;

namespace S2VX.Game.Editor.UserInterface {
    public class PlaybackRateDisplay : EditorInfoDisplay {
        [Resolved]
        private EditorScreen Editor { get; set; }

        public override void UpdateDisplay() => UpdateDisplay($"Speed: {Editor.Track.Rate.ToString("P0", CultureInfo.InvariantCulture)}");

        protected override bool OnScroll(ScrollEvent e) {
            if (e.ScrollDelta.Y > 0) {
                Editor.PlaybackIncreaseRate();
            } else {
                Editor.PlaybackDecreaseRate();
            }
            UpdateDisplay();
            return true;
        }
    }
}
