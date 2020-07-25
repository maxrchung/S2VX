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
using osu.Framework.Text;
using osuTK;
using osuTK.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace S2VX.Game
{
    public class Timeline : CompositeDrawable
    {
        [Cached]
        private RelativeBox timelineSlider = new RelativeBox
        {
            Colour = Color4.White,
            Height = 0.5f,
            Width = timelineWidth / 150,
            Anchor = Anchor.CentreLeft,
            RelativePositionAxes = Axes.None,
        };

        private const float timelineHeight  = 0.075f;
        private const float timelineWidth = 1.0f;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Height = timelineHeight,
                    Width = timelineWidth,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,

                    Children = new Drawable[]
                    {
                        new TimelineBar(),
                        new Container
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
                                    Height = timelineHeight / 10,

                                },
                                timelineSlider
                            }
                        },
                    }
                }
            };
        }
    }
}
