using S2VX.Game.Editor.ToolState;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleUpdateNoteEndTime : IReversible {
        private EditorScreen Editor { get; set; }
        private S2VXNote Note { get; }
        private double OldEndTime { get; }
        private double NewEndTime { get; }

        public ReversibleUpdateNoteEndTime(S2VXNote note, double oldEndTime, double newEndTime, EditorScreen editor) {
            Note = note;
            OldEndTime = oldEndTime;
            NewEndTime = newEndTime;
            Editor = editor;
        }

        public void Undo() {
            Note.UpdateEndTime(OldEndTime);
            if (Editor.ToolState is SelectToolState selectTool) {
                selectTool.ClearNoteSelection();
                selectTool.AddNoteSelection(Note);
            }
        }

        public void Redo() {
            Note.UpdateEndTime(NewEndTime);
            if (Editor.ToolState is SelectToolState selectTool) {
                selectTool.ClearNoteSelection();
                selectTool.AddNoteSelection(Note);
            }
        }
    }
}
