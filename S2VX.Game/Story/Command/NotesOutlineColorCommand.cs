using osuTK.Graphics;

namespace S2VX.Game.Story.Command {
    public class NotesOutlineColorCommand : S2VXCommand {
        public Color4 StartValue { get; set; } = S2VXColorConstants.OffWhite;
        public Color4 EndValue { get; set; } = S2VXColorConstants.OffWhite;
        public override void Apply(double time, S2VXStory story) {
            var value = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.GetNonHoldNotes().ForEach(note => note.SetOutlineColor(value));
        }
        protected override string ToValues() => $"{S2VXUtils.Color4ToString(StartValue)}|{S2VXUtils.Color4ToString(EndValue)}";
        public static NotesOutlineColorCommand FromString(string[] split) {
            var command = new NotesOutlineColorCommand() {
                StartValue = S2VXUtils.Color4FromString(split[4]),
                EndValue = S2VXUtils.Color4FromString(split[5]),
            };
            return command;
        }
    }
}
