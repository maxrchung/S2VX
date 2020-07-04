using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics;
using osuTK;

namespace S2VX.Game
{
    public class Camera : Drawable
    {
        private CameraMoveCommand move = new CameraMoveCommand
        {
            StartTime = 0,
            EndTime = 60000,
            StartPosition = new Vector2(0,0),
            EndPosition = new Vector2(5,5),
            Easing = Easing.None
        };

        private CameraRotateCommand rotate = new CameraRotateCommand
        {
            StartTime = 0,
            EndTime = 60000,
            StartRotation = 0.0f,
            EndRotation = 360.0f,
            Easing = Easing.None
        };

        protected override void Update()
        {
            if (Time.Current <= move.EndTime)
            {
                Position = move.Apply(Time.Current);
            }
            if (Time.Current <= rotate.EndTime)
            {
                Rotation = rotate.Apply(Time.Current);
            }
        }
    }
}
