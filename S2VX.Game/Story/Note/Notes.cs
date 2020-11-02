using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;
using System.Collections.Generic;

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

        protected override void Update() {
            foreach (var note in Children) {
                var notes = Story.Notes;
                var camera = Story.Camera;
                var grid = Story.Grid;

                var time = Time.Current;
                var endFadeOut = note.EndTime + notes.FadeOutTime;

                if (time >= endFadeOut) {
                    note.Alpha = 0;
                    // Continue early to save some calculations
                    continue;
                }

                note.Rotation = camera.Rotation;
                note.Size = camera.Scale;

                var cameraFactor = 1 / camera.Scale.X;
                note.BoxOuter.Size = Vector2.One - cameraFactor * new Vector2(grid.Thickness);
                note.BoxInner.Size = note.BoxOuter.Size - 2 * cameraFactor * new Vector2(notes.OutlineThickness);


                note.Position = S2VXUtils.Rotate(note.Coordinates - camera.Position, note.Rotation) * note.Size.X;
                note.BoxOuter.Colour = notes.OutlineColor;

                var startTime = note.EndTime - notes.ShowTime;
                if (time >= note.EndTime) {
                    var alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, note.EndTime, endFadeOut);
                    note.Alpha = alpha;
                } else if (time >= startTime) {
                    note.Alpha = 1;
                } else {
                    var startFadeIn = startTime - notes.FadeInTime;
                    var alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startFadeIn, startTime);
                    note.Alpha = alpha;
                }
            }
        }

        [BackgroundDependencyLoader]
        private void Load() => RelativeSizeAxes = Axes.Both;
    }
}
