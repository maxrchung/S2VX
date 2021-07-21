using osu.Framework.Allocation;
using osu.Framework.Input.Bindings;
using S2VX.Game.Play;
using S2VX.Game.Play.Score;
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
            PlayScreen.HitErrorBar.RecordHitError(TimingError);
            ScoreProcessor.ProcessHit(Time.Current, HitTime, Coordinates);
            IsFlaggedForRemoval = true;
        }

        // Conditions for which a note can be clicked
        private bool IsClickable() {
            TimingError = Time.Current - HitTime;
            var isWithinMissThreshold = Math.Abs(TimingError) <= Notes.MissThreshold;

            // Has IsFlaggedForRemoval check to prevent race condition that can cause double flagging for removal.
            return !Story.Notes.HasPressedNote && !IsFlaggedForRemoval && IsHovered && Alpha != 0 && isWithinMissThreshold;
        }

        public bool OnPressed(PlayAction action) {
            if (action == PlayAction.Input && IsClickable()) {
                Story.Notes.HasPressedNote = true;
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

            // Remove note if after miss threshold
            if (Time.Current >= HitTime + Notes.MissThreshold) {
                TimingError = Notes.MissThreshold;
                FlagForRemoval();
            }

            UpdateColor();
            UpdatePosition();
            return false;
        }

        protected override void UpdateColor() {
            var time = Time.Current;
            var notes = Story.Notes;
            var maxAlpha = notes.NoteAlpha;
            InnerColor = notes.NoteColor;
            OutlineColor = notes.NoteOutlineColor;
            OutlineThickness = notes.NoteOutlineThickness;
            // Fade in time to Show time
            if (time < HitTime - notes.ShowTime) {
                var startTime = HitTime - notes.ShowTime - notes.FadeInTime;
                var endTime = HitTime - notes.ShowTime;
                Alpha = S2VXUtils.ClampedInterpolation(time, 0.0f, maxAlpha, startTime, endTime);
            }
            // Show time to Hit time with miss threshold time
            // Hold the note at fully visible until after MissThreshold
            else if (time < HitTime + Notes.HitThreshold) {
                Alpha = maxAlpha;
            }
            // Hit time with miss threshold time to Fade out time
            else if (time < HitTime + Notes.HitThreshold + notes.FadeOutTime) {
                var startTime = HitTime + Notes.HitThreshold;
                var endTime = HitTime + Notes.HitThreshold + notes.FadeOutTime;
                Alpha = S2VXUtils.ClampedInterpolation(time, maxAlpha, 0.0f, startTime, endTime);
            } else {
                Alpha = 0;
            }
        }

        public override Approach AddApproach() {
            var approach = new GameApproach {
                Coordinates = Coordinates,
                HitTime = HitTime
            };
            Approach = approach;
            return approach;
        }
    }
}
