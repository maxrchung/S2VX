using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using S2VX.Game.Editor;
using S2VX.Game.Editor.Reversible;

namespace S2VX.Game.Story.Note {
    public class EditorHoldNoteAnchor : Box {
        [Resolved]
        private EditorScreen Editor { get; set; } = null;
        private EditorHoldNote Note { get; set; }
        private bool IsHead { get; set; }
        private Vector2 OldCoords { get; set; }

        public EditorHoldNoteAnchor(EditorHoldNote note, bool isHead) {
            Colour = S2VXColorConstants.BrickRed;
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Note = note;
            IsHead = isHead;
            Alpha = 0;
        }

        protected override bool OnClick(ClickEvent e) {
            Note.IsFlaggedToToggleSelection = false;
            return false;
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
