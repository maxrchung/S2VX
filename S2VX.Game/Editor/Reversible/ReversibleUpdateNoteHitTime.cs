using S2VX.Game.Story.Note;
using System.Collections.Generic;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleUpdateNoteHitTime : IReversible {
        private S2VXNote Note { get; }
        private double OldHitTime { get; }
        private double NewHitTime { get; }
        private Dictionary<S2VXNote, double> SelectedNoteToTime { get; } = new Dictionary<S2VXNote, double>();

        public ReversibleUpdateNoteHitTime(S2VXNote note, double oldHitTime, double newHitTime, Dictionary<S2VXNote, double> selectedNoteToTime) {
            Note = note;
            OldHitTime = oldHitTime;
            NewHitTime = newHitTime;
            SelectedNoteToTime = selectedNoteToTime;
        }

        public void Undo() {
            Note.UpdateHitTime(OldHitTime);
            SelectedNoteToTime[Note] = OldHitTime;
        }

        public void Redo() {
            Note.UpdateHitTime(NewHitTime);
            SelectedNoteToTime[Note] = NewHitTime;
        }
    }
}
