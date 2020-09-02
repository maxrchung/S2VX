using osu.Framework.Allocation;
using S2VX.Game.Story;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleRemoveNote : IReversible {
        [Resolved]
        private S2VXStory Story { get; set; }

        private Note Note { get; }

        public ReversibleRemoveNote(Note note) => Note = note;

        public void Undo() => Story.AddNote(Note);

        public void Redo() => Story.RemoveNote(Note);
    }
}
