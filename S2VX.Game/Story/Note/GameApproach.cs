using osu.Framework.Allocation;

namespace S2VX.Game.Story.Note {
    public class GameApproach : Approach {
        [Resolved]
        private S2VXStory Story { get; set; }

        public override void UpdateApproach() {
            UpdateColor(Story.Notes.FadeInTime);
            UpdatePosition();
        }

        protected override void UpdatePosition() => UpdateInnerApproachPosition(Coordinates, Story.Notes.FadeInTime);
    }
}
