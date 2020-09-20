using osuTK;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleUpdateNoteCoordinates : IReversible {
        private S2VXNote Note { get; }
        private Vector2 OldCoordinates { get; }
        private Vector2 NewCoordinates { get; }

        public ReversibleUpdateNoteCoordinates(S2VXNote note, Vector2 oldCoordinates, Vector2 newCoordinates) {
            Note = note;
            OldCoordinates = oldCoordinates;
            NewCoordinates = newCoordinates;
        }

        public void Undo() => Note.UpdateCoordinates(OldCoordinates);

        public void Redo() => Note.UpdateCoordinates(NewCoordinates);
    }
}
