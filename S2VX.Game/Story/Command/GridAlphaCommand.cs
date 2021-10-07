namespace S2VX.Game.Story.Command {
    public class GridAlphaCommand : S2VXCommand {
        public float StartValue { get; set; } = 1;
        public float EndValue { get; set; } = 1;
        public override void Apply(double time, S2VXStory story) {
            var alpha = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Grid.Alpha = alpha;
        }
        protected override string ToStartValue() => S2VXUtils.FloatToString(StartValue, 4);
        protected override string ToEndValue() => S2VXUtils.FloatToString(EndValue, 4);
        public static GridAlphaCommand FromString(string[] split) {
            var command = new GridAlphaCommand() {
                StartValue = S2VXUtils.StringToFloat(split[2]),
                EndValue = S2VXUtils.StringToFloat(split[4]),
            };
            return command;
        }
    }
}
