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
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Text;
using System.Linq;

namespace S2VX.Game
{
    public class Timeline : CompositeDrawable
    {
        [Cached]
        private TimelineSlider TimelineSlider = new TimelineSlider
        {
            Name = "Slider",
            Colour = Color4.White,
            Height = slider_height,
            Width = timeline_width / 150, //600
        };

        private const float timeline_height = 0.075f;
        private const float timeline_width = 1.0f;
        private const float slider_height = 0.5f;
        private Container LineContainer { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                LineContainer = new Container
                {
                    Name = "Timeline content",
                    RelativeSizeAxes = Axes.Both,
                    Height = timeline_height,
                    Width = timeline_width,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,

                    Children = new Drawable[]
                    {
                        new TimelineBar
                        {
                            Name = "Bar",
                        },
                        new Container
                        {
                            Name = "Timeline",
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Colour = Color4.White,
                            Width = timeline_width / 1.5f,

                            Children = new Drawable[]
                            {
                                new RelativeBox
                                {
                                    Name = "Line",
                                    Colour = Color4.White,
                                    Height = timeline_height / 10,

                                },
                                TimelineSlider
                            }
                        },
                        //new Clock
                        //{

                        //}
                    }
                }
            };
        }
    }
}
