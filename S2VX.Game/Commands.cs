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
        public double StartTime { get; set; } = 0;
        public double EndTime { get; set; } = 0;
        public Easing Easing { get; set; } = Easing.None;
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
        public Vector2 StartPosition { get; set; } = Vector2.Zero;
        public Vector2 EndPosition { get; set; } = Vector2.Zero;
        public Vector2 Apply(double time)
        {
            var position = StartPosition + (EndPosition - StartPosition) * ApplyEasing(time);
            return position;
        }
    }

    public class CameraRotateCommand : Command
    {
        public float StartRotation { get; set; } = 0;
        public float EndRotation { get; set; } = 0;
        public float Apply(double time)
        {
            var rotation = StartRotation + (EndRotation - StartRotation) * ApplyEasing(time);
            return rotation;
        }
    }

    public class CameraScaleCommand : Command
    {
        public float StartScale { get; set; } = 1;
        public float EndScale { get; set; } = 1;
        public float Apply(double time)
        {
            var scale = StartScale + (EndScale - StartScale) * ApplyEasing(time);
            return scale;
        }
    }
}
