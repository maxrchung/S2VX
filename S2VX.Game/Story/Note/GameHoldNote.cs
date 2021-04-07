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

        private bool IsHitScored { get; set; } // If multiple presses come within HitWindow, we penalize only the first press
        private double LastReleaseTime { get; set; }  // Holds the last time of a ReleaseDuring
        private bool IsEndScored { get; set; } // Process only first UpdateScore() called in VisibleAfter
        private double TotalScore { get; set; }
        private int InputsHeld { get; set; }

        private bool IsFlaggedForRemoval { get; set; }
        public override bool HandlePositionalInput => true;

        [BackgroundDependencyLoader]
        private void Load() => LastReleaseTime = HitTime;

        private void FlagForRemoval() {
            PlayScreen.HitErrorBar.RecordHitError(TotalScore);
            IsFlaggedForRemoval = true;
        }

        public bool OnPressed(PlayAction action) {
            var time = Time.Current;
            var notes = Story.Notes;

            // We used to increment this as part of the if check below, but
            // there was a possible issue where the previous if conditions short
            // circuited the if check and never incremented InputsHeld. This
            // could then lead to an issue where OnReleased was decrementing
            // InputsHeld into the negatives.
            ++InputsHeld;

            if (action == PlayAction.Input && IsHovered && !notes.HasPressedNote && InputsHeld == 1 && !IsEndScored) {
                if (time < HitTime - notes.MissThreshold) { // Before early miss
                    return true;

                } else if (time <= HitTime && !IsHitScored) { // Before hit time
                    TotalScore += ScoreProcessor.ProcessHit(time, HitTime);
                    IsHitScored = true;

                } else { // After hit time
                    TotalScore += ScoreProcessor.ProcessHold(time, LastReleaseTime, true, HitTime, EndTime);
                    IsHitScored = true;
                }

                notes.HasPressedNote = true;
                System.Console.WriteLine("pressed");
            }

            // Always return false so that other notes can possibly be handled
            return false;
        }

        public void OnReleased(PlayAction action) {
            --InputsHeld;

            // Only execute a Release if this is the last key being released
            if (action == PlayAction.Input && InputsHeld > 0 && InputsHeld == 0) {
                var time = Time.Current;

                if (Time.Current > HitTime && !IsEndScored) {
                    ScoreProcessor.ProcessHold(time, LastReleaseTime, false, HitTime, EndTime);
                    LastReleaseTime = Time.Current;
                }
            }
        }

        public override bool UpdateNote() {
            var time = Time.Current;
            var notes = Story.Notes;

            if (time > HitTime + notes.MissThreshold && !IsHitScored && !IsEndScored) {
                // Explicitly process end of hit
                TotalScore += ScoreProcessor.ProcessHold(time, LastReleaseTime, false, HitTime, EndTime);
                IsHitScored = true;

                if (time > EndTime) {
                    IsEndScored = true;
                }
            }

            if (time > EndTime && !IsEndScored) {
                // Explicitly process end of hold
                if (!IsHitScored) {
                    TotalScore += ScoreProcessor.ProcessHold(EndTime, HitTime, false, HitTime, EndTime);
                } else if (InputsHeld > 0) {
                    TotalScore += ScoreProcessor.ProcessHold(EndTime, EndTime, true, HitTime, EndTime);
                } else {
                    TotalScore += ScoreProcessor.ProcessHold(EndTime, LastReleaseTime, false, HitTime, EndTime);
                }
                IsEndScored = true;

            }

            if (time > EndTime + notes.FadeOutTime) {
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
