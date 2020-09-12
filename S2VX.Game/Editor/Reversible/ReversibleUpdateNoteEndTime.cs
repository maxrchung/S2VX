using S2VX.Game.Story;
using System;
using System.Collections.Generic;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleUpdateNoteEndTime : IReversible {
        private Note Note { get; }
        private double OldEndTime { get; }
        private double NewEndTime { get; }
        private Dictionary<Note, double> SelectedNoteToTime { get; } = new Dictionary<Note, double>();

        public ReversibleUpdateNoteEndTime(Note note, double oldEndTime, double newEndTime, Dictionary<Note, double> selectedNoteToTime) {
            Note = note;
            OldEndTime = oldEndTime;
            Console.WriteLine(oldEndTime);
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
