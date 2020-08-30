using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Graphics;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;
using System;
using System.Globalization;

namespace S2VX.Game.Story {
    public enum CommandType {
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
        public abstract CommandType Type { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public Easing Easing { get; set; } = Easing.None;
        public abstract void Apply(double time, S2VXStory story);

        public int CompareTo(Command other) => StartTime.CompareTo(other.StartTime);

        protected abstract string ToValues();
        public override string ToString() => $"{Type}|{StartTime}|{EndTime}|{Easing}|{ToValues()}";

        public static Command FromString(string data) {
            var split = data.Split("|");
            var type = Enum.Parse<CommandType>(split[0]);
            Command command = null;
            switch (type) {
                case CommandType.CameraMove:
                    command = CameraMoveCommand.FromString(split);
                    break;
                case CommandType.CameraRotate:
                    command = CameraRotateCommand.FromString(split);
                    break;
                case CommandType.CameraScale:
                    command = CameraScaleCommand.FromString(split);
                    break;
                case CommandType.GridAlpha:
                    command = GridAlphaCommand.FromString(split);
                    break;
                case CommandType.GridColor:
                    command = GridColorCommand.FromString(split);
                    break;
                case CommandType.GridThickness:
                    command = GridThicknessCommand.FromString(split);
                    break;
                case CommandType.BackgroundColor:
                    command = BackgroundColorCommand.FromString(split);
                    break;
                case CommandType.NotesAlpha:
                    command = NotesAlphaCommand.FromString(split);
                    break;
                case CommandType.NotesColor:
                    command = NotesColorCommand.FromString(split);
                    break;
                case CommandType.NotesFadeInTime:
                    command = NotesFadeInTimeCommand.FromString(split);
                    break;
                case CommandType.NotesShowTime:
                    command = NotesShowTimeCommand.FromString(split);
                    break;
                case CommandType.NotesFadeOutTime:
                    command = NotesFadeOutTimeCommand.FromString(split);
                    break;
                case CommandType.ApproachesDistance:
                    command = ApproachesDistanceCommand.FromString(split);
                    break;
                case CommandType.ApproachesThickness:
                    command = ApproachesThicknessCommand.FromString(split);
                    break;
                case CommandType.TimingChange:
                    command = TimingChangeCommand.FromString(split);
                    break;
                case CommandType.None:
                    break;
                default:
                    break;
            }
            command.Type = Enum.Parse<CommandType>(split[0]);
            command.StartTime = double.Parse(split[1], CultureInfo.InvariantCulture);
            command.EndTime = double.Parse(split[2], CultureInfo.InvariantCulture);
            command.Easing = Enum.Parse<Easing>(split[3]);
            return command;
        }

        public static Command FromJson(JObject json) {
            var type = Enum.Parse<CommandType>(json[nameof(Type)].ToString());
            var data = json.ToString();
            Command command = null;
            switch (type) {
                case CommandType.CameraMove:
                    command = JsonConvert.DeserializeObject<CameraMoveCommand>(data);
                    break;
                case CommandType.CameraRotate:
                    command = JsonConvert.DeserializeObject<CameraRotateCommand>(data);
                    break;
                case CommandType.CameraScale:
                    command = JsonConvert.DeserializeObject<CameraScaleCommand>(data);
                    break;
                case CommandType.GridAlpha:
                    command = JsonConvert.DeserializeObject<GridAlphaCommand>(data);
                    break;
                case CommandType.GridColor:
                    command = JsonConvert.DeserializeObject<GridColorCommand>(data);
                    break;
                case CommandType.GridThickness:
                    command = JsonConvert.DeserializeObject<GridThicknessCommand>(data);
                    break;
                case CommandType.BackgroundColor:
                    command = JsonConvert.DeserializeObject<BackgroundColorCommand>(data);
                    break;
                case CommandType.NotesAlpha:
                    command = JsonConvert.DeserializeObject<NotesAlphaCommand>(data);
                    break;
                case CommandType.NotesColor:
                    command = JsonConvert.DeserializeObject<NotesColorCommand>(data);
                    break;
                case CommandType.NotesFadeInTime:
                    command = JsonConvert.DeserializeObject<NotesFadeInTimeCommand>(data);
                    break;
                case CommandType.NotesShowTime:
                    command = JsonConvert.DeserializeObject<NotesShowTimeCommand>(data);
                    break;
                case CommandType.NotesFadeOutTime:
                    command = JsonConvert.DeserializeObject<NotesFadeOutTimeCommand>(data);
                    break;
                case CommandType.ApproachesDistance:
                    command = JsonConvert.DeserializeObject<ApproachDistanceCommand>(data);
                    break;
                case CommandType.ApproachesThickness:
                    command = JsonConvert.DeserializeObject<ApproachThicknessCommand>(data);
                    break;
                case CommandType.TimingChange:
                    command = JsonConvert.DeserializeObject<TimingChangeCommand>(data);
                    break;
                case CommandType.None:
                    break;
                default:
                    break;
            }
            return command;
        }

        public override bool Equals(object obj) => ReferenceEquals(this, obj);

        public override int GetHashCode() => ToString().GetHashCode(StringComparison.Ordinal);

        public static bool operator ==(Command left, Command right) => left is null ? right is null : left.Equals(right);

        public static bool operator !=(Command left, Command right) => !(left == right);

        public static bool operator <(Command left, Command right) => left is null ? right is null : left.CompareTo(right) < 0;

        public static bool operator <=(Command left, Command right) => left is null || left.CompareTo(right) <= 0;

        public static bool operator >(Command left, Command right) => left is object && left.CompareTo(right) > 0;

        public static bool operator >=(Command left, Command right) => left is null ? right is null : left.CompareTo(right) >= 0;
    }

