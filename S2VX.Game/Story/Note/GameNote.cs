using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Input.Bindings;
using osuTK.Graphics;
using S2VX.Game.Play;
using S2VX.Game.Play.UserInterface;
using System;

namespace S2VX.Game.Story.Note {
    public class GameNote : S2VXNote, IKeyBindingHandler<InputAction> {
        [Resolved]
        private ScoreInfo ScoreInfo { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [Resolved]
        private PlayScreen PlayScreen { get; set; }

        private Sample Hit { get; set; }
        private Sample Miss { get; set; }
        public const double MissThreshold = 200.0;
        private double TimingError;
        private bool IsFlaggedForRemoval { get; set; }
        public override bool HandlePositionalInput => true;

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            Hit = audio.Samples.Get("hit");
            Miss = audio.Samples.Get("miss");
        }

        private void FlagForRemoval() {
            PlayScreen.PlayInfoBar.RecordHitError((int)TimingError);
            if (Math.Abs(TimingError) < MissThreshold) {
                Hit.Play();
            } else {
                Miss.Play();
            }
            IsFlaggedForRemoval = true;
        }

        private void RecordMiss() {
            var missThreshold = MissThreshold;
            TimingError = missThreshold;
            ScoreInfo.AddScore(missThreshold);
            FlagForRemoval();
        }

        // Notes are clickable if they are hovered, not missed, and is the earliest note
        private bool IsClickable() {
            if (Story.Notes.HasClickedNote) {
                return false;
            }
            var time = Time.Current;
            var isVisible = Alpha > 0;
            var isWithinMissThreshold = Math.Abs(time - HitTime) <= MissThreshold;
            if (!isVisible || !isWithinMissThreshold) {
                return false;
            }
            TimingError = time - HitTime;
            return true;
        }

        private void ClickNote() {
            ScoreInfo.AddScore(Math.Abs(TimingError));
            Story.Notes.HasClickedNote = true;
            FlagForRemoval();
        }

        public bool OnPressed(InputAction action) {
            if (IsHovered && IsClickable()) {
                ClickNote();
            }

            return false;
        }

        public void OnReleased(InputAction action) { }

        public override bool UpdateNote() {
            if (Time.Current >= HitTime + MissThreshold) {
                RecordMiss();
            }

            // Removes if this note has been flagged for removal by Delete(). Removal has to be delayed for earliestNote check to work.  
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
