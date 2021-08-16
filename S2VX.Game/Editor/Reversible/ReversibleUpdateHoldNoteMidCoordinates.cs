using osuTK;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleUpdateHoldNoteMidCoordinates : IReversible {
        private EditorHoldNote Note { get; }
        private Vector2 OldCoordinates { get; }
        private Vector2 NewCoordinates { get; }
        private int MidIndex { get; }

        public ReversibleUpdateHoldNoteMidCoordinates(EditorHoldNote note, Vector2 oldCoordinates, Vector2 newCoordinates, int midIndex) {
            Note = note;
            OldCoordinates = oldCoordinates;
            NewCoordinates = newCoordinates;
            MidIndex = midIndex;
        }

        public void Undo() => Note.UpdateMidCoordinates(OldCoordinates, MidIndex);

        public void Redo() => Note.UpdateMidCoordinates(NewCoordinates, MidIndex);
    }
}
