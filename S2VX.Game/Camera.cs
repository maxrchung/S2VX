using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics;
using osuTK;

namespace S2VX.Game
{
    public class Camera : Drawable
    {
        private CameraMoveCommand command = new CameraMoveCommand
        {
            StartTime = 0,
            EndTime = 10000,
            StartPosition = new Vector2(0, 0),
            EndPosition = new Vector2(5, 5),
            Easing = Easing.None
        };

        protected override void Update()
        {
            command.Apply(this, Time.Current);
        }
    }
}
