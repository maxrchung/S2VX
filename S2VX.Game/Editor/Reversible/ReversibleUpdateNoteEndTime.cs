using S2VX.Game.Story;
using System.Collections.Generic;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleUpdateNoteEndTime : IReversible {
        private S2VXNote Note { get; }
        private double OldEndTime { get; }
        private double NewEndTime { get; }
        private Dictionary<S2VXNote, double> SelectedNoteToTime { get; } = new Dictionary<S2VXNote, double>();

        public ReversibleUpdateNoteEndTime(S2VXNote note, double oldEndTime, double newEndTime, Dictionary<S2VXNote, double> selectedNoteToTime) {
            Note = note;
            OldEndTime = oldEndTime;
            NewEndTime = newEndTime;
            SelectedNoteToTime = selectedNoteToTime;
        }

        public void Undo() {
            Note.UpdateEndTime(OldEndTime);
            SelectedNoteToTime[Note] = OldEndTime;
        }

        public void Redo() {
            Note.UpdateEndTime(NewEndTime);
            SelectedNoteToTime[Note] = NewEndTime;
        }
    }
}
