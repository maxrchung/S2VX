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
            Easing = Easing.OutElastic
        };

        private CameraRotateCommand rotate = new CameraRotateCommand
        {
            StartTime = 0,
            EndTime = 60000,
            StartRotation = 0.0f,
            EndRotation = 360.0f,
            Easing = Easing.InQuart
        };

        private CameraScaleCommand scale = new CameraScaleCommand
        {
            StartTime = 0,
            EndTime = 60000,
            StartScale = 0.6f,
            EndScale = 0.01f,
            Easing = Easing.InOutBounce
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
            if (Time.Current <= scale.EndTime)
            {
                var newScale = scale.Apply(Time.Current);
                Scale = new Vector2(newScale);
            }
        }
    }
}
