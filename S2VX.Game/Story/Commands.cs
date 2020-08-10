﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Graphics;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;
using System;

namespace S2VX.Game.Story {
    public enum Commands {
        None,
        ApproachesDistance,
        ApproachesThickness,
        BackgroundColor,
        CameraMove,
        CameraRotate,
        CameraScale,
        GridAlpha,
        GridColor,
        GridThickness,
        NotesAlpha,
        NotesColor,
        NotesFadeInTime,
        NotesFadeOutTime,
        NotesShowTime,
        TimingChange,
    }

    public abstract class Command : IComparable<Command> {
        public abstract Commands Type { get; set; }
        public double StartTime { get; set; } = 0;
        public double EndTime { get; set; } = 0;
        public Easing Easing { get; set; } = Easing.None;
        public abstract void Apply(double time, S2VXStory story);

        public int CompareTo(Command other) => StartTime.CompareTo(other.StartTime);

        protected abstract string ToValues();
        public override string ToString() => $"{Type}|{StartTime}|{EndTime}|{Easing}|{ToValues()}";

        public static Command FromString(string data) {
            var split = data.Split("|");
            var type = Enum.Parse<Commands>(split[0]);
            Command command = null;
            switch (type) {
                case Commands.CameraMove:
                    command = CameraMoveCommand.FromString(split);
                    break;
                case Commands.CameraRotate:
                    command = CameraRotateCommand.FromString(split);
                    break;
                case Commands.CameraScale:
                    command = CameraScaleCommand.FromString(split);
                    break;
                case Commands.GridAlpha:
                    command = GridAlphaCommand.FromString(split);
                    break;
                case Commands.GridColor:
                    command = GridColorCommand.FromString(split);
                    break;
                case Commands.GridThickness:
                    command = GridThicknessCommand.FromString(split);
                    break;
                case Commands.BackgroundColor:
                    command = BackgroundColorCommand.FromString(split);
                    break;
                case Commands.NotesAlpha:
                    command = NotesAlphaCommand.FromString(split);
                    break;
                case Commands.NotesColor:
                    command = NotesColorCommand.FromString(split);
                    break;
                case Commands.NotesFadeInTime:
                    command = NotesFadeInTimeCommand.FromString(split);
                    break;
                case Commands.NotesShowTime:
                    command = NotesShowTimeCommand.FromString(split);
                    break;
                case Commands.NotesFadeOutTime:
                    command = NotesFadeOutTimeCommand.FromString(split);
                    break;
                case Commands.ApproachesDistance:
                    command = ApproachesDistanceCommand.FromString(split);
                    break;
                case Commands.ApproachesThickness:
                    command = ApproachesThicknessCommand.FromString(split);
                    break;
                case Commands.TimingChange:
                    command = TimingChangeCommand.FromString(split);
                    break;
            }
            command.Type = Enum.Parse<Commands>(split[0]);
            command.StartTime = double.Parse(split[1]);
            command.EndTime = double.Parse(split[2]);
            command.Easing = Enum.Parse<Easing>(split[3]);
            return command;
        }

        public static Command FromJson(JObject json) {
            var type = Enum.Parse<Commands>(json["Type"].ToString());
            var data = json.ToString();
            Command command = null;
            switch (type) {
                case Commands.CameraMove:
                    command = JsonConvert.DeserializeObject<CameraMoveCommand>(data);
                    break;
                case Commands.CameraRotate:
                    command = JsonConvert.DeserializeObject<CameraRotateCommand>(data);
                    break;
                case Commands.CameraScale:
                    command = JsonConvert.DeserializeObject<CameraScaleCommand>(data);
                    break;
                case Commands.GridAlpha:
                    command = JsonConvert.DeserializeObject<GridAlphaCommand>(data);
                    break;
                case Commands.GridColor:
                    command = JsonConvert.DeserializeObject<GridColorCommand>(data);
                    break;
                case Commands.GridThickness:
                    command = JsonConvert.DeserializeObject<GridThicknessCommand>(data);
                    break;
                case Commands.BackgroundColor:
                    command = JsonConvert.DeserializeObject<BackgroundColorCommand>(data);
                    break;
                case Commands.NotesAlpha:
                    command = JsonConvert.DeserializeObject<NotesAlphaCommand>(data);
                    break;
                case Commands.NotesColor:
                    command = JsonConvert.DeserializeObject<NotesColorCommand>(data);
                    break;
                case Commands.NotesFadeInTime:
                    command = JsonConvert.DeserializeObject<NotesFadeInTimeCommand>(data);
                    break;
                case Commands.NotesShowTime:
                    command = JsonConvert.DeserializeObject<NotesShowTimeCommand>(data);
                    break;
                case Commands.NotesFadeOutTime:
                    command = JsonConvert.DeserializeObject<NotesFadeOutTimeCommand>(data);
                    break;
                case Commands.ApproachesDistance:
                    command = JsonConvert.DeserializeObject<ApproachesDistanceCommand>(data);
                    break;
                case Commands.ApproachesThickness:
                    command = JsonConvert.DeserializeObject<ApproachesThicknessCommand>(data);
                    break;
                case Commands.TimingChange:
                    command = JsonConvert.DeserializeObject<TimingChangeCommand>(data);
                    break;
            }
            return command;
        }
    }

