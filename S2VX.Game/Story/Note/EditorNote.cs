﻿using osu.Framework.Allocation;
using osu.Framework.Audio;
using S2VX.Game.Editor;
using S2VX.Game.Editor.Reversible;

namespace S2VX.Game.Story.Note {
    public class EditorNote : S2VXNote {
        public S2VXSample Hit { get; private set; }

        private bool CanHit { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) =>
            Hit = new S2VXSample("hit", audio);

        public override bool UpdateNote() {
            UpdateColor();
            UpdatePosition();

            var time = Clock.CurrentTime;
            if (time >= HitTime && CanHit) {
                CanHit = false;
                Hit.Play();
            }
            // Reset hit sound if clock is running and before end time
            CanHit = Clock.IsRunning && time < HitTime;
            return false;
        }

        protected override void UpdateColor() {
            var time = Time.Current;
            var notes = Story.Notes;
            var maxAlpha = notes.NoteAlpha;
            // Fade in time to Show time
            if (time < HitTime - notes.ShowTime) {
                var startTime = HitTime - notes.ShowTime - notes.FadeInTime;
                var endTime = HitTime - notes.ShowTime;
                Alpha = S2VXUtils.ClampedInterpolation(time, 0.0f, maxAlpha, startTime, endTime);
            }
            // Show time to Hit time
            else if (time < HitTime) {
                Alpha = maxAlpha;
            }
            // Hit time to Fade out time
            else if (time < HitTime + notes.FadeOutTime) {
                var startTime = HitTime;
                var endTime = HitTime + notes.FadeOutTime;
                Alpha = S2VXUtils.ClampedInterpolation(time, maxAlpha, 0.0f, startTime, endTime);
            } else {
                Alpha = 0;
            }
        }

        public override void ReversibleRemove(S2VXStory story, EditorScreen editor) =>
            editor.Reversibles.Push(new ReversibleRemoveNote(story, this, editor));
    }
}
