using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace S2VX.Game.Story.Command {
    public abstract class S2VXCommand : IComparable<S2VXCommand> {
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public Easing Easing { get; set; } = Easing.None;
        public abstract void Apply(double time, S2VXStory story);

        public int CompareTo(S2VXCommand other) => StartTime.CompareTo(other.StartTime);

        protected abstract string ToValues();
        public override string ToString() => $"{GetCommandType()}|{StartTime}|{EndTime}|{Easing}|{ToValues()}";

        public static string GetShortName(string fullName) => fullName.Replace("Command", "", StringComparison.Ordinal);

        public string GetCommandType() => GetShortName(GetType().Name);

        public static IEnumerable<string> GetCommandTypes() {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            var allCommands = types.Where(
                t => string.Equals(t.Namespace, "S2VX.Game.Story.Command", StringComparison.Ordinal)
            );
            var validCommands = allCommands.Where(t =>
                // Skips compiler auto-generated classes
                t.Name.Contains("Command", StringComparison.Ordinal)
                // Only get derived commands
                && !string.Equals(t.Name, "S2VXCommand", StringComparison.Ordinal)
            );
            var commandTypes = validCommands.Select(t => GetShortName(t.Name));
            return commandTypes;
        }

        public static S2VXCommand FromString(string data) {
            var split = data.Split("|");
            var commandType = split[0];
            var systemType = Type.GetType($"S2VX.Game.Story.Command.{commandType}Command");
            var staticMethod = systemType.GetMethod("FromString", BindingFlags.Public | BindingFlags.Static);
            var command = staticMethod.Invoke(null, new object[] { split }) as S2VXCommand;
            command.StartTime = double.Parse(split[1], CultureInfo.InvariantCulture);
            command.EndTime = double.Parse(split[2], CultureInfo.InvariantCulture);
            command.Easing = Enum.Parse<Easing>(split[3]);
            return command;
        }

        public static S2VXCommand FromJson(JObject json) {
            var commandType = json["Type"].ToString();
            var systemType = Type.GetType($"S2VX.Game.Story.Command.{commandType}Command");
            var data = json.ToString();
            var command = JsonConvert.DeserializeObject(data, systemType) as S2VXCommand;
            return command;
        }

        public override bool Equals(object obj) => ReferenceEquals(this, obj);

        public override int GetHashCode() => ToString().GetHashCode(StringComparison.Ordinal);

        public static bool operator ==(S2VXCommand left, S2VXCommand right) =>
            left is null ? right is null : left.Equals(right);

        public static bool operator !=(S2VXCommand left, S2VXCommand right) =>
            !(left == right);

        public static bool operator <(S2VXCommand left, S2VXCommand right) =>
            left is null ? right is null : left.CompareTo(right) < 0;

        public static bool operator <=(S2VXCommand left, S2VXCommand right) =>
            left is null || left.CompareTo(right) <= 0;

        public static bool operator >(S2VXCommand left, S2VXCommand right) =>
            left is object && left.CompareTo(right) > 0;

        public static bool operator >=(S2VXCommand left, S2VXCommand right) =>
            left is null ? right is null : left.CompareTo(right) >= 0;
    }
}
