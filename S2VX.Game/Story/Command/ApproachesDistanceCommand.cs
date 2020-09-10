using osu.Framework.Utils;
using System.Globalization;

namespace S2VX.Game.Story.Command {
    public class ApproachesDistanceCommand : S2VXCommand {
        public float StartValue { get; set; } = 0.5f;
        public float EndValue { get; set; } = 0.5f;
        public override void Apply(double time, S2VXStory story) {
            var distance = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Approaches.Distance = distance;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static ApproachesDistanceCommand FromString(string[] split) {
            var command = new ApproachesDistanceCommand() {
                StartValue = float.Parse(split[4], CultureInfo.InvariantCulture),
                EndValue = float.Parse(split[5], CultureInfo.InvariantCulture),
            };
            return command;
        }
    }
}
