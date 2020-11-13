using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Utils;
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

        public void Update() {
            Update();
        }

        protected override void Update() {
            // For EditorHold notes, override alpha between HitTime and EndTime
            var notes = Story.Notes;
            var time = Time.Current;
            var endFadeOut = EndTime + notes.FadeOutTime;
            var startTime = HitTime - notes.ShowTime;

            //UpdatePlacement();

            if (time >= endFadeOut) {
                Alpha = 0;
                // Return early to save some calculations
                return;
            }

            if (time >= EndTime) {
                var alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, EndTime, endFadeOut);
                Alpha = alpha;
            } else if (time >= startTime) {
                Alpha = 1;
            }

            // Deduct number of hit sounds to play once we've passed each HitSoundTime
            if (NumHitSounds > 0 && time >= HitSoundTimes[^NumHitSounds]) {
                --NumHitSounds;
                Hit.Play();
            }

            // Reset hit sound counter if clock is running and before timing points
            if (Clock.IsRunning) {
                NumHitSounds = HitSoundTimes.Count - GetNumTimingPointsPassed();
            }
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
