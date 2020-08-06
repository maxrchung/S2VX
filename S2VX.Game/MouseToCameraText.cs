using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;

namespace S2VX.Game
{
    public class MouseToCameraText : SpriteText
    {
        [Resolved]
        private Story story { get; set; } = null;

        [BackgroundDependencyLoader]
        private void load()
        {
            Text = " ";
        }

        protected override void Update()
        {
            Text = Utils.Vector2ToString(story.MousePosition);
        }
    }
}
