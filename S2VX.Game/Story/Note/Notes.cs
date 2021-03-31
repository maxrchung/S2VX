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

        [Resolved]
        private S2VXStory Story { get; set; }

        // Notes fade in, show for a period of time, then fade out
        // The note should be hit at the very end of the show time
        public float FadeInTime { get; set; }
        public float ShowTime { get; set; }
        public float FadeOutTime { get; set; }

        // Used by Note and HoldNote Tools, set by Commands
        public Color4 PreviewNoteColor { get; set; }
        public Color4 PreviewHoldNoteColor { get; set; }

        // TODO: Remove these defaults and use command classes
        public Color4 PerfectColor { get; set; } = S2VXColorConstants.LightYellow;
        public Color4 EarlyColor { get; set; } = S2VXColorConstants.White;
        public Color4 LateColor { get; set; } = S2VXColorConstants.DarkYellow;
        public Color4 MissColor { get; set; } = S2VXColorConstants.Red;
        public float PerfectThreshold { get; set; } = 30;
        public float HitThreshold { get; set; } = 100;
        public float MissThreshold { get; set; } = 200;

        public bool HasClickedNote { get; set; }

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
            HasClickedNote = false;
            var notesToRemove = new List<S2VXNote>();

            foreach (var note in Children) {
                if (note.UpdateNote()) {
                    notesToRemove.Add(note);
                }
            }

            foreach (var note in notesToRemove) {
                Story.RemoveNote(note);
            }
        }

        public List<S2VXNote> GetNonHoldNotes() => Children.Where(note => note is not HoldNote).ToList();

        public List<HoldNote> GetHoldNotes() => Children.OfType<HoldNote>().ToList();

        [BackgroundDependencyLoader]
        private void Load() => RelativeSizeAxes = Axes.Both;
    }
}
