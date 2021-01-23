using S2VX.Game.Story.Note;
using System;

namespace StoryMerge {
    public class NoteTimeInfo : IComparable<NoteTimeInfo> {
        public string Type { get; } = "";
        public double StartTime { get; }
        public double EndTime { get; }

        public NoteTimeInfo(S2VXNote note) {
            Type = "Note";
            StartTime = note.HitTime;
            EndTime = note.HitTime;
        }

        public NoteTimeInfo(HoldNote holdNote) {
            Type = "HoldNote";
            StartTime = holdNote.HitTime;
            EndTime = holdNote.EndTime;
        }

        public override string ToString() {
            var toString = "";
            if (Type == "Note") {
                toString = $"{Type} at {StartTime}";
            } else if (Type == "HoldNote") {
                toString = $"{Type} from {StartTime} to {EndTime}";
            }
            return toString;
        }

        public int CompareTo(NoteTimeInfo other) {
            var compare = StartTime.CompareTo(other.StartTime);
            if (compare == 0) {
                compare = EndTime.CompareTo(other.EndTime);
            }
            return compare;
        }
    }
}
