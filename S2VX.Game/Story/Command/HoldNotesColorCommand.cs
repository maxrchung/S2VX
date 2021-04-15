using osuTK.Graphics;

namespace S2VX.Game.Story.Command {
    public class HoldNotesColorCommand : S2VXCommand {
        public Color4 StartValue { get; set; } = S2VXColorConstants.Cyan;
        public Color4 EndValue { get; set; } = S2VXColorConstants.Cyan;
        public override void Apply(double time, S2VXStory story) {
            var value = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.PreviewHoldNoteColor = value;
            story.Notes.HoldNoteColor = value;
        }
        protected override string ToValues() => $"{S2VXUtils.Color4ToString(StartValue)}|{S2VXUtils.Color4ToString(EndValue)}";
        public static HoldNotesColorCommand FromString(string[] split) {
            var command = new HoldNotesColorCommand() {
                StartValue = S2VXUtils.StringToColor4(split[4]),
                EndValue = S2VXUtils.StringToColor4(split[5]),
            };
            return command;
        }
    }
}
