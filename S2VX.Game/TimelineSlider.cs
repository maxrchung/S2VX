using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;

namespace S2VX.Game
{
    public class TimelineSlider : Box
    {
        [Resolved]
        private Story Story { get; set; }
        private float lineWidthUnit = 2f / 3f;

        public TimelineSlider()
        {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.CentreLeft;
            Origin = Anchor.Centre;
        }

        protected override bool OnDragStart(DragStartEvent e) => true;

        protected override void OnDrag(DragEvent e)
        {
            var newX = X + ToLocalSpace(e.ScreenSpaceMousePosition).X;
            var lineWidth = lineWidthUnit * Story.DrawWidth;
            newX = Math.Clamp(newX, 0, lineWidth);
            X = newX;
        }
    }
}
