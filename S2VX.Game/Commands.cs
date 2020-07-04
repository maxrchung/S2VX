using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Framework.Utils;
using osuTK;

namespace S2VX.Game
{
    public abstract class Command
    {
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public Easing Easing { get; set; }
        protected float ApplyEasing(double time)
        {
            var fraction = (time - StartTime) / EndTime;
            var easing = new DefaultEasingFunction(Easing);
            var value = (float)easing.ApplyEasing(fraction);
            return value;
        }
    }

    public class CameraMoveCommand : Command
    {
        public Vector2 StartPosition { get; set; }
        public Vector2 EndPosition { get; set; }
        public Vector2 Apply(double time)
        {
            var position = StartPosition + (EndPosition - StartPosition) * ApplyEasing(time);
            return position;
        }
    }

    public class CameraRotateCommand : Command
    {
        public float StartRotation { get; set; }
        public float EndRotation { get; set; }
        public float Apply(double time)
        {
            var rotation = StartRotation + (EndRotation - StartRotation) * ApplyEasing(time);
            return rotation;
        }
    }
}
