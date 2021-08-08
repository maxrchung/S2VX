using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using S2VX.Game.Editor;
using S2VX.Game.Editor.Reversible;

namespace S2VX.Game.Story.Note {
    public class EditorHoldNoteAnchor : Box {
        public static int AnchorWidth { get; } = 10;

        [Resolved]
        private EditorScreen Editor { get; set; } = null;
        private EditorHoldNote Note { get; set; }
        private bool IsHead { get; set; }
        private Vector2 OldCoords { get; set; }

        public EditorHoldNoteAnchor(EditorHoldNote note, bool isHead) {
            Colour = S2VXColorConstants.BrickRed;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Note = note;
            IsHead = isHead;
            Size = new(AnchorWidth);
        }

        protected override bool OnDragStart(DragStartEvent e) {
            OldCoords = IsHead ? Note.Coordinates : Note.EndCoordinates;
            return true;
        }

        protected override void OnDrag(DragEvent e) {
            if (IsHead) {
                Note.UpdateCoordinates(Editor.MousePosition);
            } else {
                Note.UpdateEndCoordinates(Editor.MousePosition);
            }
        }

        protected override void OnDragEnd(DragEndEvent e) {
            var mousePos = Editor.MousePosition;

            if (IsHead) {
                Editor.Reversibles.Push(new ReversibleUpdateNoteCoordinates(Note, OldCoords, mousePos));
            } else {
                Editor.Reversibles.Push(new ReversibleUpdateHoldNoteEndCoordinates(Note, OldCoords, mousePos));
            }
        }
    }
}
