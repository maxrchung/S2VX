﻿using osuTK;

namespace S2VX.Game.Story.Command {
    public class CameraMoveCommand : S2VXCommand {
        public Vector2 StartValue { get; set; } = Vector2.Zero;
        public Vector2 EndValue { get; set; } = Vector2.Zero;
        public override void Apply(double time, S2VXStory story) {
            var position = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Camera.TakeCameraPositionLock(this);
            story.Camera.SetPosition(this, position);
            story.Camera.ReleaseCameraPositionLock(this);
        }
        protected override string ToValues() => $"{S2VXUtils.Vector2ToString(StartValue)}|{S2VXUtils.Vector2ToString(EndValue)}";
        public static CameraMoveCommand FromString(string[] split) {
            var command = new CameraMoveCommand() {
                StartValue = S2VXUtils.StringToVector2(split[4]),
                EndValue = S2VXUtils.StringToVector2(split[5]),
            };
            return command;
        }
    }
}
