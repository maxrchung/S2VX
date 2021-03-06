﻿namespace S2VX.Game.Story.Command {
    public class ApproachesThicknessCommand : S2VXCommand {
        public float StartValue { get; set; } = 0.008f;
        public float EndValue { get; set; } = 0.008f;
        public override void Apply(double time, S2VXStory story) {
            var thickness = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Approaches.Thickness = thickness;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static ApproachesThicknessCommand FromString(string[] split) {
            var command = new ApproachesThicknessCommand() {
                StartValue = S2VXUtils.StringToFloat(split[4]),
                EndValue = S2VXUtils.StringToFloat(split[5]),
            };
            return command;
        }
    }
}
