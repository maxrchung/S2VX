using S2VX.Game.Story;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleAddNote : IReversible {
        private S2VXStory Story { get; set; }

        private Note Note { get; }

        public ReversibleAddNote(S2VXStory story, Note note) {
            Story = story;
            Note = note;
        }

        public void Undo() => Story.RemoveNote(Note);

        public void Redo() => Story.AddNote(Note);
    }
}
