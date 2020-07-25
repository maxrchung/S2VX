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
        private Story Story { get; set; } = new Story();
        [Resolved]
        private RelativeBox TimelineSlider { get; set; } = new RelativeBox();

        private float lineWidthUnit { get; set; } = 2 / 3f;

        private bool switchToPlaying { get; set; } = false;

        private bool delayDrag { get; set; } = false;

        private (float newX, float lineWidth, double newTime) getNewValues(float mousePosX)
        {
            var xPosDelta = (DrawWidth - (DrawWidth / 1.5f)) / 2;
            var newX = mousePosX - xPosDelta;
            var lineWidth = lineWidthUnit * Story.DrawWidth;
            var xLengthRatio = newX / lineWidth;
            var newTime = xLengthRatio * Story.Track.Length;
            return (newX, lineWidth, newTime);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Colour = Color4.Black.Opacity(0.9f);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            var (newX, lineWidth, newTime) = getNewValues(ToLocalSpace(e.ScreenSpaceMousePosition).X);

            if (newX >= 0 && newX <= lineWidth)
            {
                TimelineSlider.X = newX;
                Story.Seek(newTime);
            }

            return true;
        }

        protected override bool OnDragStart(DragStartEvent e) => true;

        protected override void OnDrag(DragEvent e)
        {
            if (!delayDrag)
            {
                var (newX, lineWidth, newTime) = getNewValues(ToLocalSpace(e.ScreenSpaceMousePosition).X);
                newX = Math.Clamp(newX, 0, lineWidth);
                TimelineSlider.X = newX;

                if (Story.IsPlaying)
                {
                    Story.Play(false);
                    switchToPlaying = true;
                }

                var clampedTime = Math.Clamp(newTime, 0, Story.Track.Length);
                Story.Seek(clampedTime);
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
            var songRatio = curTime / Story.Track.Length;
            var lineWidth = lineWidthUnit * Story.DrawWidth;
            var newX = songRatio * lineWidth;
            TimelineSlider.X = (float)Math.Clamp(newX, 0, lineWidth);
        }
    }
}
