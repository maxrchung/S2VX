using osu.Framework.Utils;
using System.Globalization;

namespace S2VX.Game.Story.Command {
    public class CameraRotateCommand : S2VXCommand {
        public float StartValue { get; set; }
        public float EndValue { get; set; }
        public override void Apply(double time, S2VXStory story) {
            var rotation = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Camera.Rotation = rotation;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static CameraRotateCommand FromString(string[] split) {
            var command = new CameraRotateCommand() {
                StartValue = float.Parse(split[4], CultureInfo.InvariantCulture),
                EndValue = float.Parse(split[5], CultureInfo.InvariantCulture),
            };
            return command;
        }
    }
}
