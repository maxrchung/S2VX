namespace S2VX.Game.Story.Command {
    public class GridThicknessCommand : S2VXCommand {
        public float StartValue { get; set; } = 0.005f;
        public float EndValue { get; set; } = 0.005f;
        public override void Apply(double time, S2VXStory story) {
            var thickness = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Grid.Thickness = thickness;
        }
        protected override string ToStartValue() => S2VXUtils.FloatToString(StartValue, 4);
        protected override string ToEndValue() => S2VXUtils.FloatToString(EndValue, 4);
        public static GridThicknessCommand FromString(string[] split) {
            var command = new GridThicknessCommand() {
                StartValue = S2VXUtils.StringToFloat(split[2]),
                EndValue = S2VXUtils.StringToFloat(split[4]),
            };
            return command;
        }
    }
}
