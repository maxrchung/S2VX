using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Framework.Utils;
using osuTK.Graphics;
using osuTK.Input;
using S2VX.Game.Play;
using System;

namespace S2VX.Game.Story.Note {
    public class GameNote : S2VXNote {

        [Resolved]
        private S2VXScore Score { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [Resolved]
        private ScreenStack ScreenStack { get; set; }

        private int TimingError;

        private void Delete() {
            var playScreen = (PlayScreen)ScreenStack.CurrentScreen;
            playScreen.PlayInfoBar.RecordHitError(TimingError);
            Story.RemoveNote(this);
        }

        private void RecordMiss() {
            var missThreshold = Story.Notes.MissThreshold;
            TimingError = missThreshold;
            Score.Value += missThreshold;
            Delete();
        }

        // Returns whether or not the click occurred within the MissThreshold
        private bool IsClickable() {
            var time = Time.Current;
            TimingError = (int)(time - EndTime);
            return Math.Abs(TimingError) <= Story.Notes.MissThreshold;
        }

        protected override bool OnMouseDown(MouseDownEvent e) {
            if (IsClickable()) {
                Score.Value += Math.Abs(TimingError);
                Delete();
            }
            return false;
        }

        protected override bool OnKeyDown(KeyDownEvent e) {
            if (IsClickable()) {
                switch (e.Key) {
                    case Key.Z:
                    case Key.X:
                    case Key.C:
                    case Key.V:
                    case Key.A:
                    case Key.S:
                    case Key.D:
                    case Key.F:
                        Score.Value += Math.Abs(TimingError);
                        Delete();
                        break;
                    default:
                        break;
                }
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
                Colour = Color4.Red;
                if (time >= EndTime + notes.MissThreshold) {
                    RecordMiss();
                }
            }
        }
    }
}
