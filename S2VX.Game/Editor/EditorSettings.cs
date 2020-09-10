namespace S2VX.Game.Editor {
    public class EditorSettings {
        // Default editor settings for new projects
        public double TrackTime { get; set; }
        public double TrackVolume { get; set; } = 0.2;
        public double TrackPlaybackRate { get; set; } = 1.0;
        public int SnapDivisor { get; set; } = 1;
        public int BeatSnapDivisorIndex { get; set; } = 3;
    }
}
