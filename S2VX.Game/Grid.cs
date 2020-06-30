using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game
{
    public class Grid : CompositeDrawable
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;

            var grid = new List<Drawable>();
            for (float i = 0.05f; i <= 0.75f; i += 0.1f)
            {
                grid.Add(new RelativeBox
                {
                    Colour = Color4.White,
                    Y = i,
                    Width = 1.5f,
                    Height = 0.005f
                });
                grid.Add(new RelativeBox
                {
                    Colour = Color4.White,
                    Y = -i,
                    Width = 1.5f,
                    Height = 0.005f
                });
                grid.Add(new RelativeBox
                {
                    Colour = Color4.White,
                    X = i,
                    Width = 0.005f,
                    Height = 1.5f
                });
                grid.Add(new RelativeBox
                {
                    Colour = Color4.White,
                    X = -i,
                    Width = 0.005f,
                    Height = 1.5f
                });
            }

            InternalChildren = grid;
        }

        protected override void Update()
        {
            var prevPosition = Position;
            if (prevPosition == Position)
            {
                return;
            }
        }
    }
}