    public class CameraMoveCommand : Command {
        public override Commands Type { get; set; } = Commands.CameraMove;
        public Vector2 StartValue { get; set; } = Vector2.Zero;
        public Vector2 EndValue { get; set; } = Vector2.Zero;
        public override void Apply(double time, S2VXStory story) {
            var position = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Camera.Position = position;
        }
        protected override string ToValues() => $"{Utils.Vector2ToString(StartValue)}|{Utils.Vector2ToString(EndValue)}";
        public static CameraMoveCommand FromString(string[] split) {
            var command = new CameraMoveCommand() {
                StartValue = Utils.Vector2FromString(split[4]),
                EndValue = Utils.Vector2FromString(split[5]),
            };
            return command;
        }
    }

    public class CameraRotateCommand : Command {
        public override Commands Type { get; set; } = Commands.CameraRotate;
        public float StartValue { get; set; } = 0;
        public float EndValue { get; set; } = 0;
        public override void Apply(double time, S2VXStory story) {
            var rotation = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Camera.Rotation = rotation;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static CameraRotateCommand FromString(string[] split) {
            var command = new CameraRotateCommand() {
                StartValue = float.Parse(split[4]),
                EndValue = float.Parse(split[5]),
            };
            return command;
        }
    }

    public class CameraScaleCommand : Command {
        public override Commands Type { get; set; } = Commands.CameraScale;
        public Vector2 StartValue { get; set; } = new Vector2(0.1f);
        public Vector2 EndValue { get; set; } = new Vector2(0.1f);
        public override void Apply(double time, S2VXStory story) {
            var scale = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Camera.Scale = scale;
        }
        protected override string ToValues() => $"{Utils.Vector2ToString(StartValue)}|{Utils.Vector2ToString(EndValue)}";
        public static CameraScaleCommand FromString(string[] split) {
            var command = new CameraScaleCommand() {
                StartValue = Utils.Vector2FromString(split[4]),
                EndValue = Utils.Vector2FromString(split[5]),
            };
            return command;
        }
    }

    public abstract class GridCommand : Command {
        public Grid Grid = new Grid();
    }

    public class GridAlphaCommand : Command {
        public override Commands Type { get; set; } = Commands.GridAlpha;
        public float StartValue { get; set; } = 1;
        public float EndValue { get; set; } = 1;
        public override void Apply(double time, S2VXStory story) {
            var alpha = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Grid.Alpha = alpha;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static GridAlphaCommand FromString(string[] split) {
            var command = new GridAlphaCommand() {
                StartValue = float.Parse(split[4]),
                EndValue = float.Parse(split[5]),
            };
            return command;
        }
    }

    public class GridColorCommand : Command {
        public override Commands Type { get; set; } = Commands.GridColor;
        public Color4 StartValue { get; set; } = Color4.White;
        public Color4 EndValue { get; set; } = Color4.White;
        public override void Apply(double time, S2VXStory story) {
            var color = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Grid.Colour = color;
        }
        protected override string ToValues() => $"{Utils.Color4ToString(StartValue)}|{Utils.Color4ToString(EndValue)}";
        public static GridColorCommand FromString(string[] split) {
            var command = new GridColorCommand() {
                StartValue = Utils.Color4FromString(split[4]),
                EndValue = Utils.Color4FromString(split[5]),
            };
            return command;
        }
    }

    public class GridThicknessCommand : Command {
        public override Commands Type { get; set; } = Commands.GridThickness;
        public float StartValue { get; set; } = 0.005f;
        public float EndValue { get; set; } = 0.005f;
        public override void Apply(double time, S2VXStory story) {
            var thickness = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Grid.Thickness = thickness;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static GridThicknessCommand FromString(string[] split) {
            var command = new GridThicknessCommand() {
                StartValue = float.Parse(split[4]),
                EndValue = float.Parse(split[5]),
            };
            return command;
        }
    }

    public class BackgroundColorCommand : Command {
        public override Commands Type { get; set; } = Commands.BackgroundColor;
        public Color4 StartValue { get; set; } = Color4.White;
        public Color4 EndValue { get; set; } = Color4.White;
        public override void Apply(double time, S2VXStory story) {
            var color = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Background.Colour = color;
        }
        protected override string ToValues() => $"{Utils.Color4ToString(StartValue)}|{Utils.Color4ToString(EndValue)}";
        public static BackgroundColorCommand FromString(string[] split) {
            var command = new BackgroundColorCommand() {
                StartValue = Utils.Color4FromString(split[4]),
                EndValue = Utils.Color4FromString(split[5]),
            };
            return command;
        }
    }

