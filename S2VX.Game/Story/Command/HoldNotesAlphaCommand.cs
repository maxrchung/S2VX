namespace S2VX.Game.Story.Command {
    public class HoldNotesAlphaCommand : S2VXCommand {
        public float StartValue { get; set; } = 1;
        public float EndValue { get; set; } = 1;
        public override void Apply(double time, S2VXStory story) {
            var value = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.GetHoldNotes().ForEach(note => note.SetAlpha(value));
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static HoldNotesAlphaCommand FromString(string[] split) {
            var command = new HoldNotesAlphaCommand() {
                StartValue = S2VXUtils.StringToFloat(split[4]),
                EndValue = S2VXUtils.StringToFloat(split[5]),
            };
            return command;
        }
    }
}
