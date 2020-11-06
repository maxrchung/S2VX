using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleAddHoldNote : IReversible {
        private S2VXStory Story { get; set; }

        private HoldNote Note { get; }

        public ReversibleAddHoldNote(S2VXStory story, HoldNote note) {
            Story = story;
            Note = note;
        }

        public void Undo() => Story.RemoveNote(Note);

        public void Redo() => Story.AddNote(Note);
    }
}
