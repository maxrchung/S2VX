using osu.Framework.Utils;
using System.Globalization;

namespace S2VX.Game.Story.Command {
    public class NotesAlphaCommand : S2VXCommand {
        public float StartValue { get; set; } = 0.7f;
        public float EndValue { get; set; } = 0.7f;
        public override void Apply(double time, S2VXStory story) {
            var value = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.GetNonHoldNotes().ForEach(note => note.SetAlpha(value));
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static NotesAlphaCommand FromString(string[] split) {
            var command = new NotesAlphaCommand() {
                StartValue = float.Parse(split[4], CultureInfo.InvariantCulture),
                EndValue = float.Parse(split[5], CultureInfo.InvariantCulture),
            };
            return command;
        }
    }
}
