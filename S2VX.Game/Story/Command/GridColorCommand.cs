using osu.Framework.Utils;
using osuTK.Graphics;

namespace S2VX.Game.Story.Command {
    public class GridColorCommand : S2VXCommand {
        public override CommandType Type { get; set; } = CommandType.GridColor;
        public Color4 StartValue { get; set; } = Color4.White;
        public Color4 EndValue { get; set; } = Color4.White;
        public override void Apply(double time, S2VXStory story) {
            var color = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Grid.Colour = color;
        }
        protected override string ToValues() => $"{S2VXUtils.Color4ToString(StartValue)}|{S2VXUtils.Color4ToString(EndValue)}";
        public static GridColorCommand FromString(string[] split) {
            var command = new GridColorCommand() {
                StartValue = S2VXUtils.Color4FromString(split[4]),
                EndValue = S2VXUtils.Color4FromString(split[5]),
            };
            return command;
        }
    }
}
