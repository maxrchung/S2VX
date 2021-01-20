namespace S2VX.Game.Story.Command {
    public class GridAlphaCommand : S2VXCommand {
        public float StartValue { get; set; } = 1;
        public float EndValue { get; set; } = 1;
        public override void Apply(double time, S2VXStory story) {
            var alpha = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Grid.Alpha = alpha;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static GridAlphaCommand FromString(string[] split) {
            var command = new GridAlphaCommand() {
                StartValue = S2VXUtils.StringToFloat(split[4]),
                EndValue = S2VXUtils.StringToFloat(split[5]),
            };
            return command;
        }
    }
}
