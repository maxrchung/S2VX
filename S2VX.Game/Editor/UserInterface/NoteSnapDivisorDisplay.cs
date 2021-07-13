using osu.Framework.Allocation;
using osu.Framework.Input.Events;

namespace S2VX.Game.Editor.UserInterface {
    public class NoteSnapDivisorDisplay : EditorInfoDisplay {
        [Resolved]
        private EditorScreen Editor { get; set; }

        public override void UpdateDisplay() => UpdateDisplay(
            Editor.SnapDivisor == 0 ? "Snap Divisor: Free" : "Snap Divisor: " + S2VXUtils.FloatToString(1.0f / Editor.SnapDivisor));

        protected override bool OnScroll(ScrollEvent e) {
            if (e.ScrollDelta.Y > 0) {
                Editor.SnapDivisorIncrease();
            } else {
                Editor.SnapDivisorDecrease();
            }
            UpdateDisplay();
            return true;
        }
    }
}
