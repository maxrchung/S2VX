﻿using System.Globalization;

namespace S2VX.Game.Story.Command {
    public class CameraRotateCommand : S2VXCommand {
        public float StartValue { get; set; }
        public float EndValue { get; set; }
        public override void Apply(double time, S2VXStory story) {
            var rotation = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Camera.TakeCameraRotationLock(this);
            story.Camera.SetRotation(this, rotation);
            story.Camera.ReleaseCameraRotationLock(this);
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
