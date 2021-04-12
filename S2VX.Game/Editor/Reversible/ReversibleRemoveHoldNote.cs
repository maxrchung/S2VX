using S2VX.Game.Editor.ToolState;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleRemoveHoldNote : IReversible {
        private EditorScreen Editor { get; set; }
        private S2VXStory Story { get; set; }
        private HoldNote Note { get; }

        public ReversibleRemoveHoldNote(S2VXStory story, HoldNote note, EditorScreen editor) {
            Story = story;
            Note = note;
            Editor = editor;
        }

        public void Undo() {
            Story.AddNote(Note);
            if (Editor.ToolState is SelectToolState selectTool) {
                selectTool.ClearNoteSelection();
                selectTool.AddNoteSelection(Note);
            }
        }

        public void Redo() {
            Story.RemoveNote(Note);
            if (Editor.ToolState is SelectToolState selectTool) {
                selectTool.ClearNoteSelection();
            }
        }
    }
}
