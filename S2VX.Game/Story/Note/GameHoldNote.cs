using osu.Framework.Allocation;
using osu.Framework.Input.Bindings;
using S2VX.Game.Play;
using S2VX.Game.Play.UserInterface;
using System;

namespace S2VX.Game.Story.Note {
    public class GameHoldNote : HoldNote, IKeyBindingHandler<PlayAction> {
        [Resolved]
        private ScoreProcessor ScoreProcessor { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [Resolved]
        private PlayScreen PlayScreen { get; set; }

        private enum Action {
            None,
            Press,
            Release
        }

        public HoldNoteState State { get; private set; } = HoldNoteState.NotVisible;
        private Action LastAction { get; set; } = Action.None;
        private bool IsHitScored { get; set; } // If multiple presses come within HitWindow, we penalize only the first press

        /// <summary>
        /// Used during hold score calculation to reference the last pressed time
        /// It can be:<br/>
        ///     - Set to HitTime: By default<br/>
        ///     - Set to HitTime + MissThreshold: When you reach HitTime + MissThreshold without hitting<br/>
        ///     - Set in OnReleased
        /// </summary>
        private double LastHoldReferenceTime { get; set; }
        private bool IsEndScored { get; set; } // Process only first UpdateScore() called in VisibleAfter
        private double TotalScore { get; set; }
        private int InputsHeld { get; set; }

        private bool IsFlaggedForRemoval { get; set; }
        public override bool HandlePositionalInput => true;

        [BackgroundDependencyLoader]
        private void Load() => LastHoldReferenceTime = HitTime;

        private void FlagForRemoval() {
            PlayScreen.HitErrorBar.RecordHitError((int)Math.Round(TotalScore));
            IsFlaggedForRemoval = true;
        }

        // Note is clickable if in a visible state and has not been clicked yet
        private bool IsClickable() {
            if (Story.Notes.HasPressedNote) {
                return false;
            }
            var clickableState = State is HoldNoteState.HitWindow or HoldNoteState.During;
            return clickableState;

        }

        public bool OnPressed(PlayAction action) {
            if (IsHovered && IsClickable() && ++InputsHeld == 1) {
                LastAction = Action.Press;
                Story.Notes.HasPressedNote = true;
                ProcessPressedScore();
            }
            return false;
        }

        public void OnReleased(PlayAction action) {
            if (!IsFlaggedForRemoval && InputsHeld > 0 && --InputsHeld == 0) { // Only execute a Release if this is the last key being released
                LastAction = Action.Release;
                if (State == HoldNoteState.During) {
                    LastHoldReferenceTime = Time.Current;
                }
                ProcessReleasedScore();
            }
        }

        public override bool UpdateNote() {
            UpdateState();
            ProcessTimedScore();

            // Tells notes.cs if this note has been flagged for removal.
            if (IsFlaggedForRemoval) {
                return true;
            }

            UpdateColor();
            UpdatePosition();
            return false;
        }

        private void UpdateState() {
            var time = Time.Current;
            var notes = Story.Notes;
            if (time < HitTime - notes.ShowTime - notes.FadeInTime) {
                State = HoldNoteState.NotVisible;

            } else if (time >= HitTime - notes.MissThreshold && time <= HitTime) {
                // HitWindow comes first in logic since it may overshadow VisibleBefore
                State = HoldNoteState.HitWindow;

            } else if (time < HitTime - notes.MissThreshold) {
                State = HoldNoteState.VisibleBefore;
            } else if (time < EndTime) {
                State = HoldNoteState.During;
            } else if (time < EndTime + notes.FadeOutTime) {
                State = HoldNoteState.VisibleAfter;
            } else {
                State = HoldNoteState.VisibleAfter;  // This is needed for tests that skip past FadeOutTime
                FlagForRemoval();
            }
        }

        protected override void UpdateColor() {
            var time = Time.Current;
            var notes = Story.Notes;
            var maxAlpha = notes.HoldNoteAlpha;
            InnerColor = notes.HoldNoteColor;
            OutlineColor = notes.HoldNoteOutlineColor;
            OutlineThickness = notes.HoldNoteOutlineThickness;
            // Fade in time to Show time
            if (time < HitTime - notes.ShowTime) {
                var startTime = HitTime - notes.ShowTime - notes.FadeInTime;
                var endTime = HitTime - notes.ShowTime;
                Alpha = S2VXUtils.ClampedInterpolation(time, 0.0f, maxAlpha, startTime, endTime);
            }
            // Show time to End time
            else if (time < EndTime) {
                Alpha = maxAlpha;
            }
            // End time to Fade out time
            else if (time < EndTime + notes.FadeOutTime) {
                var startTime = EndTime;
                var endTime = EndTime + notes.FadeOutTime;
                Alpha = S2VXUtils.ClampedInterpolation(time, maxAlpha, 0.0f, startTime, endTime);
            } else {
                Alpha = 0;
            }
        }

        private void ProcessPressedScore() {
            var time = Time.Current;
            switch (State) {
                case HoldNoteState.HitWindow:
                    if (!IsHitScored) {
                        IsHitScored = true;
                        TotalScore += ScoreProcessor.ProcessHit(time, HitTime);
                    }
                    break;
                case HoldNoteState.During:
                    if (!IsHitScored) {
                        IsHitScored = true;
                        TotalScore += ScoreProcessor.ProcessHit(time, HitTime);
                    } else {
                        TotalScore += ScoreProcessor.ProcessHold(time, LastHoldReferenceTime, true, HitTime, EndTime);
                    }
                    break;
                default: // Should never get here
                    break;
            }
        }

        private void ProcessReleasedScore() {
            var time = Time.Current;
            switch (State) {
                case HoldNoteState.During:
                    // No need to update TotalTime here since this does not increase score
                    ScoreProcessor.ProcessHold(time, LastHoldReferenceTime, false, HitTime, EndTime);
                    break;
                default: // Should never get here
                    break;
            }
        }

        private void ProcessTimedScore() {
            var time = Time.Current;
            var notes = Story.Notes;
            switch (State) {
                case HoldNoteState.During:
                    // Explicitly handles a miss if a player fails to press the hit note
                    var missTime = HitTime + notes.MissThreshold;
                    if (!IsHitScored && time > missTime) {
                        IsHitScored = true;
                        TotalScore += ScoreProcessor.ProcessHit(missTime, HitTime);
                        LastHoldReferenceTime = missTime;
                    }
                    break;
                case HoldNoteState.VisibleAfter:
                    if (!IsEndScored) {
                        IsEndScored = true;
                        switch (LastAction) {
                            case Action.None: // There was never any action, entire hold note was missed
                            case Action.Release: // Early release
                                TotalScore += ScoreProcessor.ProcessHold(EndTime, LastHoldReferenceTime, false, HitTime, EndTime);
                                break;
                            case Action.Press: // There was no early release, no scoring is needed
                                ScoreProcessor.ProcessHold(EndTime, LastHoldReferenceTime, true, HitTime, EndTime);
                                break;
                        }
                    }
                    break;
                default: // Should never get here
                    break;
            }
        }
    }
}
