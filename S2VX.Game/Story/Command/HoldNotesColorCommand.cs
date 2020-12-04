using osu.Framework.Utils;
using osuTK.Graphics;

namespace S2VX.Game.Story.Command {
    public class HoldNotesColorCommand : S2VXCommand {
        public Color4 StartValue { get; set; } = Color4.White;
        public Color4 EndValue { get; set; } = Color4.White;
        public override void Apply(double time, S2VXStory story) {
            var value = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.GetHoldNotes().ForEach(note => note.SetColor(value));
            story.Notes.PreviewHoldNoteColor = value;
        }
        protected override string ToValues() => $"{S2VXUtils.Color4ToString(StartValue)}|{S2VXUtils.Color4ToString(EndValue)}";
        public static HoldNotesColorCommand FromString(string[] split) {
            var command = new HoldNotesColorCommand() {
                StartValue = S2VXUtils.Color4FromString(split[4]),
                EndValue = S2VXUtils.Color4FromString(split[5]),
            };
            return command;
        }
    }
}
