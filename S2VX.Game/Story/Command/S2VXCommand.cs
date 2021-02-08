using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace S2VX.Game.Story.Command {
    public abstract class S2VXCommand : IComparable<S2VXCommand> {
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public Easing Easing { get; set; } = Easing.None;
        public abstract void Apply(double time, S2VXStory story);

        /// <summary>
        /// Compare commands by start time, then end time, then name
        /// </summary>
        public int CompareTo(S2VXCommand other) {
            var compare = StartTime.CompareTo(other.StartTime);
            if (compare == 0) {
                compare = EndTime.CompareTo(other.EndTime);
            }
            if (compare == 0) {
                compare = string.Compare(GetType().Name, other.GetType().Name, StringComparison.OrdinalIgnoreCase);
            }
            return compare;
        }

        protected abstract string ToValues();
        public override string ToString() => $"{GetCommandName()}|{StartTime}|{EndTime}|{Easing}|{ToValues()}";

        public static string GetShortName(string fullName) => fullName.Replace("Command", "", StringComparison.Ordinal);

        public string GetCommandName() => GetShortName(GetType().Name);

        private static IEnumerable<Type> GetCommandTypes() {
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
            return validCommands;
        }

        public static IEnumerable<string> GetCommandNames() {
            var commandTypes = GetCommandTypes();
            var commandNames = commandTypes.Select(t => GetShortName(t.Name));
            return commandNames;
        }

        public static IEnumerable<S2VXCommand> GetDefaultCommands() {
            var commandTypes = GetCommandTypes();
            var defaultCommands = commandTypes.Select(t => Activator.CreateInstance(t) as S2VXCommand);
            return defaultCommands;
        }

        public static S2VXCommand FromString(string data) {
            var split = data.Split("|");
            var commandName = split[0];
            var systemType = Type.GetType($"S2VX.Game.Story.Command.{commandName}Command");
            var staticMethod = systemType.GetMethod("FromString", BindingFlags.Public | BindingFlags.Static);
            var command = staticMethod.Invoke(null, new object[] { split }) as S2VXCommand;
            command.StartTime = S2VXUtils.StringToDouble(split[1]);
            command.EndTime = S2VXUtils.StringToDouble(split[2]);
            command.Easing = Enum.Parse<Easing>(split[3]);
            return command;
        }

        public static S2VXCommand FromJson(JObject json) {
            var commandName = json["Type"].ToString();
            var systemType = Type.GetType($"S2VX.Game.Story.Command.{commandName}Command");
            var data = json.ToString();
            var command = JsonConvert.DeserializeObject(data, systemType) as S2VXCommand;
            return command;
        }
    }
}
