using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Utils;
using osuTK;
using S2VX.Game.Editor;
using S2VX.Game.Editor.Reversible;
using System.Collections.Generic;

namespace S2VX.Game.Story.Note {
    public class EditorHoldNote : HoldNote {
        private int NumHitSounds { get; set; }
        private List<double> HitSoundTimes { get; set; }
        private SampleChannel Hit { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            Hit = audio.Samples.Get("hit");
            HitSoundTimes = new List<double>() { HitTime, EndTime };
        }

        public override void UpdateHitTime(double hitTime) {
            EndTime = hitTime + EndTime - HitTime;
            base.UpdateHitTime(hitTime);
            ((HoldApproach)Approach).EndTime = EndTime;
            HitSoundTimes = new List<double>() { HitTime, EndTime };
        }

        public override void UpdateCoordinates(Vector2 coordinates) {
            EndCoordinates = coordinates + EndCoordinates - Coordinates;
            base.UpdateCoordinates(coordinates);
            ((HoldApproach)Approach).EndCoordinates = EndCoordinates;
        }

        public override bool UpdateNote() {
            base.UpdateNote();

            // For EditorHold notes, override alpha between HitTime and EndTime
            var notes = Story.Notes;
            var time = Time.Current;
            var endFadeOut = EndTime + notes.FadeOutTime;
            var startTime = HitTime - notes.ShowTime;

            var coordinates = Interpolation.ValueAt(Time.Current, Coordinates, EndCoordinates, HitTime, EndTime);
            UpdatePlacement(coordinates);

            if (time >= endFadeOut) {
                Alpha = 0;
                // Return early to save some calculations
                return false;
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
            return false;
        }

        private int GetNumTimingPointsPassed() {
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

        public override void ReversibleRemove(S2VXStory story, EditorScreen editor) =>
            editor.Reversibles.Push(new ReversibleRemoveHoldNote(story, this, editor));
    }
}
