using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using SixLabors.ImageSharp;
using osuTK;
using SixLabors.ImageSharp.Processing;


namespace S2VX.Game
{
    public class Timeline : CompositeDrawable
    {
        private const float timeline_height = 0.075f;
        private const float timeline_width = 1.0f;
        private const float slider_height = 0.5f;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                new Container
                {
                    Name = "Timeline content",
                    RelativeSizeAxes = Axes.Both,
                    Height = timeline_height,
                    Width = timeline_width,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,

                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Name = "Bar",
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Black.Opacity(0.9f),
                        },
                        new RelativeBox
                        {
                            Name = "Timeline",
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.White,
                            Height = timeline_height / 10,
                            Width = timeline_width / 1.5f,
                        },
                        new RelativeBox
                        {
                            Name = "Slider",
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.White,
                            Height = slider_height,
                            Width = timeline_width / 600,
                        }
                    }
                }
            };
        }
    }
}
