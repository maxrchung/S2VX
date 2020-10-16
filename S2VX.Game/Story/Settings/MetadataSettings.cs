using NUnit.Framework;
using S2VX.Game.Story.Command;
using System.Collections.Generic;
using System.Linq;

namespace S2VX.Game.Story.Settings {
    public class MetadataSettings {
        public string SongTitle { get; set; }
        public string SongArtist { get; set; }
        public string StoryAuthor { get; set; }
        public string StoryChapter { get; set; }
        // Slowest BPM of song found by parsing TimingChangeCommands
        public float SlowestBPM { get; set; } = new TimingChangeCommand().StartValue;
        // Fastest BPM of song
        public float FastestBPM { get; set; } = new TimingChangeCommand().StartValue;
        // Time difference between the earliest Note and the latest Note
        public double SongLength { get; set; }
        public int CommandCount { get; set; }
        public int NoteCount { get; set; }
        public string ThumbnailFileName { get; set; }
        public string SongFileName { get; set; }
        public string MiscDescription { get; set; }

        // Updates properties based on story configuration
        public void Calculate(S2VXStory story) {
            var timingCommands = story.Commands.OfType<TimingChangeCommand>();
            if (timingCommands.Any()) {
                var min = float.MaxValue;
                var max = float.MinValue;
                foreach (var timingCommand in timingCommands) {
                    foreach (var value in new float[2] { timingCommand.StartValue, timingCommand.EndValue }) {
                        if (value < min) {
                            min = value;
                        }
                        if (value > max) {
                            max = value;
                        }
                    }
                }
                SlowestBPM = min;
                FastestBPM = max;
            }

            if (story.Notes.Children.Any()) {
                var min = double.MaxValue;
                var max = double.MinValue;
                foreach (var note in story.Notes.Children) {
                    foreach (var value in new double[2] { note.EndTime, note.EndTime }) {
                        if (value < min) {
                            min = value;
                        }
                        if (value > max) {
                            max = value;
                        }
                    }
                }
                SongLength = max - min;
            }
            
            CommandCount = story.Commands.Count;
            NoteCount = story.Notes.Children.Count;
        }
    }
}
