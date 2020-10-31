namespace S2VX.Game.Story.Note {
    public abstract class HoldNote : S2VXNote {
        public double EndTime { get; set; }

        public override void UpdateHitTime(double hitTime) {
            base.UpdateHitTime(hitTime);
            EndTime = hitTime + 1000;    // TODO: #216 be able to change hold duration
            ((HoldApproach)Approach).EndTime = EndTime;
        }
    }
}
