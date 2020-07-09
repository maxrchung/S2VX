using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace S2VX.Game
{
    public class Approaches : CompositeDrawable
    {
        public float Distance { get; set; } = 0.5f;
        public float Thickness { get; set; } = 0.005f;

        private Notes notes { get; set; } = new Notes();

        [BackgroundDependencyLoader]
        private void load(Notes notes)
        {
            this.notes = notes;

            RelativeSizeAxes = Axes.Both;
            var approaches = new List<Drawable>()
            {
                new Approach
                {
                    EndTime = 5000,
                    Coordinates = new Vector2(3, 3)
                }
            };
            InternalChildren = approaches;
        }

        protected override void Update()
        {
            Alpha = notes.Alpha;
            Colour = notes.Colour;
        }
    }
}
