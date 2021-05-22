using osu.Framework.Allocation;
using S2VX.Game.Editor;

namespace S2VX.Game.Story.Note {
    public class EditorHoldApproach : HoldApproach {
        [Resolved]
        private EditorScreen Editor { get; set; }

        public override void UpdateApproach() {
            UpdateColor(Editor.EditorApproachRate);
            UpdatePosition();
        }

        protected override void UpdatePosition() => UpdateHoldApproachPosition(Editor.EditorApproachRate);
    }
}
