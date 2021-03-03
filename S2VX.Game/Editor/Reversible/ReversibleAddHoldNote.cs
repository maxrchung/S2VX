using S2VX.Game.Editor.ToolState;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleAddHoldNote : IReversible {
        private EditorScreen Editor { get; set; }
        private S2VXStory Story { get; set; }

        private HoldNote Note { get; }

        public ReversibleAddHoldNote(S2VXStory story, HoldNote note, EditorScreen editor) {
            Story = story;
            Note = note;
            Editor = editor;
        }

        public void Undo() {
            Story.RemoveNote(Note);
            if (Editor.ToolState is SelectToolState selectTool) {
                selectTool.ClearNoteSelection();
            }
        }

        public void Redo() {
            Story.AddNote(Note);
            if (Editor.ToolState is SelectToolState selectTool) {
                selectTool.ClearNoteSelection();
                selectTool.AddNoteSelection(Note);
            }
        }
    }
}
