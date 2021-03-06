﻿using osu.Framework.Allocation;
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

        public enum HoldNoteState {
            NotVisible,
            VisibleBefore,
            HitWindow,
            During,
            VisibleAfter
        }

        public enum Action {
            None,
            Press,
            ReleaseHitWindow,
            ReleaseDuring   // Differentiated from ReleaseHitWindow since score penalities will be based only on releases in During
        }

        public const double MissThreshold = 200.0;
        public HoldNoteState State { get; private set; } = HoldNoteState.NotVisible;
        public Action LastAction { get; set; } = Action.None;
        private bool HasBeenPressedInHitWindow { get; set; } // If multiple presses come within HitWindow, we penalize only the first press
        private double LastReleaseDuringTime { get; set; }  // Holds the last time of a ReleaseDuring
        private bool EndTimeHasBeenScored { get; set; } // Process only first UpdateScore() called in VisibleAfter
        private double TotalScore { get; set; }
        private int InputsBeingHeld { get; set; }
        private bool IsBeingHeld() => InputsBeingHeld > 0;

        private Sample Hit { get; set; }
        private Sample Miss { get; set; }
        private bool HitSoundHasBeenPlayed { get; set; }
        private bool ReleaseSoundHasBeenPlayed { get; set; }

        private bool IsFlaggedForRemoval { get; set; }
        public override bool HandlePositionalInput => true;

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            Hit = audio.Samples.Get("hit");
            Miss = audio.Samples.Get("miss");
        }

        private void FlagForRemoval() {
            UpdateScore();
            PlayScreen.PlayInfoBar.RecordHitError((int)TotalScore);
            IsFlaggedForRemoval = true;
        }

        // Note is clickable in the HitWindow and During states and is the earliest note
        private bool IsClickable() {
            var clickableState = State == HoldNoteState.HitWindow || State == HoldNoteState.During;
            var isEarliestNote = Story.Notes.Children.Last() == this;
            return clickableState && isEarliestNote;
        }

        private void HitNoteSound() {
            if (!HitSoundHasBeenPlayed) {
                if (Math.Abs(HitTime - Time.Current) < MissThreshold) {
                    Hit.Play();
                } else {
                    Miss.Play();
                }
                HitSoundHasBeenPlayed = true;
            }
        }

        private void ReleaseNoteSound() {
            if (!IsBeingHeld() && !ReleaseSoundHasBeenPlayed) {
                if (Math.Abs(EndTime - Time.Current) < MissThreshold) {
                    Hit.Play();
                } else {
                    Miss.Play();
                }
                ReleaseSoundHasBeenPlayed = true;
            }
        }

        public bool OnPressed(InputAction action) {
            if (IsClickable() && IsHovered && ++InputsBeingHeld == 1) { // Only execute a Press if this is the first key being held
                LastAction = Action.Press;
                UpdateScore();
                HitNoteSound();
            }
            return false;
        }

        public void OnReleased(InputAction action) {
            if (!IsFlaggedForRemoval && IsBeingHeld() && --InputsBeingHeld == 0) { // Only execute a Release if this is the last key being released
                if (State == HoldNoteState.HitWindow) {
                    LastAction = Action.ReleaseHitWindow;
                } else if (State == HoldNoteState.During) {
                    LastAction = Action.ReleaseDuring;
                    LastReleaseDuringTime = Time.Current;
                }
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
            return false;
        }

        private void UpdateState() {
            var time = Time.Current;
            var notes = Story.Notes;
            if (time < HitTime - notes.ShowTime - notes.FadeInTime) {
                State = HoldNoteState.NotVisible;
            } else if (time >= HitTime - MissThreshold && time <= HitTime) {
                // HitWindow comes first in logic since it may overshadow VisibleBefore
                State = HoldNoteState.HitWindow;
            } else if (time < HitTime - MissThreshold) {
                State = HoldNoteState.VisibleBefore;
            } else if (time <= EndTime) {
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

        // There are three times when this can get called
        // 1. First time press in HitWindow
        // 2. Press in During
        // 3. When the note is flagged for removal
        private void UpdateScore() {
            var time = Time.Current;
            switch (State) {
                case HoldNoteState.HitWindow:
                    if (!HasBeenPressedInHitWindow) {
                        HasBeenPressedInHitWindow = true;
                        TotalScore += HitTime - time;
                        ScoreInfo.AddScore(HitTime - time);
                    }
                    break;
                case HoldNoteState.During:
                    if (LastAction == Action.ReleaseDuring) {
                        // Penalize gap between now and previous release
                        TotalScore += time - LastReleaseDuringTime;
                        ScoreInfo.AddScore(time - LastReleaseDuringTime);
                    } else {
                        // Late hold, penalize gap between now and HitTime
                        TotalScore += time - HitTime;
                        ScoreInfo.AddScore(time - HitTime);
                    }
                    break;
                case HoldNoteState.VisibleAfter:
                    if (!EndTimeHasBeenScored) {
                        EndTimeHasBeenScored = true;
                        switch (LastAction) {
                            case Action.None: // There was never any action, entire hold note was missed
                            case Action.ReleaseHitWindow: // Entire duration of the hold was missed (player mistakened this as a regular note and just tapped)
                                TotalScore += EndTime - HitTime;
                                ScoreInfo.AddScore(EndTime - HitTime);
                                break;
                            case Action.ReleaseDuring: // Early release
                                TotalScore += EndTime - LastReleaseDuringTime;
                                ScoreInfo.AddScore(EndTime - LastReleaseDuringTime);
                                break;
                            case Action.Press: // There was no early release, no action is needed
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
