using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osuTK;

namespace S2VX.Game
{
    public abstract class Command
    {
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public Easing Easing { get; set; }
        protected double ApplyEasing(double time)
        {
            var fraction = (time - StartTime) / EndTime;
            var easing = new DefaultEasingFunction(Easing);
            var value = easing.ApplyEasing(fraction);
            return value;
        }
    }

    public class CameraMoveCommand : Command
    {
        public Vector2 StartPosition { get; set; }
        public Vector2 EndPosition { get; set; }
        public void Apply(Camera camera, double time)
        {
            var position = StartPosition + Vector2.Multiply(EndPosition - StartPosition, (float) ApplyEasing(time));
            camera.Position = position;
        }
    }

    public class GridMoveCommand : Command
    {
        public Vector2 StartPosition { get; set; }
        public Vector2 EndPosition { get; set; }
        public Vector2 Apply(double time)
        {
            var position = StartPosition + Vector2.Multiply(EndPosition - StartPosition, (float)ApplyEasing(time));
            return position;
        }
    }
}
