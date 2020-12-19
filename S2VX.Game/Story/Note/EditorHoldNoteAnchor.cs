using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using S2VX.Game.Editor;
using System;
using System.Collections.Generic;
using System.Text;

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
        }

        protected override bool OnDragStart(DragStartEvent e) {
            if (IsHead) {
                OldCoords = Note.Coordinates;
            } else {
                OldCoords = Note.EndCoordinates;
            }
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
            //if (IsHead) {
            //    Editor.Reversibles.Push(new ReversibleUpdateNoteCoordinates(note, OldCoords, newPos));
            //} else {
            //}
            // Make UpdateEndCoordinates reverseible later
        }
    }
}
