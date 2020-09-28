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

        private GameNote GetEarliestClickableNote() {
            var largestTimingError = -Story.Notes.MissThreshold;
            var earliestNote = new GameNote();
            var time = Time.Current;
            foreach (var note in Story.Notes.Children) {
                var timingError = (int)(time - note.EndTime);
                if (largestTimingError <= timingError && timingError <= Story.Notes.MissThreshold) {
                    largestTimingError = timingError;
                    earliestNote = (GameNote)note;
                }
            }
            return earliestNote;
        }

        // Returns whether or not the click occurred within the MissThreshold
        private bool IsClickable() {
            var time = Time.Current;
            TimingError = (int)(time - EndTime);
            var inMissThreshold = Math.Abs(TimingError) <= Story.Notes.MissThreshold;
            var isEarliestNote = GetEarliestClickableNote() == this;
            return inMissThreshold && isEarliestNote;
        }

        protected override bool OnMouseDown(MouseDownEvent e) {
            if (IsClickable()) {
                Score.Value += Math.Abs(TimingError);
                Delete();
            }
            return false;
        }

        protected override bool OnKeyDown(KeyDownEvent e) {
            if (IsClickable() && IsHovered) {
                switch (e.Key) {
                    case Key.Z:
                        Score.Value += Math.Abs(TimingError);
                        Delete();
                        break;
                    case Key.X:
                        Score.Value += Math.Abs(TimingError);
                        Delete();
                        break;
                    case Key.C:
                        Score.Value += Math.Abs(TimingError);
                        Delete();
                        break;
                    case Key.V:
                        Score.Value += Math.Abs(TimingError);
                        Delete();
                        break;
                    case Key.A:
                        Score.Value += Math.Abs(TimingError);
                        Delete();
                        break;
                    case Key.S:
                        Score.Value += Math.Abs(TimingError);
                        Delete();
                        break;
                    case Key.D:
                        Score.Value += Math.Abs(TimingError);
                        Delete();
                        break;
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
