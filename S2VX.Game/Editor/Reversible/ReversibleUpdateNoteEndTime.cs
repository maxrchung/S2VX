using S2VX.Game.Story;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleUpdateNoteEndTime : IReversible {
        private Note Note { get; }
        private double OldEndTime { get; }
        private double NewEndTime { get; }

        public ReversibleUpdateNoteEndTime(Note note, double endTime) {
            Note = note;
            OldEndTime = note.EndTime;
            NewEndTime = endTime;
        }

        public void Undo() => Note.UpdateEndTime(OldEndTime);

        public void Redo() => Note.UpdateEndTime(NewEndTime);
    }
}
