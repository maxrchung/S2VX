using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;
using S2VX.Game.Editor;
using S2VX.Game.Editor.ToolState;

namespace S2VX.Game.Story.Note {
    public class EditorHoldNoteAnchor : Box {
        public static int AnchorWidth { get; } = 10;
        protected EditorHoldNote Note { get; set; }
        protected Vector2 OldCoords { get; set; }

        public EditorHoldNoteAnchor(EditorHoldNote note) {
            Colour = S2VXColorConstants.BrickRed;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Note = note;
            Size = new(AnchorWidth);
        }

        /// <summary>
        /// Guard clause to ensure that only select tool can update anchors.
        /// Null exception can be thrown if anchors are updated during hold note
        /// creation.
        /// </summary>
        protected static bool CanUpdateAnchor(EditorScreen editor) =>
            editor.ToolState is SelectToolState;
    }
}
