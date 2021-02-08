namespace S2VX.Game.Story.Command {
    public class ApproachesDistanceCommand : S2VXCommand {
        public float StartValue { get; set; } = 0.5f;
        public float EndValue { get; set; } = 0.5f;
        public override void Apply(double time, S2VXStory story) {
            var distance = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Approaches.Distance = distance;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static ApproachesDistanceCommand FromString(string[] split) {
            var command = new ApproachesDistanceCommand() {
                StartValue = S2VXUtils.StringToFloat(split[4]),
                EndValue = S2VXUtils.StringToFloat(split[5]),
            };
            return command;
        }
    }
}
