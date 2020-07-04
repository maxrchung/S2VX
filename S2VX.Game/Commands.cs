using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game
{
    public abstract class Command
    {
        public double StartTime { get; set; } = 0;
        public double EndTime { get; set; } = 0;
        public Easing Easing { get; set; } = Easing.None;
    }

    public class CameraMoveCommand : Command
    {
        public Vector2 StartPosition { get; set; } = Vector2.Zero;
        public Vector2 EndPosition { get; set; } = Vector2.Zero;
        public Vector2 Apply(double time)
        {
            var position = Interpolation.ValueAt(time, StartPosition, EndPosition, StartTime, EndTime, Easing);
            return position;
        }
    }

    public class CameraRotateCommand : Command
    {
        public float StartRotation { get; set; } = 0;
        public float EndRotation { get; set; } = 0;
        public float Apply(double time)
        {
            var rotation = Interpolation.ValueAt(time, StartRotation, EndRotation, StartTime, EndTime, Easing);
            return rotation;
        }
    }

    public class CameraScaleCommand : Command
    {
        public float StartScale { get; set; } = 0.1f;
        public float EndScale { get; set; } = 0.1f;
        public float Apply(double time)
        {
            var scale = Interpolation.ValueAt(time, StartScale, EndScale, StartTime, EndTime, Easing);
            return scale;
        }
    }

    public class GridAlphaCommand : Command
    {
        public float StartAlpha { get; set; } = 1;
        public float EndAlpha { get; set; } = 1;
        public float Apply(double time)
        {
            var alpha = Interpolation.ValueAt(time, StartAlpha, EndAlpha, StartTime, EndTime, Easing);
            return alpha;
        }
    }

    public class GridColorCommand : Command
    {
        public Color4 StartColor { get; set; } = Color4.White;
        public Color4 EndColor { get; set; } = Color4.White;
        public Color4 Apply(double time)
        {
            var color = Interpolation.ValueAt(time, StartColor, EndColor, StartTime, EndTime, Easing);
            return color;
        }
    }

    public class GridThicknessCommand : Command
    {
        public float StartThickness { get; set; } = 0.005f;
        public float EndThickness { get; set; } = 0.005f;
        public float Apply(double time)
        {
            var alpha = Interpolation.ValueAt(time, StartThickness, EndThickness, StartTime, EndTime, Easing);
            return alpha;
        }
    }

    public class BackColorCommand : Command
    {
        public Color4 StartColor { get; set; } = Color4.White;
        public Color4 EndColor { get; set; } = Color4.White;
        public Color4 Apply(double time)
        {
            var color = Interpolation.ValueAt(time, StartColor, EndColor, StartTime, EndTime, Easing);
            return color;
        }
    }
}
