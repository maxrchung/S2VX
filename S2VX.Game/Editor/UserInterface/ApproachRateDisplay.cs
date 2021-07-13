using osu.Framework.Allocation;
using osu.Framework.Input.Events;

namespace S2VX.Game.Editor.UserInterface {
    public class ApproachRateDisplay : EditorInfoDisplay {
        [Resolved]
        private EditorScreen Editor { get; set; }

        public override void UpdateDisplay() => UpdateDisplay($"Approach Rate: {Editor.EditorApproachRate}");

        protected override bool OnScroll(ScrollEvent e) {
            if (e.ScrollDelta.Y > 0) {
                Editor.ApproachRateIncrease();
            } else {
                Editor.ApproachRateDecrease();
            }
            UpdateDisplay();
            return true;
        }
    }
}
