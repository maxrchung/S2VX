using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK.Input;

namespace S2VX.Game
{
    public class S2VXGame : S2VXGameBase
    {
        [Cached]
        public Story Story { get; } = new Story();

        [BackgroundDependencyLoader]
        private void load()
        {
            Child = Story;
        }
    }
}
