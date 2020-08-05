using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Input;

namespace S2VX.Game
{
    public class MouseToCameraText : SpriteText
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
            var mouse = Mouse.GetState();
            var mousePosition = new Vector2(mouse.X, mouse.Y);
            var camera = story.Camera;
            Text = Utils.Vector2ToString(mousePosition);
        }
    }
}
