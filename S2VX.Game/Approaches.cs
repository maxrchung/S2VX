using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace S2VX.Game
{
    public class Approaches : CompositeDrawable
    {
        public float Distance = 0.5f;
        public float Thickness = 0.005f;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            var notes = new List<Drawable>()
            {
                new Approach
                {
                    EndTime = 5000,
                    Coordinates = new Vector2(3, 3)
                }
            };
            InternalChildren = notes;
        }
    }
}
