using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using System.Collections.Generic;

namespace S2VX.Game.Story {
    public class Notes : CompositeDrawable {
        public List<Note> Children { get; private set; } = new List<Note>();
        public void SetChildren(List<Note> notes) => Children = notes;

        // Notes fade in, show for a period of time, then fade out
        // The note should be hit at the very end of the show time
        public float FadeInTime { get; set; } = 100;
        public float ShowTime { get; set; } = 1000;
        public float FadeOutTime { get; set; } = 100;
        public float ApproachDistance { get; set; } = 0.5f;
        public float ApproachThickness { get; set; } = 0.005f;

        public void AddNote(Vector2 position, double time) {
            var note = new Note {
                Coordinates = position,
                EndTime = time
            };
            Children.Add(note);
            AddInternal(note);
        }

        public void DeleteNote(Note note) {
            Children.Remove(note);
            RemoveInternal(note);
        }

        [BackgroundDependencyLoader]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = Children;
        }
    }
}
