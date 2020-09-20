using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleAddNote : IReversible {
        private S2VXStory Story { get; set; }

        private S2VXNote Note { get; }

        public ReversibleAddNote(S2VXStory story, S2VXNote note) {
            Story = story;
            Note = note;
        }

        public void Undo() => Story.RemoveNote(Note);

        public void Redo() => Story.AddNote(Note);
    }
}
