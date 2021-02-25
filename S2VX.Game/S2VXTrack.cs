using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Audio;
using System.IO;

namespace S2VX.Game {
    /// <summary>
    /// Wrapper around DrawableTrack so that we can keep reference to the audio path
    /// </summary>
    public class S2VXTrack : DrawableTrack {
        public string AudioPath { get; }

        public S2VXTrack(string audioPath, Track track) : base(track) =>
            AudioPath = audioPath;

        public static S2VXTrack Open(string audioPath, AudioManager audioManager) {
            var trackStream = File.OpenRead(audioPath);
            var track = new TrackBass(trackStream);
            audioManager.AddItem(track);
            return new S2VXTrack(audioPath, track);
        }
    }
}