    public class CameraMoveCommand : Command {
        public override CommandType Type { get; set; } = CommandType.CameraMove;
        public Vector2 StartValue { get; set; } = Vector2.Zero;
        public Vector2 EndValue { get; set; } = Vector2.Zero;
        public override void Apply(double time, S2VXStory story) {
            var position = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Camera.Position = position;
        }
        protected override string ToValues() => $"{S2VXUtils.Vector2ToString(StartValue)}|{S2VXUtils.Vector2ToString(EndValue)}";
        public static CameraMoveCommand FromString(string[] split) {
            var command = new CameraMoveCommand() {
                StartValue = S2VXUtils.Vector2FromString(split[4]),
                EndValue = S2VXUtils.Vector2FromString(split[5]),
            };
            return command;
        }
    }

    public class CameraRotateCommand : Command {
        public override CommandType Type { get; set; } = CommandType.CameraRotate;
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

    public class CameraScaleCommand : Command {
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

    public abstract class GridCommand : Command {
        public Grid Grid { get; } = new Grid();
    }

    public class GridAlphaCommand : Command {
        public override CommandType Type { get; set; } = CommandType.GridAlpha;
        public float StartValue { get; set; } = 1;
        public float EndValue { get; set; } = 1;
        public override void Apply(double time, S2VXStory story) {
            var alpha = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Grid.Alpha = alpha;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static GridAlphaCommand FromString(string[] split) {
            var command = new GridAlphaCommand() {
                StartValue = float.Parse(split[4], CultureInfo.InvariantCulture),
                EndValue = float.Parse(split[5], CultureInfo.InvariantCulture),
            };
            return command;
        }
    }

    public class GridColorCommand : Command {
        public override CommandType Type { get; set; } = CommandType.GridColor;
        public Color4 StartValue { get; set; } = Color4.White;
        public Color4 EndValue { get; set; } = Color4.White;
        public override void Apply(double time, S2VXStory story) {
            var color = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Grid.Colour = color;
        }
        protected override string ToValues() => $"{S2VXUtils.Color4ToString(StartValue)}|{S2VXUtils.Color4ToString(EndValue)}";
        public static GridColorCommand FromString(string[] split) {
            var command = new GridColorCommand() {
                StartValue = S2VXUtils.Color4FromString(split[4]),
                EndValue = S2VXUtils.Color4FromString(split[5]),
            };
            return command;
        }
    }

    public class GridThicknessCommand : Command {
        public override CommandType Type { get; set; } = CommandType.GridThickness;
        public float StartValue { get; set; } = 0.005f;
        public float EndValue { get; set; } = 0.005f;
        public override void Apply(double time, S2VXStory story) {
            var thickness = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Grid.Thickness = thickness;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static GridThicknessCommand FromString(string[] split) {
            var command = new GridThicknessCommand() {
                StartValue = float.Parse(split[4], CultureInfo.InvariantCulture),
                EndValue = float.Parse(split[5], CultureInfo.InvariantCulture),
            };
            return command;
        }
    }

    public class BackgroundColorCommand : Command {
        public override CommandType Type { get; set; } = CommandType.BackgroundColor;
        public Color4 StartValue { get; set; } = Color4.White;
        public Color4 EndValue { get; set; } = Color4.White;
        public override void Apply(double time, S2VXStory story) {
            var color = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Background.Colour = color;
        }
        protected override string ToValues() => $"{S2VXUtils.Color4ToString(StartValue)}|{S2VXUtils.Color4ToString(EndValue)}";
        public static BackgroundColorCommand FromString(string[] split) {
            var command = new BackgroundColorCommand() {
                StartValue = S2VXUtils.Color4FromString(split[4]),
                EndValue = S2VXUtils.Color4FromString(split[5]),
            };
            return command;
        }
    }

