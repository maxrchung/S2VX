using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using System.Collections.Generic;

namespace S2VX.Game.Story.Note {
    public class EditorHoldNote : HoldNote {
        public int NumHitSounds { get; set; }
        public List<double> HitSoundTimes { get; private set; }
        public SampleChannel Hit { get; private set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        public override void UpdateHitTime(double hitTime) {
            base.UpdateHitTime(hitTime);
            EndTime = hitTime + 1000;    // TODO: #216 be able to change hold duration
            ((HoldApproach)Approach).EndTime = EndTime;
            HitSoundTimes = new List<double>() { HitTime, EndTime };
        }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            Hit = audio.Samples.Get("hit");
            HitSoundTimes = new List<double>() { HitTime, EndTime };
        }

        public int GetNumTimingPointsPassed() {
            var time = Time.Current;
            var ans = 0;
            foreach (var hitSoundTime in HitSoundTimes) {
                if (time >= hitSoundTime) {
                    ++ans;
                } else {
                    break;
                }
            }
            return ans;
        }
    }
}
