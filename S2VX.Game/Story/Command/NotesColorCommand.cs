using osuTK.Graphics;

namespace S2VX.Game.Story.Command {
    public class NotesColorCommand : S2VXCommand {
        public Color4 StartValue { get; set; } = S2VXColorConstants.BrickRed;
        public Color4 EndValue { get; set; } = S2VXColorConstants.BrickRed;
        public override void Apply(double time, S2VXStory story) {
            var value = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.PreviewNoteColor = value;
            story.Notes.NoteColor = value;
        }
        protected override string ToValues() => $"{S2VXUtils.Color4ToString(StartValue)}|{S2VXUtils.Color4ToString(EndValue)}";
        public static NotesColorCommand FromString(string[] split) {
            var command = new NotesColorCommand() {
                StartValue = S2VXUtils.StringToColor4(split[4]),
                EndValue = S2VXUtils.StringToColor4(split[5]),
            };
            return command;
        }
    }
}
