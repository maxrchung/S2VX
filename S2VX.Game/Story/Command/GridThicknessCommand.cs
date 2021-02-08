namespace S2VX.Game.Story.Command {
    public class GridThicknessCommand : S2VXCommand {
        public float StartValue { get; set; } = 0.005f;
        public float EndValue { get; set; } = 0.005f;
        public override void Apply(double time, S2VXStory story) {
            var thickness = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Grid.Thickness = thickness;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static GridThicknessCommand FromString(string[] split) {
            var command = new GridThicknessCommand() {
                StartValue = S2VXUtils.StringToFloat(split[4]),
                EndValue = S2VXUtils.StringToFloat(split[5]),
            };
            return command;
        }
    }
}
