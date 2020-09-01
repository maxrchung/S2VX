using osu.Framework.Allocation;
using S2VX.Game.Story;
using System;

namespace S2VX.Game.Editor.Reversible {
    public class AddNoteReversible : IReversible {
        [Resolved]
        private S2VXStory Story { get; set; }

        private Note Note { get; }

        public AddNoteReversible(Note note) => Note = note;

        public void Undo() => Story.RemoveNote(Note);

        public void Redo() => Story.AddNote(Note.Position, Note.EndTime);
    }
}
