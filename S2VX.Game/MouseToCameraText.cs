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
            var mousePosition = story.MousePosition;
            var relativePosition = (mousePosition - (story.DrawSize / 2)) / story.DrawWidth;
            var camera = story.Camera;
            var rotatedPosition = Utils.Rotate(relativePosition, camera.Rotation);
            var scaledPosition = rotatedPosition * (1 / camera.Scale.X);
            var translatedPosition = scaledPosition + camera.Position;
            Text = Utils.Vector2ToString(translatedPosition);
        }
    }
}
