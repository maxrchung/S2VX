using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace S2VX.Game
{
    public class Notes : CompositeDrawable
    {
        public float ShowTime = 1000;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            var notes = new List<Drawable>()
            {
                new Note
                {
                    EndTime = 5000,
                    Coordinates = new Vector2(3, 3)
                }
            };

            InternalChildren = notes;
        }
    }
}
