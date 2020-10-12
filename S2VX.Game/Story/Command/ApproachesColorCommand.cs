using osu.Framework.Utils;
using osuTK.Graphics;

namespace S2VX.Game.Story.Command {
    public class ApproachesColorCommand : S2VXCommand {
        public Color4 StartValue { get; set; } = Color4.Blue;
        public Color4 EndValue { get; set; } = Color4.Blue;
        public override void Apply(double time, S2VXStory story) {
            var value = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Approaches.Colour = value;
        }
        protected override string ToValues() => $"{S2VXUtils.Color4ToString(StartValue)}|{S2VXUtils.Color4ToString(EndValue)}";
        public static ApproachesColorCommand FromString(string[] split) {
            var command = new ApproachesColorCommand() {
                StartValue = S2VXUtils.Color4FromString(split[4]),
                EndValue = S2VXUtils.Color4FromString(split[5]),
            };
            return command;
        }
    }
}
