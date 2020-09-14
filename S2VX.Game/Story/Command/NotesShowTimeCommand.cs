using osu.Framework.Utils;
using System.Globalization;

namespace S2VX.Game.Story.Command {
    public class NotesShowTimeCommand : S2VXCommand {
        public float StartValue { get; set; } = 100.0f;
        public float EndValue { get; set; } = 100.0f;
        public override void Apply(double time, S2VXStory story) {
            var showTime = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.ShowTime = showTime;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static NotesShowTimeCommand FromString(string[] split) {
            var command = new NotesShowTimeCommand() {
                StartValue = float.Parse(split[4], CultureInfo.InvariantCulture),
                EndValue = float.Parse(split[5], CultureInfo.InvariantCulture),
            };
            return command;
        }
    }
}
