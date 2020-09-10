using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Graphics;
using System;
using System.Globalization;
using System.Reflection;

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
            var systemType = System.Type.GetType($"S2VX.Game.Story.Command.{type}Command");
            var staticMethod = systemType.GetMethod("FromString", BindingFlags.Public | BindingFlags.Static);
            var command = staticMethod.Invoke(null, new object[] { split }) as S2VXCommand;
            command.Type = Enum.Parse<CommandType>(split[0]);
            command.StartTime = double.Parse(split[1], CultureInfo.InvariantCulture);
            command.EndTime = double.Parse(split[2], CultureInfo.InvariantCulture);
            command.Easing = Enum.Parse<Easing>(split[3]);
            return command;
        }

        public static S2VXCommand FromJson(JObject json) {
            var type = Enum.Parse<CommandType>(json[nameof(Type)].ToString());
            var systemType = System.Type.GetType($"S2VX.Game.Story.Command.{type}Command");
            var data = json.ToString();
            var command = JsonConvert.DeserializeObject(data, systemType) as S2VXCommand;
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
