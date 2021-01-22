using S2VX.Game.Story.Note;
using System;

namespace StoryMerge {
    public class NoteInfo : IComparable<NoteInfo> {
        public string Type { get; } = "";
        public double StartTime { get; }
        public double EndTime { get; }

        public NoteInfo(S2VXNote note) {
            Type = "Note";
            StartTime = note.HitTime;
            EndTime = note.HitTime;
        }

        public NoteInfo(HoldNote holdNote) {
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

        public int CompareTo(NoteInfo other) => StartTime.CompareTo(other.StartTime);
    }
}
