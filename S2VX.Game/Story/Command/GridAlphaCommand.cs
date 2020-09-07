using osu.Framework.Utils;
using System.Globalization;

namespace S2VX.Game.Story.Command {
    public class GridAlphaCommand : S2VXCommand {
        public override CommandType Type { get; set; } = CommandType.GridAlpha;
        public float StartValue { get; set; } = 1;
        public float EndValue { get; set; } = 1;
        public override void Apply(double time, S2VXStory story) {
            var alpha = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Grid.Alpha = alpha;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static GridAlphaCommand FromString(string[] split) {
            var command = new GridAlphaCommand() {
                StartValue = float.Parse(split[4], CultureInfo.InvariantCulture),
                EndValue = float.Parse(split[5], CultureInfo.InvariantCulture),
            };
            return command;
        }
    }
}
