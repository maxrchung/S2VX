using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
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

        // Before starting the Story in the PlayScreen, we want to explicitly
        // remove GameNotes up to some certain track time. This is so that we
        // won't hear Miss hitsounds and prematurely calculate score.
        public void RemoveNotesUpTo(double trackTime) {
            int validIndex;
            for (validIndex = 0; validIndex < Children.Count; ++validIndex) {
                if (Children[validIndex].EndTime < trackTime) {
                    break;
                }
            }

            var newNotes = Children.Take(validIndex).ToList();
            Children = newNotes;
            ClearInternal(false);
            InternalChildren = Children;
        }

        [BackgroundDependencyLoader]
        private void Load() => RelativeSizeAxes = Axes.Both;
    }
}
