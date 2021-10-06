﻿namespace S2VX.Game.Story.Command {
    public class NotesOutlineThicknessCommand : S2VXCommand {
        public float StartValue { get; set; } = 0.01f;
        public float EndValue { get; set; } = 0.01f;
        public override void Apply(double time, S2VXStory story) {
            var value = S2VXUtils.ClampedInterpolation(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.NoteOutlineThickness = value;
        }
        protected override string ToStartValue() => $"{StartValue}";
        protected override string ToEndValue() => $"{EndValue}";
        public static NotesOutlineThicknessCommand FromString(string[] split) {
            var command = new NotesOutlineThicknessCommand() {
                StartValue = S2VXUtils.StringToFloat(split[2]),
                EndValue = S2VXUtils.StringToFloat(split[4]),
            };
            return command;
        }
    }
}
