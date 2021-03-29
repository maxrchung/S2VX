using osu.Framework.Allocation;
using osu.Framework.Input.Bindings;
using S2VX.Game.Play;
using S2VX.Game.Play.UserInterface;
using System;

namespace S2VX.Game.Story.Note {
    public class GameNote : S2VXNote, IKeyBindingHandler<PlayAction> {

        [Resolved]
        private ScoreProcessor ScoreProcessor { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [Resolved]
        private PlayScreen PlayScreen { get; set; }

        private double TimingError { get; set; }
        private bool IsFlaggedForRemoval { get; set; }
        public override bool HandlePositionalInput => true;

        private void FlagForRemoval() {
            if (IsFlaggedForRemoval) {
                throw new InvalidOperationException("Flagged for removal twice. Fix immediately.");
            }
            PlayScreen.HitErrorBar.RecordHitError((int)Math.Round(TimingError));
            ScoreProcessor.Process(Time.Current, this);
            IsFlaggedForRemoval = true;
        }

        // Conditions for which a note can be clicked
        private bool IsClickable() {
            TimingError = Time.Current - HitTime;
            var isWithinMissThreshold = Math.Abs(TimingError) <= Story.Notes.MissThreshold;

            // Has IsFlaggedForRemoval check to prevent race condition that can cause double flagging for removal.
            return !Story.Notes.HasClickedNote && !IsFlaggedForRemoval && IsHovered && Alpha != 0 && isWithinMissThreshold;
        }

        public bool OnPressed(PlayAction action) {
            if (action == PlayAction.Input && IsClickable()) {
                Story.Notes.HasClickedNote = true;
                FlagForRemoval();
            }

            return false;
        }

        public void OnReleased(PlayAction action) { }

        public override bool UpdateNote() {
            // Tells Notes.cs if this note has been flagged for removal.
            if (IsFlaggedForRemoval) {
                return true;
            }

            // Remove note if after threshold
            if (Time.Current >= HitTime + Story.Notes.MissThreshold) {
                FlagForRemoval();
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
            else if (time < HitTime + notes.MissThreshold) {
                Alpha = 1;
            }
            // Hit time with miss threshold time to Fade out time
            else if (time < HitTime + notes.MissThreshold + notes.FadeOutTime) {
                var startTime = HitTime + notes.MissThreshold;
                var endTime = HitTime + notes.MissThreshold + notes.FadeOutTime;
                Alpha = S2VXUtils.ClampedInterpolation(time, 1.0f, 0.0f, startTime, endTime);
            } else {
                Alpha = 0;
            }
        }
    }
}
