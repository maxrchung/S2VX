using Newtonsoft.Json;

namespace S2VX.Game.Editor {
    [JsonObject]
    public class EditorSettings {
        // Default editor settings for new projects
        [JsonProperty(PropertyName = "TrackTime")]
        public double TrackTime { get; set; }

        [JsonProperty(PropertyName = "TrackVolume")]
        public double TrackVolume { get; set; } = 0.2;

        [JsonProperty(PropertyName = "TrackPlaybackRate")]
        public double TrackPlaybackRate { get; set; } = 1.0;

        [JsonProperty(PropertyName = "SnapDivisor")]
        public int SnapDivisor { get; set; } = 1;

        [JsonProperty(PropertyName = "BeatSnapDivisorIndex")]
        public int BeatSnapDivisorIndex { get; set; } = 3;
    }
}
