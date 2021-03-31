using osu.Framework.Audio;
using osu.Framework.Audio.Sample;

namespace S2VX.Game {
    /// <summary>
    /// Wrapper around Sample so that we can keep count of how many times the sample was played
    /// </summary>
    public class S2VXSample {
        public int PlayCount { get; private set; }
        private Sample Sample { get; }

        public S2VXSample(string sampleName, AudioManager audioManager) =>
            Sample = audioManager.Samples.Get(sampleName);

        public void Play() {
            Sample.Play();
            ++PlayCount;
        }

        public void Reset() => PlayCount = 0;
    }
}
