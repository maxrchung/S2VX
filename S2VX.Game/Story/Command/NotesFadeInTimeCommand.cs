namespace S2VX.Game.Story.Command {
    public class NotesFadeInTimeCommand : S2VXCommand {
        public float StartValue { get; set; } = 100.0f;
        public float EndValue { get; set; } = 100.0f;
        public override void Apply(double time, S2VXStory story) {
            var fadeInTime = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.FadeInTime = fadeInTime;
        }
        protected override string ToStartValue() => S2VXUtils.FloatToString(StartValue, 4);
        protected override string ToEndValue() => S2VXUtils.FloatToString(EndValue, 4);
        public static NotesFadeInTimeCommand FromString(string[] split) {
            var command = new NotesFadeInTimeCommand() {
                StartValue = S2VXUtils.StringToFloat(split[2]),
                EndValue = S2VXUtils.StringToFloat(split[4]),
            };
            return command;
        }
    }
}
