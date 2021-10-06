namespace S2VX.Game.Story.Command {
    public class NotesFadeOutTimeCommand : S2VXCommand {
        public float StartValue { get; set; } = 100.0f;
        public float EndValue { get; set; } = 100.0f;
        public override void Apply(double time, S2VXStory story) {
            var fadeOutTime = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.FadeOutTime = fadeOutTime;
        }
        protected override string ToStartValue() => $"{StartValue}";
        protected override string ToEndValue() => $"{EndValue}";
        public static NotesFadeOutTimeCommand FromString(string[] split) {
            var command = new NotesFadeOutTimeCommand() {
                StartValue = S2VXUtils.StringToFloat(split[2]),
                EndValue = S2VXUtils.StringToFloat(split[4]),
            };
            return command;
        }
    }
}
