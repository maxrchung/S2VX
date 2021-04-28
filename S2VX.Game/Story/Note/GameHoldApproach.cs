using osu.Framework.Allocation;

namespace S2VX.Game.Story.Note {
    public class GameHoldApproach : HoldApproach {
        [Resolved]
        private S2VXStory Story { get; set; }

        public override void UpdateApproach() {
            UpdateColor(Story.Notes.FadeInTime);
            UpdatePosition();
        }

        protected override void UpdatePosition() => UpdateHoldApproachPosition(Story.Notes.FadeInTime);
    }
}
