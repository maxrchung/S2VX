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
        private SampleChannel Hit { get; set; }
        private SampleChannel Miss { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            Hit = audio.Samples.Get("hit");
            Miss = audio.Samples.Get("miss");
        }

        [Resolved]
        private ScoreInfo ScoreInfo { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [Resolved]
        private PlayScreen PlayScreen { get; set; }

        private const int HitMissThreshold = 200;
        private const int ReleaseMissThreshold = 200;
        private int HitTimingError;
        private int ReleaseTimingError;

        private Key? KeyBeingHeld { get; set; }
        private MouseButton? MouseButtonBeingHeld { get; set; }
        private bool ShouldBeRemoved { get; set; }


        private void Delete() => ShouldBeRemoved = true;

        private void RecordMiss() {
            var missThreshold = HitMissThreshold;
            ScoreInfo.AddScore(missThreshold);
            PlayScreen.PlayInfoBar.RecordHitError(missThreshold);
            Miss.Play();
            Delete();
        }

        private bool IsBeingHeld() => KeyBeingHeld != null || MouseButtonBeingHeld != null;

        // Notes are clickable if there is nothing already holding it, are visible on screen, not missed, and is the earliest note
        private bool IsClickable() {
            var time = Time.Current;
            // Limit timing error to be +/- MissThreshold
            HitTimingError = (int)Math.Round(Math.Clamp(time - HitTime, -HitMissThreshold, HitMissThreshold));
            var inMissThreshold = HitTimingError <= HitMissThreshold && Alpha > 0;
            var earliestNote = Story.Notes.Children.Last();
            var isEarliestNote = earliestNote == this;
            return !IsBeingHeld() && inMissThreshold && isEarliestNote;
        }

        private void ClickNote() {
            ScoreInfo.AddScore(Math.Abs(HitTimingError));
            PlayScreen.PlayInfoBar.RecordHitError(HitTimingError);
            if (Math.Abs(HitTimingError) < HitMissThreshold) {
                Hit.Play();
            } else {
                Miss.Play();
            }
        }

        private void ReleaseNote() {
            var time = Time.Current;
            ReleaseTimingError = (int)Math.Round(Math.Clamp(time - EndTime, -ReleaseMissThreshold, ReleaseMissThreshold));
            ScoreInfo.AddScore(Math.Abs(ReleaseTimingError));
            if (Math.Abs(ReleaseTimingError) < ReleaseMissThreshold) {
                Hit.Play();
            } else {
                Miss.Play();
            }
            PlayScreen.PlayInfoBar.RecordHitError(ReleaseTimingError);
            Delete();
        }

        protected override bool OnMouseDown(MouseDownEvent e) {
            if (IsClickable()) {
                MouseButtonBeingHeld = e.Button;
                ClickNote();
            }

            return false;
        }

        /// <summary>
        /// Mouse up needs to have a dispose check because it's possible to
        /// leave the current screen and have the on mouse up trigger on the
        /// next screen.
        /// </summary>
        protected override void OnMouseUp(MouseUpEvent e) {
            if (IsDisposed) {
                return;
            }

            if (!ShouldBeRemoved && MouseButtonBeingHeld == e.Button) {
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
                        KeyBeingHeld = e.Key;
                        ClickNote();
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
                ReleaseNote();
            }
        }

        public override bool UpdateNote() {
            var time = Time.Current;
            var notes = Story.Notes;

            // Trigger release and delete if cursor is no longer over the HoldNote during the hold
            if (IsBeingHeld() && !IsHovered || time >= EndTime + ReleaseMissThreshold) {
                ReleaseNote();
            }

            // Removes if this note has been flagged for removal by Delete(). Removal has to be delayed for earliestNote check to work.  
            if (ShouldBeRemoved) {
                return true;
            }

            base.UpdateNote();

            UpdatePlacement();

            if (time >= HitTime && IsBeingHeld()) {
                Alpha = 1;
            } else if (time >= HitTime && !IsBeingHeld()) {
                // Hold the note at fully visible until after MissThreshold
                var startFadeTime = HitTime + HitMissThreshold;
                var endFadeTime = startFadeTime + notes.FadeOutTime;
                var alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, startFadeTime, endFadeTime);
                Alpha = alpha;
                Colour = Color4.Red;
                if (time >= HitTime + HitMissThreshold) {
                    RecordMiss();
                }
            }
            return false;
        }
    }
}
