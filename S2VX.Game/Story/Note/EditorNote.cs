using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using S2VX.Game.Editor;
using S2VX.Game.Editor.Reversible;

namespace S2VX.Game.Story.Note {
    public class EditorNote : S2VXNote {
        private bool CanHit { get; set; }
        private SampleChannel Hit { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) =>
            Hit = audio.Samples.Get("hit");

        public override bool UpdateNote() {
            var time = Clock.CurrentTime;
            if (time >= HitTime && CanHit) {
                CanHit = false;
                Hit.Play();
            }
            // Reset hit sound if clock is running and before end time
            CanHit = Clock.IsRunning && time < HitTime;
            return base.UpdateNote();
        }

        public override void ReversibleRemove(S2VXStory story, EditorScreen editor) =>
            editor.Reversibles.Push(new ReversibleRemoveNote(story, this, editor));
    }
}
