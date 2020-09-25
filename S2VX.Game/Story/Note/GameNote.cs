using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using S2VX.Game.Play;
using System;

namespace S2VX.Game.Story.Note {
    public class GameNote : S2VXNote {

        [Resolved]
        private S2VXScore Score { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        private double ClickedAtTime;
        private int TimingError;

        private void RecordMiss() {
            Score.Value += Story.Notes.MissThreshold;
            Console.WriteLine($"{EndTime} missed, score is now {Score.Value}");
            Story.RemoveNote(this);
        }

        // Returns whether or not the click occurred within the MissThreshold
        private bool IsClickable() {
            var time = Time.Current;
            ClickedAtTime = time;
            TimingError = Math.Abs((int)(EndTime - time));
            return TimingError <= Story.Notes.MissThreshold;
        }

        protected override bool OnClick(ClickEvent e) {
            if (IsClickable()) {
                Score.Value += TimingError;
                Console.WriteLine($"{EndTime} clicked at {ClickedAtTime}, score is now {Score.Value}");
                Story.RemoveNote(this);
            }
            return false;
        }

        protected override void Update() {
            base.Update();
            var time = Time.Current;
            if (time >= EndTime) {
                // Hold the note at fully visible until after MissThreshold
                var notes = Story.Notes;
                var startFadeTime = EndTime + notes.MissThreshold;
                var endFadeTime = startFadeTime + notes.FadeOutTime;
                var alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, startFadeTime, endFadeTime);
                Alpha = alpha;
                if (time >= EndTime + notes.MissThreshold) {
                    RecordMiss();
                }
            }
        }
    }
}