    public class NotesFadeInTimeCommand : Command {
        public override Commands Type { get; set; } = Commands.NotesFadeInTime;
        public float StartValue { get; set; } = 100.0f;
        public float EndValue { get; set; } = 100.0f;
        public override void Apply(double time, S2VXStory story) {
            var fadeInTime = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.FadeInTime = fadeInTime;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static NotesFadeInTimeCommand FromString(string[] split) {
            var command = new NotesFadeInTimeCommand() {
                StartValue = float.Parse(split[4]),
                EndValue = float.Parse(split[5]),
            };
            return command;
        }
    }

    public class NotesShowTimeCommand : Command {
        public override Commands Type { get; set; } = Commands.NotesShowTime;
        public float StartValue { get; set; } = 100.0f;
        public float EndValue { get; set; } = 100.0f;
        public override void Apply(double time, S2VXStory story) {
            var showTime = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.ShowTime = showTime;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static NotesShowTimeCommand FromString(string[] split) {
            var command = new NotesShowTimeCommand() {
                StartValue = float.Parse(split[4]),
                EndValue = float.Parse(split[5]),
            };
            return command;
        }
    }

    public class NotesFadeOutTimeCommand : Command {
        public override Commands Type { get; set; } = Commands.NotesFadeOutTime;
        public float StartValue { get; set; } = 100.0f;
        public float EndValue { get; set; } = 100.0f;
        public override void Apply(double time, S2VXStory story) {
            var fadeOutTime = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.FadeOutTime = fadeOutTime;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static NotesFadeOutTimeCommand FromString(string[] split) {
            var command = new NotesFadeOutTimeCommand() {
                StartValue = float.Parse(split[4]),
                EndValue = float.Parse(split[5]),
            };
            return command;
        }
    }

    public class NotesAlphaCommand : Command {
        public override Commands Type { get; set; } = Commands.NotesAlpha;
        public float StartValue { get; set; } = 1;
        public float EndValue { get; set; } = 1;
        public override void Apply(double time, S2VXStory story) {
            var alpha = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.Alpha = alpha;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static NotesAlphaCommand FromString(string[] split) {
            var command = new NotesAlphaCommand() {
                StartValue = float.Parse(split[4]),
                EndValue = float.Parse(split[5]),
            };
            return command;
        }
    }
    public class NotesColorCommand : Command {
        public override Commands Type { get; set; } = Commands.NotesColor;
        public Color4 StartValue { get; set; } = Color4.White;
        public Color4 EndValue { get; set; } = Color4.White;
        public override void Apply(double time, S2VXStory story) {
            var color = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.Colour = color;
        }
        protected override string ToValues() => $"{Utils.Color4ToString(StartValue)}|{Utils.Color4ToString(EndValue)}";
        public static NotesColorCommand FromString(string[] split) {
            var command = new NotesColorCommand() {
                StartValue = Utils.Color4FromString(split[4]),
                EndValue = Utils.Color4FromString(split[5]),
            };
            return command;
        }
    }

    public class ApproachesDistanceCommand : Command {
        public override Commands Type { get; set; } = Commands.ApproachesDistance;
        public float StartValue { get; set; } = 0.5f;
        public float EndValue { get; set; } = 0.5f;
        public override void Apply(double time, S2VXStory story) {
            var distance = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Approaches.Distance = distance;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static ApproachesDistanceCommand FromString(string[] split) {
            var command = new ApproachesDistanceCommand() {
                StartValue = float.Parse(split[4]),
                EndValue = float.Parse(split[5]),
            };
            return command;
        }
    }

    public class ApproachesThicknessCommand : Command {
        public override Commands Type { get; set; } = Commands.ApproachesThickness;
        public float StartValue { get; set; } = 0.005f;
        public float EndValue { get; set; } = 0.005f;
        public override void Apply(double time, S2VXStory story) {
            var thickness = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Approaches.Thickness = thickness;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static ApproachesThicknessCommand FromString(string[] split) {
            var command = new ApproachesThicknessCommand() {
                StartValue = float.Parse(split[4]),
                EndValue = float.Parse(split[5]),
            };
            return command;
        }
    }

    public class TimingChangeCommand : Command {
        public override Commands Type { get; set; } = Commands.TimingChange;
        public float StartValue { get; set; } = 0;
        public float EndValue { get; set; } = 0;
        public override void Apply(double time, S2VXStory story) {
            var bpm = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.BPM = bpm;
            story.Offset = StartTime;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static TimingChangeCommand FromString(string[] split) {
            var command = new TimingChangeCommand() {
                StartValue = float.Parse(split[4]),
                EndValue = float.Parse(split[5]),
            };
            return command;
        }
    }
}
