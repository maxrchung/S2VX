using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Utils;
using S2VX.Game.Editor;
using S2VX.Game.Editor.Reversible;

namespace S2VX.Game.Story.Note {
    public class EditorNote : S2VXNote {
        private bool CanHit { get; set; }
        private SampleChannel Hit { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) =>
            Hit = audio.Samples.Get("hit");

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
            // Fade in time to Show time
            if (time < HitTime - notes.ShowTime) {
                var startTime = HitTime - notes.ShowTime - notes.FadeInTime;
                var endTime = HitTime - notes.ShowTime;
                Alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startTime, endTime);
            }
            // Show time to Hit time
            else if (time < HitTime) {
                Alpha = 1;
            }
            // Hit time to Fade out time
            else if (time < HitTime + notes.FadeOutTime) {
                var startTime = HitTime;
                var endTime = HitTime + notes.FadeOutTime;
                Alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, startTime, endTime);
            } else {
                Alpha = 0;
            }
        }

        public override void ReversibleRemove(S2VXStory story, EditorScreen editor) =>
            editor.Reversibles.Push(new ReversibleRemoveNote(story, this, editor));
    }
}