    public class NotesFadeInTimeCommand : Command {
        public override CommandType Type { get; set; } = CommandType.NotesFadeInTime;
        public float StartValue { get; set; } = 100.0f;
        public float EndValue { get; set; } = 100.0f;
        public override void Apply(double time, S2VXStory story) {
            var fadeInTime = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.FadeInTime = fadeInTime;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static NotesFadeInTimeCommand FromString(string[] split) {
            var command = new NotesFadeInTimeCommand() {
                StartValue = float.Parse(split[4], CultureInfo.InvariantCulture),
                EndValue = float.Parse(split[5], CultureInfo.InvariantCulture),
            };
            return command;
        }
    }

    public class NotesShowTimeCommand : Command {
        public override CommandType Type { get; set; } = CommandType.NotesShowTime;
        public float StartValue { get; set; } = 100.0f;
        public float EndValue { get; set; } = 100.0f;
        public override void Apply(double time, S2VXStory story) {
            var showTime = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.ShowTime = showTime;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static NotesShowTimeCommand FromString(string[] split) {
            var command = new NotesShowTimeCommand() {
                StartValue = float.Parse(split[4], CultureInfo.InvariantCulture),
                EndValue = float.Parse(split[5], CultureInfo.InvariantCulture),
            };
            return command;
        }
    }

    public class NotesFadeOutTimeCommand : Command {
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

    public class NotesAlphaCommand : Command {
        public override CommandType Type { get; set; } = CommandType.NotesAlpha;
        public float StartValue { get; set; } = 1;
        public float EndValue { get; set; } = 1;
        public override void Apply(double time, S2VXStory story) {
            var alpha = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.Alpha = alpha;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static NotesAlphaCommand FromString(string[] split) {
            var command = new NotesAlphaCommand() {
                StartValue = float.Parse(split[4], CultureInfo.InvariantCulture),
                EndValue = float.Parse(split[5], CultureInfo.InvariantCulture),
            };
            return command;
        }
    }
    public class NotesColorCommand : Command {
        public override CommandType Type { get; set; } = CommandType.NotesColor;
        public Color4 StartValue { get; set; } = Color4.White;
        public Color4 EndValue { get; set; } = Color4.White;
        public override void Apply(double time, S2VXStory story) {
            var color = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.Colour = color;
        }
        protected override string ToValues() => $"{S2VXUtils.Color4ToString(StartValue)}|{S2VXUtils.Color4ToString(EndValue)}";
        public static NotesColorCommand FromString(string[] split) {
            var command = new NotesColorCommand() {
                StartValue = S2VXUtils.Color4FromString(split[4]),
                EndValue = S2VXUtils.Color4FromString(split[5]),
            };
            return command;
        }
    }

    public class ApproachesDistanceCommand : Command {
        public override CommandType Type { get; set; } = CommandType.ApproachesDistance;
        public float StartValue { get; set; } = 0.5f;
        public float EndValue { get; set; } = 0.5f;
        public override void Apply(double time, S2VXStory story) {
            var distance = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.ApproachDistance = distance;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static ApproachesDistanceCommand FromString(string[] split) {
            var command = new ApproachesDistanceCommand() {
                StartValue = float.Parse(split[4], CultureInfo.InvariantCulture),
                EndValue = float.Parse(split[5], CultureInfo.InvariantCulture),
            };
            return command;
        }
    }

    public class ApproachesThicknessCommand : Command {
        public override CommandType Type { get; set; } = CommandType.ApproachesThickness;
        public float StartValue { get; set; } = 0.005f;
        public float EndValue { get; set; } = 0.005f;
        public override void Apply(double time, S2VXStory story) {
            var thickness = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.Notes.ApproachThickness = thickness;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static ApproachesThicknessCommand FromString(string[] split) {
            var command = new ApproachesThicknessCommand() {
                StartValue = float.Parse(split[4], CultureInfo.InvariantCulture),
                EndValue = float.Parse(split[5], CultureInfo.InvariantCulture),
            };
            return command;
        }
    }

    public class TimingChangeCommand : Command {
        public override CommandType Type { get; set; } = CommandType.TimingChange;
        public float StartValue { get; set; }
        public float EndValue { get; set; }
        public override void Apply(double time, S2VXStory story) {
            var bpm = Interpolation.ValueAt(time, StartValue, EndValue, StartTime, EndTime, Easing);
            story.BPM = bpm;
            story.Offset = StartTime;
        }
        protected override string ToValues() => $"{StartValue}|{EndValue}";
        public static TimingChangeCommand FromString(string[] split) {
            var command = new TimingChangeCommand() {
                StartValue = float.Parse(split[4], CultureInfo.InvariantCulture),
                EndValue = float.Parse(split[5], CultureInfo.InvariantCulture),
            };
            return command;
        }
    }
}
