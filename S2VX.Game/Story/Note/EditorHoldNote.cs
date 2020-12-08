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
            // Prevent changes at end time or else we'll get some wild interpolation
            if (EndTime - Time.Current <= EditorScreen.TrackTimeTolerance) {
                return;
            }

            var startCoordinates = Interpolation.ValueAt(HitTime, coordinates, EndCoordinates, Time.Current, EndTime);
            base.UpdateCoordinates(startCoordinates);
            EndCoordinates = startCoordinates + EndCoordinates - Coordinates;
            ((HoldApproach)Approach).EndCoordinates = EndCoordinates;
        }

        public override bool UpdateNote() {
            UpdatePlacement();
            UpdateAlpha();

            var time = Time.Current;
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

        protected override void UpdateAlpha() {
            var time = Time.Current;
            var notes = Story.Notes;
            // Fade in time to Show time
            if (time < HitTime - notes.ShowTime) {
                var startTime = HitTime - notes.ShowTime - notes.FadeInTime;
                var endTime = HitTime - notes.ShowTime;
                Alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startTime, endTime);
            }
            // Show time (to Hit time) to End time
            else if (time < EndTime) {
                Alpha = 1;
            }
            // End time to Fade out time
            else if (time < EndTime + notes.FadeOutTime) {
                var startTime = EndTime;
                var endTime = EndTime + notes.FadeOutTime;
                Alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, startTime, endTime);
            } else {
                Alpha = 0;
            }
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
