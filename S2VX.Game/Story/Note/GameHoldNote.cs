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

        private bool IsBeingHeld { get; set; }
        private bool ShouldBeRemoved { get; set; }


        private void Delete() => ShouldBeRemoved = true;

        private void RecordMiss() {
            var missThreshold = HitMissThreshold;
            ScoreInfo.AddScore(missThreshold);
            PlayScreen.PlayInfoBar.RecordHitError(missThreshold);
            Miss.Play();
            Delete();
        }

        // Notes are clickable if they are visible on screen, not missed, and is the earliest note
        private bool IsClickable() {
            var time = Time.Current;
            // Limit timing error to be +/- MissThreshold
            HitTimingError = (int)Math.Round(Math.Clamp(time - HitTime, -HitMissThreshold, HitMissThreshold));
            var inMissThreshold = HitTimingError <= HitMissThreshold && Alpha > 0;
            var earliestNote = Story.Notes.Children.Last();
            var isEarliestNote = earliestNote == this;
            return inMissThreshold && isEarliestNote;
        }

        private void ClickNote() {
            ScoreInfo.AddScore(Math.Abs(HitTimingError));
            PlayScreen.PlayInfoBar.RecordHitError(HitTimingError);
            if (Math.Abs(HitTimingError) < HitMissThreshold) {
                Hit.Play();
            } else {
                Miss.Play();
            }
            IsBeingHeld = true;
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
            if (!IsBeingHeld && IsClickable()) {
                ClickNote();
            }

            return false;
        }

        protected override void OnMouseUp(MouseUpEvent e) {
            if (!ShouldBeRemoved && IsBeingHeld) {
                ReleaseNote();
            }
        }

        protected override bool OnKeyDown(KeyDownEvent e) {
            if (!IsBeingHeld && IsClickable() && IsHovered) {
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
                        break;
                    default:
                        break;
                }
            }

            return false;
        }

        protected override void OnKeyUp(KeyUpEvent e) {
            if (!ShouldBeRemoved && IsBeingHeld) {
                ReleaseNote();
            }
        }

        protected override void Update() {
            var time = Time.Current;
            var notes = Story.Notes;

            // Trigger release and delete if mouse is no longer over the HoldNote during the hold
            if (IsBeingHeld && !IsHovered || time >= EndTime + ReleaseMissThreshold) {
                ReleaseNote();
            }

            // Removes if this note has been flagged for removal by Delete(). Removal has to be delayed for earliestNote check to work.  
            if (ShouldBeRemoved) {
                Story.RemoveNote(this);
                return;
            }

            base.Update();

            UpdatePlacement();

            if (time >= HitTime && IsBeingHeld) {
                Alpha = 1;
            } else if (time >= HitTime && !IsBeingHeld) {
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
        }
    }
}
