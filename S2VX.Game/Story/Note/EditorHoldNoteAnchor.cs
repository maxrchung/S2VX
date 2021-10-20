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
        /// It's possible to crash the game while creating a hold note. Here's
        /// an example scenario:
        /// 1). User selects the hold note tool.
        /// 2). User places a start anchor.
        /// 3). User scrolls the timeline forward
        /// 4). User clicks and drags when placing a second anchor.
        ///
        /// During a hold note's creation, the hold note's approach is not set.
        /// In step 4, when a user clicks and drags, the drag actually triggers
        /// the end anchor's OnDrag and tries to update the hold note's
        /// coordinates and approach coordinates. Because the approach doesn't
        /// exist, the game throws a null exception.
        ///
        /// Two ways we can solve this is either adding a guard clause so that
        /// only the select tool can perform anchor updates or adding null
        /// checks within the hold note class's update functions. I opted for
        /// the former since clicking with the hold note tool should only create
        /// or manipulate hold notes. It feels confusing to me if a click also
        /// updates anchor. That seems more like the job exclusive for select
        /// tool.
        /// </summary>
        protected static bool CanUpdateAnchor(EditorScreen editor) =>
            editor.ToolState is SelectToolState;
    }
}
