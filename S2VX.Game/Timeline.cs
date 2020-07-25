using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Text;
using osuTK;
using osuTK.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace S2VX.Game
{
    public class Timeline : CompositeDrawable
    {
        [Resolved]
        private Story Story { get; set; } = new Story();

        private Container bar { get; set; } = new Container();

        private RelativeBox slider { get; set; } = new RelativeBox
        {
            Colour = Color4.White,
            Height = 0.5f,
            Width = timelineWidth / 150,
            Anchor = Anchor.CentreLeft,
            RelativePositionAxes = Axes.None
        };

        private bool switchToPlaying { get; set; } = false;

        private bool delayDrag { get; set; } = false;

        private void updateSlider(Vector2 mousePosition)
        {
            var mousePosX = ToLocalSpace(mousePosition).X;
            var xPosDelta = (DrawWidth - bar.DrawWidth) / 2;
            var newX = mousePosX - xPosDelta;
            var xLengthRatio = newX / bar.DrawWidth;
            var newTime = xLengthRatio * Story.Track.Length;

            var clampedX = Math.Clamp(newX, 0, bar.DrawWidth);
            slider.X = clampedX;

            var clampedTime = Math.Clamp(newTime, 0, Story.Track.Length);
            Story.Seek(clampedTime);
        }

        private const float timelineHeight = 0.075f;
        private const float timelineWidth = 1.0f;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Height = timelineHeight;
            Width = timelineWidth;
            Anchor = Anchor.BottomCentre;
            Origin = Anchor.BottomCentre;

            InternalChildren = new Drawable[]
            {
                new RelativeBox
                {
                    Colour = Color4.Black.Opacity(0.9f)
                },
                bar = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Color4.White,
                    Width = timelineWidth / 1.5f,
                    Children = new Drawable[]
                    {
                        new RelativeBox
                        {
                            Colour = Color4.White,
                            Height = timelineHeight / 10
                        },
                        slider
                    }
                },
            };
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            updateSlider(e.ScreenSpaceMousePosition);
            if (Story.IsPlaying)
            {
                Story.Play(false);
                switchToPlaying = true;
            }
            else
            {
                switchToPlaying = false;
            }
            return true;
        }

        protected override bool OnDragStart(DragStartEvent e) => true;

        protected override void OnDrag(DragEvent e)
        {
            if (!delayDrag)
            {
                updateSlider(e.ScreenSpaceMousePosition);
                delayDrag = true;
            }
        }

        protected override void OnDragEnd(DragEndEvent e)
        {
            if (switchToPlaying)
            {
                Story.Play(true);
            }
        }

        protected override void Update()
        {
            delayDrag = false;
            var songRatio = Story.GameTime / Story.Track.Length;
            var newX = songRatio * bar.DrawWidth;
            slider.X = (float)Math.Clamp(newX, 0, bar.DrawWidth);
        }
    }
}
