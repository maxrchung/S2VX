using osuTK.Graphics;

namespace S2VX.Game.Story.Command {
    public class HoldNotesOutlineColorCommand : S2VXCommand {
        public Color4 StartValue { get; set; } = S2VXColorConstants.OffWhite;
        public Color4 EndValue { get; set; } = S2VXColorConstants.OffWhite;
        public override void Apply(double time, S2VXStory story) {
            var value = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.HoldNoteOutlineColor = value;
        }
        protected override string ToStartValue() => S2VXUtils.Color4ToString(StartValue);
        protected override string ToEndValue() => S2VXUtils.Color4ToString(EndValue);
        public static HoldNotesOutlineColorCommand FromString(string[] split) {
            var command = new HoldNotesOutlineColorCommand() {
                StartValue = S2VXUtils.StringToColor4(split[2]),
                EndValue = S2VXUtils.StringToColor4(split[4]),
            };
            return command;
        }
    }
}
