﻿using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using S2VX.Game.Editor;
using S2VX.Game.Editor.Reversible;

namespace S2VX.Game.Story.Note {
    public class EditorHoldNoteEndAnchor : EditorHoldNoteAnchor {
        [Resolved]
        private EditorScreen Editor { get; set; } = null;

        public EditorHoldNoteEndAnchor(EditorHoldNote note) : base(note) { }

        protected override bool OnDragStart(DragStartEvent e) {
            if (!CanUpdateAnchor(Editor)) {
                return false;
            }
            OldCoords = Note.EndCoordinates;
            return true;
        }

        protected override void OnDrag(DragEvent e) {
            if (!CanUpdateAnchor(Editor)) {
                return;
            }
            Note.UpdateEndCoordinates(Editor.MousePosition);
        }

        protected override void OnDragEnd(DragEndEvent e) {
            if (!CanUpdateAnchor(Editor)) {
                return;
            }
            Editor.Reversibles.Push(new ReversibleUpdateHoldNoteEndCoordinates(Note, OldCoords, Editor.MousePosition));
        }
    }
}
