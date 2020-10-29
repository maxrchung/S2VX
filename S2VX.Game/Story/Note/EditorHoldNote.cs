using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Utils;
using System.Collections.Generic;

namespace S2VX.Game.Story.Note {
    public class EditorHoldNote : S2VXNote {
        private int NumHitSounds { get; set; }
        private List<double> HitSoundTimes { get; set; }
        private SampleChannel Hit { get; set; }
        public double EndTime { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            Hit = audio.Samples.Get("hit");
            HitSoundTimes = new List<double>() { HitTime, EndTime };
        }

        public override void UpdateHitTime(double hitTime) {
            base.UpdateHitTime(hitTime);
            EndTime = hitTime + 1000;   // TODO: change this
        }

        protected override void Update() {
            base.Update();

            // For EditorHold notes, override alpha between HitTime and EndTime
            var notes = Story.Notes;
            var time = Time.Current;
            var endFadeOut = EndTime + notes.FadeOutTime;
            var startTime = HitTime - notes.ShowTime;

            if (time >= endFadeOut) {
                Alpha = 0;
                // Return early to save some calculations
                return;
            }

            UpdatePlacement();

            if (time >= EndTime) {
                var alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, EndTime, endFadeOut);
                Alpha = alpha;
            } else if (time >= startTime) {
                Alpha = 1;
            }

            switch (NumHitSounds) {
                case 2:
                    if (time >= HitTime) {
                        --NumHitSounds;
                        Hit.Play();
                    }
                    break;
                case 1:
                    if (time >= EndTime) {
                        --NumHitSounds;
                        Hit.Play();
                    }
                    break;
                case 0:
                    break;
            }

            // Reset hit sound counter if clock is running and before timing points
            if (Clock.IsRunning) {
                if (time < HitTime) {
                    NumHitSounds = HitSoundTimes.Count;
                } else if (time < EndTime) {
                    NumHitSounds = HitSoundTimes.Count - 1;
                }
            }
        }
    }
}
