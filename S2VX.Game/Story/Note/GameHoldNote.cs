using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Input.Bindings;
using osuTK.Graphics;
using S2VX.Game.Play;
using S2VX.Game.Play.UserInterface;
using System;
using System.Linq;

namespace S2VX.Game.Story.Note {
    public class GameHoldNote : HoldNote, IKeyBindingHandler<InputAction> {
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
        private Sample Hit { get; set; }
        private Sample Miss { get; set; }
        private bool HitSoundHasBeenPlayed { get; set; }
        private bool ReleaseSoundHasBeenPlayed { get; set; }
        private int ScoreBefore { get; set; }
        private int ScoreDuring { get; set; }
        private int ScoreAfter { get; set; }
        private HoldNoteState State { get; set; } = HoldNoteState.NotVisibleBefore;
        private int InputsBeingHeld { get; set; }
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

        private bool IsBeingHeld() => InputsBeingHeld == 0;

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

        public override bool HandlePositionalInput => true;

        public bool OnPressed(InputAction action) {
            if (IsClickable() && IsHovered) {
                HitNoteSound();
                ++InputsBeingHeld;
            }
            return false;
        }

        public void OnReleased(InputAction action) {
            if (!ShouldBeRemoved && IsBeingHeld()) {
                --InputsBeingHeld;
                ReleaseNoteSound();
            }
        }

        public override bool UpdateNote() {
            UpdateState();

            // Removes if this note has been flagged for removal by Delete(). Removal has to be delayed for earliestNote check to work.  
            if (ShouldBeRemoved) {
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
            } else if (time <= EndTime) {
                State = HoldNoteState.During;
            } else if (!ShouldBeRemoved) {
                State = HoldNoteState.VisibleAfter;
            } else {
                Delete();
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
