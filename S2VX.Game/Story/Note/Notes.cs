using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using System.Collections.Generic;

namespace S2VX.Game.Story.Note {
    public class Notes : CompositeDrawable {
        public List<S2VXNote> Children { get; private set; } = new List<S2VXNote>();
        public void SetChildren(List<S2VXNote> notes) => Children = notes;

        // Notes fade in, show for a period of time, then fade out
        // The note should be hit at the very end of the show time
        public float FadeInTime { get; set; } = 100;
        public float ShowTime { get; set; } = 1000;
        public float FadeOutTime { get; set; } = 100;

        public void AddNote(S2VXNote note) {
            Children.Add(note);
            AddInternal(note);
        }

        public void RemoveNote(S2VXNote note) {
            Children.Remove(note);
            RemoveInternal(note);
        }

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = Children;
        }
    }
}
