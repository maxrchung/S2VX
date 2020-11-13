using S2VX.Game.Editor.ToolState;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleUpdateNoteHitTime : IReversible {
        private EditorScreen Editor { get; set; }
        private S2VXNote Note { get; }
        private double OldHitTime { get; }
        private double NewHitTime { get; }

        public ReversibleUpdateNoteHitTime(S2VXNote note, double oldHitTime, double newHitTime, EditorScreen editor) {
            Note = note;
            OldHitTime = oldHitTime;
            NewHitTime = newHitTime;
            Editor = editor;
        }

        public void Undo() {
            Note.UpdateHitTime(OldHitTime);
            if (Editor.ToolState is SelectToolState selectTool) {
                selectTool.ClearNoteSelection();
                selectTool.AddNoteSelection(Note);
            }
        }

        public void Redo() {
            Note.UpdateHitTime(NewHitTime);
            if (Editor.ToolState is SelectToolState selectTool) {
                selectTool.ClearNoteSelection();
                selectTool.AddNoteSelection(Note);
            }
        }
    }
}
