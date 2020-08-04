using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace S2VX.Game
{
    public class MouseToGridText : SpriteText
    {
        [Resolved]
        private Story story { get; set; } = new Story();

        [BackgroundDependencyLoader]
        private void load()
        {
            Text = " ";
        }

        protected override void Update()
        {
            Text = "(300, 300)";
        }
    }
}
