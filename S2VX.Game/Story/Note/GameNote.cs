using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Input.Bindings;
using S2VX.Game.Play;
using S2VX.Game.Play.UserInterface;
using System;

namespace S2VX.Game.Story.Note {
    public class GameNote : S2VXNote, IKeyBindingHandler<PlayAction> {
        public S2VXSample Hit { get; private set; }
        public S2VXSample Miss { get; private set; }

        [Resolved]
        private ScoreInfo ScoreInfo { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [Resolved]
        private PlayScreen PlayScreen { get; set; }

        public const double MissThreshold = 200.0;
        private double TimingError;
        private bool IsFlaggedForRemoval { get; set; }
        public override bool HandlePositionalInput => true;

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            Hit = new S2VXSample("hit", audio);
            Miss = new S2VXSample("miss", audio);
        }

        private void FlagForRemoval() {
            if (IsFlaggedForRemoval) {
                throw new InvalidOperationException("Flagged for removal twice. Fix immediately.");
            }
            PlayScreen.HitErrorBar.RecordHitError((int)TimingError);
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
            // Has IsFlaggedForRemoval check to prevent race condition that can cause double flagging for removal.
            if (Story.Notes.HasClickedNote || IsFlaggedForRemoval) {
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

        public bool OnPressed(PlayAction action) {
            if (IsHovered && IsClickable() && action == PlayAction.Input) {
                ClickNote();
            }

            return false;
        }

        public void OnReleased(PlayAction action) { }

        public override bool UpdateNote() {
            // Tells notes.cs if this note has been flagged for removal.
            if (IsFlaggedForRemoval) {
                return true;
            }

            if (Time.Current >= HitTime + MissThreshold) {
                RecordMiss();
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
        }
    }
}
