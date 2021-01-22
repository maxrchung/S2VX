using osuTK.Graphics;

namespace S2VX.Game.Story.Command {
    public class BackgroundColorCommand : S2VXCommand {
        public Color4 StartValue { get; set; } = S2VXColorConstants.DarkBlack;
        public Color4 EndValue { get; set; } = S2VXColorConstants.DarkBlack;
        public override void Apply(double time, S2VXStory story) {
            var color = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Background.Colour = color;
        }
        protected override string ToValues() => $"{S2VXUtils.Color4ToString(StartValue)}|{S2VXUtils.Color4ToString(EndValue)}";
        public static BackgroundColorCommand FromString(string[] split) {
            var command = new BackgroundColorCommand() {
                StartValue = S2VXUtils.StringToColor4(split[4]),
                EndValue = S2VXUtils.StringToColor4(split[5]),
            };
            return command;
        }
    }
}
