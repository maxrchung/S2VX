namespace S2VX.Game.Story.Command {
    public class NotesShowTimeCommand : S2VXCommand {
        public float StartValue { get; set; } = 1000.0f;
        public float EndValue { get; set; } = 1000.0f;
        public override void Apply(double time, S2VXStory story) {
            var showTime = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.ShowTime = showTime;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static NotesShowTimeCommand FromString(string[] split) {
            var command = new NotesShowTimeCommand() {
                StartValue = S2VXUtils.StringToFloat(split[4]),
                EndValue = S2VXUtils.StringToFloat(split[5]),
            };
            return command;
        }
    }
}
