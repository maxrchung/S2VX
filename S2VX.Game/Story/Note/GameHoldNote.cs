using osu.Framework.Allocation;
using osu.Framework.Input.Bindings;
using S2VX.Game.Play;
using S2VX.Game.Play.UserInterface;

namespace S2VX.Game.Story.Note {
    public class GameHoldNote : HoldNote, IKeyBindingHandler<PlayAction> {
        [Resolved]
        private ScoreProcessor ScoreProcessor { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [Resolved]
        private PlayScreen PlayScreen { get; set; }

        private bool IsHitProcessed { get; set; } // If multiple presses come within HitWindow, we penalize only the first press
        private double LastReleaseTime { get; set; }  // Holds the last time of a ReleaseDuring
        private bool IsEndScored { get; set; } // Process only first UpdateScore() called in VisibleAfter
        private double TotalScore { get; set; }
        private int InputsHeld { get; set; }

        private bool IsFlaggedForRemoval { get; set; }
        public override bool HandlePositionalInput => true;

        public GameHoldNote() => LastReleaseTime = HitTime;

        private void FlagForRemoval() {
            PlayScreen.HitErrorBar.RecordHitError(TotalScore);
            IsFlaggedForRemoval = true;
        }

        public bool OnPressed(PlayAction action) {
            var time = Time.Current;
            var notes = Story.Notes;

            if (action == PlayAction.Input && IsHovered && !notes.HasPressedNote && ++InputsHeld == 1) {
                if (time < HitTime + notes.MissThreshold && !IsHitProcessed) {
                    TotalScore += ScoreProcessor.ProcessHit(time, HitTime);
                    IsHitProcessed = true;

                } else {
                    TotalScore += ScoreProcessor.ProcessHold(time, LastReleaseTime, true, HitTime, EndTime);
                }

                notes.HasPressedNote = true;
            }
            return true;
        }

        public void OnReleased(PlayAction action) {
            // Only execute a Release if this is the last key being released
            if (action == PlayAction.Input && InputsHeld > 0 && --InputsHeld == 0) {
                if (Time.Current > HitTime) {
                    LastReleaseTime = Time.Current;
                }
            }
        }

        public override bool UpdateNote() {
            var time = Time.Current;
            var notes = Story.Notes;

            // Explicitly process end of hit
            if (time > HitTime + notes.MissThreshold && !IsHitProcessed) {
                TotalScore += ScoreProcessor.ProcessHit(time, HitTime);
                IsHitProcessed = true;
            }

            // Explicitly process end of hold
            if (time > EndTime && !IsEndScored) {
                TotalScore += InputsHeld > 0
                    ? ScoreProcessor.ProcessHold(EndTime, EndTime, true, HitTime, EndTime)
                    : ScoreProcessor.ProcessHold(EndTime, LastReleaseTime, false, HitTime, EndTime);
                IsEndScored = true;

            } else if (time > EndTime + notes.FadeOutTime) {
                FlagForRemoval();
            }

            // Tells notes.cs if this note has been flagged for removal.
            if (IsFlaggedForRemoval) {
                return true;
            }

            UpdateColor();
            UpdatePosition();
            return false;
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
        }
    }
}
