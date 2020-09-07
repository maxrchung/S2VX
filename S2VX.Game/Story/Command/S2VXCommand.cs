using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Graphics;
using System;
using System.Globalization;

namespace S2VX.Game.Story.Command {
    public abstract class S2VXCommand : IComparable<S2VXCommand> {
        public abstract CommandType Type { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public Easing Easing { get; set; } = Easing.None;
        public abstract void Apply(double time, S2VXStory story);

        public int CompareTo(S2VXCommand other) => StartTime.CompareTo(other.StartTime);

        protected abstract string ToValues();
        public override string ToString() => $"{Type}|{StartTime}|{EndTime}|{Easing}|{ToValues()}";

        public static S2VXCommand FromString(string data) {
            var split = data.Split("|");
            var type = Enum.Parse<CommandType>(split[0]);
            S2VXCommand command = null;
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

        public static S2VXCommand FromJson(JObject json) {
            var type = Enum.Parse<CommandType>(json[nameof(Type)].ToString());
            var data = json.ToString();
            S2VXCommand command = null;
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
                    command = JsonConvert.DeserializeObject<ApproachesDistanceCommand>(data);
                    break;
                case CommandType.ApproachesThickness:
                    command = JsonConvert.DeserializeObject<ApproachesThicknessCommand>(data);
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

        public static bool operator ==(S2VXCommand left, S2VXCommand right) => left is null ? right is null : left.Equals(right);

        public static bool operator !=(S2VXCommand left, S2VXCommand right) => !(left == right);

        public static bool operator <(S2VXCommand left, S2VXCommand right) => left is null ? right is null : left.CompareTo(right) < 0;

        public static bool operator <=(S2VXCommand left, S2VXCommand right) => left is null || left.CompareTo(right) <= 0;

        public static bool operator >(S2VXCommand left, S2VXCommand right) => left is object && left.CompareTo(right) > 0;

        public static bool operator >=(S2VXCommand left, S2VXCommand right) => left is null ? right is null : left.CompareTo(right) >= 0;
    }
}
