using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace S2VX.Game
{
    public class TimelineBar : Box
    {
        [Resolved]
        private Story Story { get; set; }
        [Resolved]
        private TimelineSlider TimelineSlider { get; set; }
        private float lineWidthUnit = 2f / 3f;

        private double trackLength = 368925.62358276645; // temp
        public TimelineBar()
        {
            RelativeSizeAxes = Axes.Both;
            Colour = Color4.Black.Opacity(0.9f);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            var xPosDelta = (DrawWidth - (DrawWidth / 1.5f)) / 2;
            var localX = ToLocalSpace(e.ScreenSpaceMousePosition).X - xPosDelta;
            var lineWidth = lineWidthUnit * Story.DrawWidth;

            if (localX >= 0 && localX <= lineWidth)
            {
                var xLengthRatio = localX / lineWidth;
                TimelineSlider.X = localX;
                Story.GameTime = xLengthRatio * trackLength;
            }

            return true;
        }

        protected override bool OnDragStart(DragStartEvent e) => true;

        protected override void OnDrag(DragEvent e)
        {
            var xPosDelta = (DrawWidth - (DrawWidth / 1.5f)) / 2;
            var newX = X + ToLocalSpace(e.ScreenSpaceMousePosition).X - xPosDelta;
            var lineWidth = lineWidthUnit * Story.DrawWidth;
            var xLengthRatio = newX / lineWidth;
            newX = Math.Clamp(newX, 0, lineWidth);
            TimelineSlider.X = newX;
            Story.GameTime = xLengthRatio * trackLength;
        }
    }
}
