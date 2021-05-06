
namespace S2VX.Game.Story.Note {
    public class GameHoldApproach : HoldApproach {

        public override void UpdateApproach() {
            UpdateColor();
            UpdatePosition();
        }

        protected override void UpdatePosition() => UpdateHoldApproachPosition();
    }
}
