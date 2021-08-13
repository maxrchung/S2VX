﻿using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using S2VX.Game.Editor;
using S2VX.Game.Editor.Reversible;

namespace S2VX.Game.Story.Note {
    public class EditorHoldNoteStartAnchor : EditorHoldNoteAnchor {
        [Resolved]
        private EditorScreen Editor { get; set; } = null;

        public EditorHoldNoteStartAnchor(EditorHoldNote note) : base(note) { }

        protected override bool OnDragStart(DragStartEvent e) {
            OldCoords = Note.Coordinates;
            return true;
        }

        protected override void OnDrag(DragEvent e) => Note.UpdateCoordinates(Editor.MousePosition);

        protected override void OnDragEnd(DragEndEvent e) =>
            Editor.Reversibles.Push(new ReversibleUpdateNoteCoordinates(Note, OldCoords, Editor.MousePosition));
    }
}