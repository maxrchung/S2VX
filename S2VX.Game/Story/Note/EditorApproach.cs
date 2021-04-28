using osu.Framework.Allocation;
using S2VX.Game.Editor;

namespace S2VX.Game.Story.Note {
    public class EditorApproach : Approach {
        [Resolved]
        private EditorScreen Editor { get; set; }

        public override void UpdateApproach() {
            UpdateColor(Editor.EditorFadeInTime);
            UpdatePosition();
        }

        protected override void UpdatePosition() => UpdateInnerApproachPosition(Coordinates, Editor.EditorFadeInTime);
    }
}
