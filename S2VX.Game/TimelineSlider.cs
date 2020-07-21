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

        public TimelineSlider()
        {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.CentreLeft;
            Origin = Anchor.Centre;
        }

        protected override bool OnDragStart(DragStartEvent e) => true;

        protected override void OnDrag(DragEvent e)
        {
            X += e.Delta.X;
        }
    }
}
