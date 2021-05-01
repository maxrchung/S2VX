
namespace S2VX.Game.Story.Note {
    public class GameApproach : Approach {

        public override void UpdateApproach() {
            UpdateColor();
            UpdatePosition();
        }

        protected override void UpdatePosition() => UpdateInnerApproachPosition(Coordinates);
    }
}
