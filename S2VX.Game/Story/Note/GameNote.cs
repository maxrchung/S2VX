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
    public class GameNote : S2VXNote {
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

        private const int MissThreshold = 200;
        private int TimingError;

        private bool ShouldBeRemoved { get; set; }


        private void Delete() {
            PlayScreen.PlayInfoBar.RecordHitError(TimingError);
            if (Math.Abs(TimingError) < MissThreshold) {
                Hit.Play();
            } else {
                Miss.Play();
            }
            ShouldBeRemoved = true;
        }

        private void RecordMiss() {
            var missThreshold = MissThreshold;
            TimingError = missThreshold;
            ScoreInfo.AddScore(missThreshold);
            Delete();
        }

        // Notes are clickable if they are visible on screen, not missed, and is the earliest note
        private bool IsClickable() {
            var time = Time.Current;
            // Limit timing error to be +/- MissThreshold (though it will never be >= MissThreshold since RecordMiss would have already run)
            TimingError = (int)Math.Round(Math.Clamp(time - HitTime, -MissThreshold, MissThreshold));
            var inMissThreshold = TimingError <= MissThreshold && Alpha > 0;
            var earliestNote = Story.Notes.Children.Last();
            var isEarliestNote = earliestNote == this;
            return inMissThreshold && isEarliestNote;
        }

        private void ClickNote() {
            ScoreInfo.AddScore(Math.Abs(TimingError));
            Delete();
        }

        protected override bool OnMouseDown(MouseDownEvent e) {
            if (IsClickable()) {
                ClickNote();
            }

            return false;
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
                        break;
                    default:
                        break;
                }
            }

            return false;
        }

        public override bool UpdateNote() {
            // Removes if this note has been flagged for removal by Delete(). Removal has to be delayed for earliestNote check to work.  
            if (ShouldBeRemoved) {
                return true;
            }

            UpdateColor();
            UpdatePosition();

            if (Time.Current >= HitTime + MissThreshold) {
                RecordMiss();
            }
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
            // Show time to Hit time with miss threshold time
            // Hold the note at fully visible until after MissThreshold
            else if (time < HitTime + MissThreshold) {
                Alpha = 1;
            }
            // Hit time with miss threshold time to Fade out time
            else if (time < HitTime + MissThreshold + notes.FadeOutTime) {
                var startTime = HitTime + MissThreshold;
                var endTime = HitTime + MissThreshold + notes.FadeOutTime;
                Alpha = S2VXUtils.ClampedInterpolation(time, 1.0f, 0.0f, startTime, endTime);
            } else {
                Alpha = 0;
            }

            if (time >= HitTime) {
                Colour = Color4.Red;
            }
        }
    }
}
