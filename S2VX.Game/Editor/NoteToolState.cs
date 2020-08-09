using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osuTK;
using S2VX.Game.Story;

namespace S2VX.Game.Editor
{
    public class NoteToolState : ToolState
    {
        private Note preview { get; set; } = new Note();

        [Resolved]
        private S2VXEditor editor { get; set; } = null;

        [Resolved]
        private S2VXStory story { get; set; } = null;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Size = Vector2.One;
            InternalChild = preview;
        }

        public override bool OnMouseClick()
        {
            return false;
        }

        protected override void Update()
        {
            preview.EndTime = story.GameTime;
            preview.Coordinates = editor.MousePosition;
        }
    }
}
