using osu.Framework.Allocation;
using S2VX.Game.Story;

namespace S2VX.Game.Editor.Reversible {
    public class RemoveNoteReversible : IReversible {
        [Resolved]
        private S2VXStory Story { get; set; }

        private Note Note { get; }

        public RemoveNoteReversible(Note note) => Note = note;

        public void Undo() => Story.AddNote(Note.Position, Note.EndTime);

        public void Redo() => Story.RemoveNote(Note);
    }
}
