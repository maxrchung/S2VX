namespace S2VX.Game.Editor {
    public class MetadataSettings {
        public string SongTitle { get; set; }
        public string SongArtist { get; set; }
        public string StoryAuthor { get; set; }
        public string StoryChapter { get; set; }
        // Slowest BPM of song found by parsing TimingChangeCommands
        public float SlowestBPM { get; set; }
        // Fastest BPM of song
        public float FastestBPM { get; set; }
        // Time difference between the earliest Note and the latest Note
        public double SongLength { get; set; }
        public int CommandCount { get; set; }
        public int NoteCount { get; set; }
        public string ThumbnailFileName { get; set; }
        public string SongFileName { get; set; }
        public string MiscDescription { get; set; }
    }
}
