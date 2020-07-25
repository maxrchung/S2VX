using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace S2VX.Game
{
    public class Approaches : CompositeDrawable
    {
        public List<Approach> Children = new List<Approach>();
        public float Distance { get; set; } = 0.5f;
        public float Thickness { get; set; } = 0.005f;

        [Resolved]
        private Notes notes { get; set; } = new Notes();

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = Children;
        }

        protected override void Update()
        {
            Alpha = notes.Alpha;
            Colour = notes.Colour;
        }
    }
}
