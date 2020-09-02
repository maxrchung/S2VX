using osuTK;
using S2VX.Game.Story;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleUpdateNoteCoordinates : IReversible {
        private Note Note { get; }
        private Vector2 OldCoordinates { get; }
        private Vector2 NewCoordinates { get; }

        public ReversibleUpdateNoteCoordinates(Note note, Vector2 coordinates) {
            Note = note;
            OldCoordinates = note.Coordinates;
            NewCoordinates = coordinates;
        }

        public void Undo() => Note.UpdateCoordinates(OldCoordinates);

        public void Redo() => Note.UpdateCoordinates(NewCoordinates);
    }
}
