using S2VX.Game.Story.Command;
using System;

namespace StoryMerge {
    public class CommandInfo : IComparable<CommandInfo> {
        public string Type { get; } = "";
        public double StartTime { get; }
        public double EndTime { get; }

        public CommandInfo(S2VXCommand command) {
            Type = command.GetCommandName();
            StartTime = command.StartTime;
            EndTime = command.EndTime;
        }

        public override string ToString() =>
            $"{Type} from {StartTime} to {EndTime}";

        public int CompareTo(CommandInfo other) {
            var compare = StartTime.CompareTo(other.StartTime);
            if (compare == 0) {
                compare = EndTime.CompareTo(other.EndTime);
            }
            return compare;
        }
    }
}
