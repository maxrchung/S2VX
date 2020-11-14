using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Utils;
using osuTK.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace S2VX.Game.Story.Note {
    public class Notes : CompositeDrawable {

        public List<S2VXNote> Children { get; private set; } = new List<S2VXNote>();
        public void SetChildren(List<S2VXNote> notes) {
            Children = notes;
            InternalChildren = Children;
        }

        [Resolved]
        private S2VXStory Story { get; set; }

        // Notes fade in, show for a period of time, then fade out
        // The note should be hit at the very end of the show time
        public float FadeInTime { get; set; }
        public float ShowTime { get; set; }
        public float FadeOutTime { get; set; }
        public float OutlineThickness { get; set; }
        public Color4 OutlineColor { get; set; }

        public void AddNote(S2VXNote note) {
            Children.Add(note);
            Sort();
        }

        public void Sort() {
            Children.Sort();
            ClearInternal(false);
            InternalChildren = Children;
        }

        public void RemoveNote(S2VXNote note) {
            Children.Remove(note);
            RemoveInternal(note);
        }

        private void UpdateEditorHoldNote(EditorHoldNote holdNote) {
            // For EditorHold notes, override alpha between HitTime and EndTime
            var notes = Story.Notes;
            var camera = Story.Camera;
            var time = Time.Current;
            var endFadeOut = holdNote.EndTime + notes.FadeOutTime;
            var startTime = holdNote.HitTime - notes.ShowTime;

            holdNote.Rotation = camera.Rotation;
            holdNote.Size = camera.Scale;

            if (time >= endFadeOut) {
                holdNote.Alpha = 0;
                // Return early to save some calculations
                return;
            }

            holdNote.UpdatePlacement(camera, notes);

            if (time >= holdNote.EndTime) {
                var alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, holdNote.EndTime, endFadeOut);
                holdNote.Alpha = alpha;
            } else if (time >= startTime) {
                holdNote.Alpha = 1;
            }

            // Deduct number of hit sounds to play once we've passed each HitSoundTime
            if (holdNote.NumHitSounds > 0 && time >= holdNote.HitSoundTimes[^holdNote.NumHitSounds]) {
                --holdNote.NumHitSounds;
                holdNote.Hit.Play();
            }

            // Reset hit sound counter if clock is running and before timing points
            if (Clock.IsRunning) {
                holdNote.NumHitSounds = holdNote.HitSoundTimes.Count - holdNote.GetNumTimingPointsPassed();
            }
        }

        private void UpdateNote(S2VXNote note) {
            var notes = Story.Notes;
            var camera = Story.Camera;
            var time = Time.Current;
            var endFadeOut = note.HitTime + notes.FadeOutTime;

            note.Rotation = camera.Rotation;
            note.Size = camera.Scale;

            if (time >= endFadeOut) {
                note.Alpha = 0;
                // Return early to save some calculations
                return;
            }

            note.UpdatePlacement(camera, notes);

            var startTime = note.HitTime - notes.ShowTime;
            if (time >= note.HitTime) {
                var alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, note.HitTime, endFadeOut);
                note.Alpha = alpha;
            } else if (time >= startTime) {
                note.Alpha = 1;
            } else {
                var startFadeIn = startTime - notes.FadeInTime;
                var alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startFadeIn, startTime);
                note.Alpha = alpha;
            }
        }

        protected override void Update() {
            foreach (var note in Children) {
                if (note is EditorHoldNote holdNote) {
                    UpdateEditorHoldNote(holdNote);
                } else {
                    UpdateNote(note);
                }
            }
        }

        public List<S2VXNote> GetNonHoldNotes() => Children.Where(note => !(note is HoldNote)).ToList();

        public List<HoldNote> GetHoldNotes() => Children.OfType<HoldNote>().ToList();

        [BackgroundDependencyLoader]
        private void Load() => RelativeSizeAxes = Axes.Both;
    }
}
