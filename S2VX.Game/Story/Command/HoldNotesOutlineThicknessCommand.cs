using System.Globalization;

namespace S2VX.Game.Story.Command {
    public class HoldNotesOutlineThicknessCommand : S2VXCommand {
        public float StartValue { get; set; } = 0.01f;
        public float EndValue { get; set; } = 0.01f;
        public override void Apply(double time, S2VXStory story) {
            var value = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.GetHoldNotes().ForEach(note => note.OutlineThickness = value);
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static HoldNotesOutlineThicknessCommand FromString(string[] split) {
            var command = new HoldNotesOutlineThicknessCommand() {
                StartValue = float.Parse(split[4], CultureInfo.InvariantCulture),
                EndValue = float.Parse(split[5], CultureInfo.InvariantCulture),
            };
            return command;
        }
    }
}
