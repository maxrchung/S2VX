using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osuTK.Graphics;
using osuTK.Input;
using S2VX.Game.Play;
using S2VX.Game.Play.UserInterface;
using System;
using System.Linq;

namespace S2VX.Game.Story.Note {
    public class GameHoldNote : HoldNote {
        [Resolved]
        private ScoreInfo ScoreInfo { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [Resolved]
        private PlayScreen PlayScreen { get; set; }

        private enum HoldNoteState {
            NotVisibleBefore,
            VisibleBefore,
            During,
            VisibleAfter
        }
        private const int MissThreshold = 200;
        private SampleChannel Hit { get; set; }
        private SampleChannel Miss { get; set; }
        private int ScoreBefore { get; set; }
        private int ScoreDuring { get; set; }
        private int ScoreAfter { get; set; }
        private HoldNoteState State { get; set; } = HoldNoteState.NotVisibleBefore;
        private Key? KeyBeingHeld { get; set; }
        private MouseButton? MouseButtonBeingHeld { get; set; }
        private bool ShouldBeRemoved { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            Hit = audio.Samples.Get("hit");
            Miss = audio.Samples.Get("miss");
        }

        private void Delete() {
            ScoreBefore = Math.Clamp(ScoreBefore, 0, MissThreshold);
            ScoreAfter = Math.Clamp(ScoreAfter, 0, MissThreshold);
            var totalScore = ScoreBefore + ScoreDuring + ScoreAfter;
            PlayScreen.PlayInfoBar.RecordHitError(totalScore);
            ShouldBeRemoved = true;
        }

        private bool IsBeingHeld() => KeyBeingHeld != null || MouseButtonBeingHeld != null;

        // Note is clickable if in a visible state and is the earliest note
        private bool IsClickable() {
            var isVisible = State != HoldNoteState.NotVisibleBefore;
            var isEarliestNote = Story.Notes.Children.Last() == this;
            return isVisible && isEarliestNote;
        }

        private void ClickNote() {
            if (!IsBeingHeld()) {
                var time = Time.Current;
                if (Math.Abs(HitTime - time) < MissThreshold) {
                    Hit.Play();
                } else {
                    Miss.Play();
                }
            }
        }

        private void ReleaseNote() {
            var time = Time.Current;
            if (Math.Abs(EndTime - time) < MissThreshold) {
                Hit.Play();
            } else {
                Miss.Play();
            }
        }

        protected override bool OnMouseDown(MouseDownEvent e) {
            if (IsClickable()) {
                ClickNote();
                MouseButtonBeingHeld = e.Button;
            }

            return false;
        }

        protected override void OnMouseUp(MouseUpEvent e) {
            // Mouse up needs to have a dispose check because it's possible to
            // leave the current screen and have the on mouse up trigger on the
            // next screen.
            if (IsDisposed) {
                return;
            }

            if (!ShouldBeRemoved && MouseButtonBeingHeld == e.Button) {
                MouseButtonBeingHeld = null;
                ReleaseNote();
            }
        }

        protected override bool OnKeyDown(KeyDownEvent e) {
            if (IsClickable() && IsHovered) {
                switch (e.Key) {
                    case Key.Z:
                    case Key.X:
                    case Key.C:
                    case Key.V:
                    case Key.A:
                    case Key.S:
                    case Key.D:
                    case Key.F:
                        ClickNote();
                        KeyBeingHeld = e.Key;
                        break;
                    default:
                        break;
                }
            }

            return false;
        }

        protected override void OnKeyUp(KeyUpEvent e) {
            if (IsDisposed) {
                return;
            }

            if (!ShouldBeRemoved && KeyBeingHeld == e.Key) {
                KeyBeingHeld = null;
                ReleaseNote();
            }
        }

        public override bool UpdateNote() {
            // Removes if this note has been flagged for removal by Delete(). Removal has to be delayed for earliestNote check to work.  
            if (ShouldBeRemoved) {
                return true;
            }

            UpdateColor();
            UpdatePosition();
            UpdateScore();
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
            // Show time to End time
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
                Delete();
            }

            if (time >= HitTime && !IsBeingHeld()) {
                Colour = Color4.Red;
            }
        }

        private void UpdateScore() {
            var time = Time.Current;
            var notes = Story.Notes;
            if (time < notes.ShowTime - notes.FadeInTime) {
                State = HoldNoteState.NotVisibleBefore;
            } else if (time < HitTime) {
                State = HoldNoteState.VisibleBefore;
            } else if (time <= EndTime) {
                State = HoldNoteState.During;
            } else if (!ShouldBeRemoved) {
                State = HoldNoteState.VisibleAfter;
            } else {
                // So we don't add to score in the few ms between calling Delete() and the note actually being removed
                return;
            }
            var elapsed = (int)Math.Round(Time.Elapsed);
            if (IsHovered && IsBeingHeld()) {
                switch (State) {
                    case HoldNoteState.VisibleBefore:
                        ScoreBefore += elapsed;
                        ScoreInfo.AddScore(elapsed);
                        break;
                    case HoldNoteState.VisibleAfter:
                        ScoreAfter += elapsed;
                        ScoreInfo.AddScore(elapsed);
                        break;
                    default:
                        break;
                }
            } else if (State == HoldNoteState.During) {
                ScoreDuring += elapsed;
                ScoreInfo.AddScore(elapsed);
            }
        }
    }
}
