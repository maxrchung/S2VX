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
        private float lineWidthUnit = 2f / 3f;

        public TimelineSlider()
        {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.CentreLeft;
            Origin = Anchor.Centre;
        }
    }
}
