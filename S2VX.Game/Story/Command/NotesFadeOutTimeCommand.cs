﻿using osu.Framework.Utils;
using System.Globalization;

namespace S2VX.Game.Story.Command {
    public class NotesFadeOutTimeCommand : S2VXCommand {
        public override CommandType Type { get; set; } = CommandType.NotesFadeOutTime;
        public float StartValue { get; set; } = 100.0f;
        public float EndValue { get; set; } = 100.0f;
        public override void Apply(double time, S2VXStory story) {
            var fadeOutTime = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.FadeOutTime = fadeOutTime;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static NotesFadeOutTimeCommand FromString(string[] split) {
            var command = new NotesFadeOutTimeCommand() {
                StartValue = float.Parse(split[4], CultureInfo.InvariantCulture),
                EndValue = float.Parse(split[5], CultureInfo.InvariantCulture),
            };
            return command;
        }
    }
}