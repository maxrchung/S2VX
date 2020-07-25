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

        private double trackLength => Story.Track.Length;

        private bool switchToPlaying = false;

        private bool delayDrag = false;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Colour = Color4.Black.Opacity(0.9f);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            var xPosDelta = (DrawWidth - (DrawWidth / 1.5f)) / 2;
            var newX = ToLocalSpace(e.ScreenSpaceMousePosition).X - xPosDelta;
            var lineWidth = lineWidthUnit * Story.DrawWidth;

            if (newX >= 0 && newX <= lineWidth)
            {
                var xLengthRatio = newX / lineWidth;
                TimelineSlider.X = newX;
                var newTime = xLengthRatio * trackLength;
                Story.GameTime = newTime;
                Story.Track.Seek(newTime);
            }

            return true;
        }

        protected override bool OnDragStart(DragStartEvent e) => true;

        protected override void OnDrag(DragEvent e)
        {
            if (!delayDrag)
            {
                var xPosDelta = (DrawWidth - (DrawWidth / 1.5f)) / 2;
                var newX = X + ToLocalSpace(e.ScreenSpaceMousePosition).X - xPosDelta;
                var lineWidth = lineWidthUnit * Story.DrawWidth;
                var xLengthRatio = newX / lineWidth;
                newX = Math.Clamp(newX, 0, lineWidth);
                TimelineSlider.X = newX;
                var newTime = xLengthRatio * trackLength;
                Story.GameTime = Math.Clamp(newTime, 0, trackLength);

                if (Story.IsPlaying)
                {
                    Story.Track.Stop();
                    Story.IsPlaying = false;
                    switchToPlaying = true;
                }

                Story.Track.Seek(newTime);
                delayDrag = true;
            }
        }

        protected override void OnDragEnd(DragEndEvent e)
        {
            if (switchToPlaying)
            {
                Story.Track.Start();
                Story.IsPlaying = true;
                switchToPlaying = false;
            }
        }

        protected override void Update()
        {
            delayDrag = false;
            var curTime = Story.GameTime;
            var songRatio = curTime / trackLength;
            var lineWidth = lineWidthUnit * Story.DrawWidth;
            var newX = songRatio * lineWidth;
            TimelineSlider.X = (float)Math.Clamp(newX, 0, lineWidth);
        }
    }
}
