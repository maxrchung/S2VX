using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;

namespace S2VX.Game
{
    public class MainScreen : Screen
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            var elements = new List<Drawable>
            {
                new Box
                {
                    Colour = Color4.CornflowerBlue,
                    RelativeSizeAxes = Axes.Both
                }
            };

            for (float i = 0.05f; i <= 0.5f; i += 0.1f)
            {
                elements.Add(new Box
                {
                    Colour = Color4.White,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Y = i,
                    Width = 1.5f,
                    Height = 0.005f
                });

                elements.Add(new Box
                {
                    Colour = Color4.White,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Y = -i,
                    Width = 1.5f,
                    Height = 0.005f
                });

                elements.Add(new Box
                {
                    Colour = Color4.White,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    X = i,
                    Width = 0.005f,
                    Height = 1.5f
                });

                elements.Add(new Box
                {
                    Colour = Color4.White,
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    X = -i,
                    Width = 0.005f,
                    Height = 1.5f
                });
            }

            InternalChildren = elements;
        }
    }
}
