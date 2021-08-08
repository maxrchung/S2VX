using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace S2VX.Game.Story.Note {
    public class EditorHoldNoteAnchor : Box {
        public static int AnchorWidth { get; } = 10;
        protected EditorHoldNote Note { get; set; }
        protected Vector2 OldCoords { get; set; }

        public EditorHoldNoteAnchor(EditorHoldNote note) {
            Colour = S2VXColorConstants.BrickRed;
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Note = note;
            Size = new(AnchorWidth);
        }
    }
}
