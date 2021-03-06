﻿using osuTK.Graphics;

namespace S2VX.Game.Story.Command {
    public class GridColorCommand : S2VXCommand {
        public Color4 StartValue { get; set; } = S2VXColorConstants.LightBlack;
        public Color4 EndValue { get; set; } = S2VXColorConstants.LightBlack;
        public override void Apply(double time, S2VXStory story) {
            var color = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Grid.Colour = color;
        }
        protected override string ToValues() => $"{S2VXUtils.Color4ToString(StartValue)}|{S2VXUtils.Color4ToString(EndValue)}";
        public static GridColorCommand FromString(string[] split) {
            var command = new GridColorCommand() {
                StartValue = S2VXUtils.StringToColor4(split[4]),
                EndValue = S2VXUtils.StringToColor4(split[5]),
            };
            return command;
        }
    }
}
