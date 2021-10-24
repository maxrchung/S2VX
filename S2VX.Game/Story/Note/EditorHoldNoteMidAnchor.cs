using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using S2VX.Game.Editor;
using S2VX.Game.Editor.Reversible;

namespace S2VX.Game.Story.Note {
    public class EditorHoldNoteMidAnchor : EditorHoldNoteAnchor {
        [Resolved]
        private EditorScreen Editor { get; set; } = null;

        // Corresponds to the MidCoordinates index the anchor is representing
        private int MidIndex { get; }

        public EditorHoldNoteMidAnchor(EditorHoldNote note, int midIndex) : base(note) => MidIndex = midIndex;

        protected override bool OnDragStart(DragStartEvent e) {
            if (!CanUpdateAnchor(Editor)) {
                return false;
            }
            OldCoords = Note.MidCoordinates[MidIndex];
            return true;
        }

        protected override void OnDrag(DragEvent e) {
            if (!CanUpdateAnchor(Editor)) {
                return;
            }
            Note.UpdateMidCoordinates(Editor.MousePosition, MidIndex);
        }

        protected override void OnDragEnd(DragEndEvent e) {
            if (!CanUpdateAnchor(Editor)) {
                return;
            }
            Editor.Reversibles.Push(new ReversibleUpdateHoldNoteMidCoordinates(Note, OldCoords, Editor.MousePosition, MidIndex));
        }
    }
}
