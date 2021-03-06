using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Input.Bindings;
using osuTK.Graphics;
using S2VX.Game.Play;
using S2VX.Game.Play.UserInterface;
using System;
using System.Linq;

namespace S2VX.Game.Story.Note {
    public class GameHoldNote : HoldNote, IKeyBindingHandler<InputAction> {
        public S2VXSample Hit { get; private set; }
        public S2VXSample Miss { get; private set; }

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

        private const double MissThreshold = 200.0;
        private bool HitSoundHasBeenPlayed { get; set; }
        private bool ReleaseSoundHasBeenPlayed { get; set; }
        private double ScoreBefore { get; set; }
        private double ScoreDuring { get; set; }
        private double ScoreAfter { get; set; }
        private HoldNoteState State { get; set; } = HoldNoteState.NotVisibleBefore;
        private int InputsBeingHeld { get; set; }
        private bool IsFlaggedForRemoval { get; set; }
        private bool IsBeingHeld() => InputsBeingHeld > 0;
        public override bool HandlePositionalInput => true;

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            Hit = new S2VXSample("hit", audio);
            Miss = new S2VXSample("miss", audio);
        }

        private void FlagForRemoval() {
            ScoreBefore = Math.Clamp(ScoreBefore, 0, MissThreshold);
            ScoreAfter = Math.Clamp(ScoreAfter, 0, MissThreshold);
            var totalScore = ScoreBefore + ScoreDuring + ScoreAfter;
            PlayScreen.PlayInfoBar.RecordHitError((int)totalScore);
            IsFlaggedForRemoval = true;
        }

        // Note is clickable if in a visible state and is the earliest note
        private bool IsClickable() {
            var isVisible = State != HoldNoteState.NotVisibleBefore;
            var isEarliestNote = Story.Notes.Children.Last() == this;
            return isVisible && isEarliestNote;
        }

        private void HitNoteSound() {
            if (!HitSoundHasBeenPlayed) {
                var time = Time.Current;
                if (Math.Abs(HitTime - time) < MissThreshold) {
                    Hit.Play();
                } else {
                    Miss.Play();
                }
                HitSoundHasBeenPlayed = true;
            }
        }

        private void ReleaseNoteSound() {
            if (!IsBeingHeld() && !ReleaseSoundHasBeenPlayed) {
                var time = Time.Current;
                if (Math.Abs(EndTime - time) < MissThreshold) {
                    Hit.Play();
                } else {
                    Miss.Play();
                }
                ReleaseSoundHasBeenPlayed = true;
            }
        }

        public bool OnPressed(InputAction action) {
            if (IsClickable() && IsHovered) {
                ++InputsBeingHeld;
                HitNoteSound();
            }
            return false;
        }

        public void OnReleased(InputAction action) {
            if (!IsFlaggedForRemoval && IsBeingHeld()) {
                --InputsBeingHeld;
                ReleaseNoteSound();
            }
        }

        public override bool UpdateNote() {
            UpdateState();

            // Removes if this note has been flagged for removal by Delete(). Removal has to be delayed for earliestNote check to work.  
            if (IsFlaggedForRemoval) {
                return true;
            }

            UpdateColor();
            UpdatePosition();
            UpdateScore();
            return false;
        }

        private void UpdateState() {
            var time = Time.Current;
            var notes = Story.Notes;
            if (time < HitTime - notes.ShowTime - notes.FadeInTime) {
                State = HoldNoteState.NotVisibleBefore;
            } else if (time < HitTime) {
                State = HoldNoteState.VisibleBefore;
            } else if (time < EndTime) {
                State = HoldNoteState.During;
            } else if (time < EndTime + notes.FadeOutTime) {
                State = HoldNoteState.VisibleAfter;
            } else {
                FlagForRemoval();
            }
        }

        protected override void UpdateColor() {
            var time = Time.Current;
            var notes = Story.Notes;

            // Fade in time to Show time
            if (time < HitTime - notes.ShowTime) {
                var startTime = HitTime - notes.ShowTime - notes.FadeInTime;
                var endTime = HitTime - notes.ShowTime;
                Alpha = S2VXUtils.ClampedInterpolation(time, 0.0f, 1.0f, startTime, endTime);
            }
            // Show time to End time
            else if (time < EndTime) {
                Alpha = 1;
            }
            // End time to Fade out time
            else if (time < EndTime + notes.FadeOutTime) {
                var startTime = EndTime;
                var endTime = EndTime + notes.FadeOutTime;
                Alpha = S2VXUtils.ClampedInterpolation(time, 1.0f, 0.0f, startTime, endTime);
            } else {
                Alpha = 0;
            }

            if (time >= HitTime && !IsBeingHeld()) {
                Colour = Color4.Red;
            }
        }

        private void UpdateScore() {
            var elapsed = Time.Elapsed;
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
