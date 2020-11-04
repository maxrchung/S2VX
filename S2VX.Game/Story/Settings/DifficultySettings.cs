using S2VX.Game.Story.Command;
using System;
using System.Linq;

namespace S2VX.Game.Story.Settings {
    public class DifficultySettings {
        // Slowest BPM of song found by parsing TimingChangeCommands
        public float SlowestBPM { get; set; }
        // Fastest BPM of song
        public float FastestBPM { get; set; }
        // Time difference between the earliest Note and the latest Note
        public double StoryLength { get; set; }
        public int CommandCount { get; set; }
        public int NoteCount { get; set; }

        // Updates properties based on story configuration
        public void Calculate(S2VXStory story) {
            var commands = story.Commands;
            CommandCount = commands.Count;
            var timingCommands = story.Commands.OfType<TimingChangeCommand>();
            if (timingCommands.Any()) {
                SlowestBPM = timingCommands.Min(t => Math.Min(t.StartValue, t.EndValue));
                FastestBPM = timingCommands.Max(t => Math.Max(t.StartValue, t.EndValue));
            }

            var notes = story.Notes.Children;
            NoteCount = notes.Count;
            if (NoteCount > 0) {
                var min = notes.Min(n => n.HitTime);
                var max = notes.Max(n => n.HitTime);
                StoryLength = max - min;
            }
        }
    }
}
