using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleRemoveNote : IReversible {
        private S2VXStory Story { get; set; }

        private S2VXNote Note { get; }

        public ReversibleRemoveNote(S2VXStory story, S2VXNote note) {
            Story = story;
            Note = note;
        }

        public void Undo() => Story.AddNote(Note);

        public void Redo() => Story.RemoveNote(Note);
    }
}
