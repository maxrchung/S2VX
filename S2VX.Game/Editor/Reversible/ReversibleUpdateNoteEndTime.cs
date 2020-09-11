using S2VX.Game.Story;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleUpdateNoteEndTime : IReversible {
        private Note Note { get; }
        private double OldEndTime { get; }
        private double NewEndTime { get; }

        public ReversibleUpdateNoteEndTime(Note note, double oldEndTime, double newEndTime) {
            Note = note;
            OldEndTime = oldEndTime;
            NewEndTime = newEndTime;
        }

        public void Undo() {
            Note.UpdateEndTime(OldEndTime);
        }

        public void Redo() {
            Note.UpdateEndTime(NewEndTime);
            //SelectedNoteToTime[Note] = NewEndTime;
        }
    }
}
