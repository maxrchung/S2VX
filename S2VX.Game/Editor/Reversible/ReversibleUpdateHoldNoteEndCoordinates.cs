using osuTK;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleUpdateHoldNoteEndCoordinates : IReversible {
        private EditorHoldNote Note { get; }
        private Vector2 OldCoordinates { get; }
        private Vector2 NewCoordinates { get; }

        public ReversibleUpdateHoldNoteEndCoordinates(EditorHoldNote note, Vector2 oldCoordinates, Vector2 newCoordinates) {
            Note = note;
            OldCoordinates = oldCoordinates;
            NewCoordinates = newCoordinates;
        }

        public void Undo() => Note.UpdateEndCoordinates(OldCoordinates);

        public void Redo() => Note.UpdateEndCoordinates(NewCoordinates);
    }
}
