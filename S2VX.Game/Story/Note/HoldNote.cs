using osuTK;

namespace S2VX.Game.Story.Note {
    public abstract class HoldNote : S2VXNote {
        public double EndTime { get; set; }
        public Approach ReleaseApproach { get; set; }

        public override void UpdateHitTime(double hitTime) {
            base.UpdateHitTime(hitTime);
            EndTime = hitTime + 1000;   // TODO: change this
        }

        public override void UpdateCoordinates(Vector2 coordinates) {
            Approach.Coordinates = coordinates;
            ReleaseApproach.Coordinates = coordinates;
            Coordinates = coordinates;
        }
    }
}
