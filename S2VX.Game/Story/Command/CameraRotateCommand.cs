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
        protected override string ToStartValue() => S2VXUtils.FloatToString(StartValue, 4);
        protected override string ToEndValue() => S2VXUtils.FloatToString(EndValue, 4);
        public static CameraRotateCommand FromString(string[] split) {
            var command = new CameraRotateCommand() {
                StartValue = S2VXUtils.StringToFloat(split[2]),
                EndValue = S2VXUtils.StringToFloat(split[4]),
            };
            return command;
        }
    }
}
