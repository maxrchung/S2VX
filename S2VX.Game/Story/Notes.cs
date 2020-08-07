using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace S2VX.Game.Story
{
    public class Notes : CompositeDrawable
    {
        public List<Note> Children = new List<Note>();

        // Notes fade in, show for a period of time, then fade out
        // The note should be hit at the very end of the show time
        public float FadeInTime { get; set; } = 100;
        public float ShowTime { get; set; } = 1000;
        public float FadeOutTime { get; set; } = 100;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = Children;
        }
    }
}
