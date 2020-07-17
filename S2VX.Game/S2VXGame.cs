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

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    Story.IsPlaying = !Story.IsPlaying;
                    break;
                case Key.X:
                    Story.GameTime = 0;
                    break;
            }

            return true;
        }
    }
}
