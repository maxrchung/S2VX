﻿using osu.Framework.Utils;
using osuTK;

namespace S2VX.Game.Story.Command {

    public class CameraScaleCommand : S2VXCommand {
        public override CommandType Type { get; set; } = CommandType.CameraScale;
        public Vector2 StartValue { get; set; } = new Vector2(0.1f);
        public Vector2 EndValue { get; set; } = new Vector2(0.1f);
        public override void Apply(double time, S2VXStory story) {
            var scale = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Camera.Scale = scale;
        }
        protected override string ToValues() => $"{S2VXUtils.Vector2ToString(StartValue)}|{S2VXUtils.Vector2ToString(EndValue)}";
        public static CameraScaleCommand FromString(string[] split) {
            var command = new CameraScaleCommand() {
                StartValue = S2VXUtils.Vector2FromString(split[4]),
                EndValue = S2VXUtils.Vector2FromString(split[5]),
            };
            return command;
        }
    }
}