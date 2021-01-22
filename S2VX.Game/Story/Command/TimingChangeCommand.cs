namespace S2VX.Game.Story.Command {
    public class TimingChangeCommand : S2VXCommand {
        public float StartValue { get; set; } = 100.0f;
        public float EndValue { get; set; } = 100.0f;
        public override void Apply(double time, S2VXStory story) {
            var bpm = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.BPM = bpm;
            story.Offset = StartTime;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static TimingChangeCommand FromString(string[] split) {
            var command = new TimingChangeCommand() {
                StartValue = S2VXUtils.StringToFloat(split[4]),
                EndValue = S2VXUtils.StringToFloat(split[5]),
            };
            return command;
        }
    }
}
