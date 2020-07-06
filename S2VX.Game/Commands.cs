using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Transforms;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;
using SixLabors.ImageSharp.Processing;

namespace S2VX.Game
{
    public abstract class Command : IComparable<Command>
    {
        public double StartTime { get; set; } = 0;
        public double EndTime { get; set; } = 0;
        public Easing Easing { get; set; } = Easing.None;
        public abstract void Apply(double time);

        public int CompareTo(Command other)
        {
            return StartTime.CompareTo(other.StartTime);
        }
    }

    public abstract class CameraCommand : Command
    {
        public Camera Camera = new Camera();
    }

    public class CameraMoveCommand : CameraCommand
    {
        public Vector2 StartPosition { get; set; } = Vector2.Zero;
        public Vector2 EndPosition { get; set; } = Vector2.Zero;
        public override void Apply(double time)
        {
            var position = Interpolation.ValueAt(time, StartPosition, EndPosition, StartTime, EndTime, Easing);
            Camera.Position = position;
        }
    }

    public class CameraRotateCommand : CameraCommand
    {
        public float StartRotation { get; set; } = 0;
        public float EndRotation { get; set; } = 0;
        public override void Apply(double time)
        {
            var rotation = Interpolation.ValueAt(time, StartRotation, EndRotation, StartTime, EndTime, Easing);
            Camera.Rotation = rotation;
        }
    }

    public class CameraScaleCommand : CameraCommand
    {
        public Vector2 StartScale { get; set; } = new Vector2(0.1f);
        public Vector2 EndScale { get; set; } = new Vector2(0.1f);
        public override void Apply(double time)
        {
            var scale = Interpolation.ValueAt(time, StartScale, EndScale, StartTime, EndTime, Easing);
            Camera.Scale = scale;
        }
    }

    public abstract class GridCommand : Command
    {
        public Grid Grid = new Grid();
    }

    public class GridAlphaCommand : GridCommand
    {
        public float StartAlpha { get; set; } = 1;
        public float EndAlpha { get; set; } = 1;
        public override void Apply(double time)
        {
            var alpha = Interpolation.ValueAt(time, StartAlpha, EndAlpha, StartTime, EndTime, Easing);
            Grid.Alpha = alpha;
        }
    }

    public class GridColorCommand : GridCommand
    {
        public Color4 StartColor { get; set; } = Color4.White;
        public Color4 EndColor { get; set; } = Color4.White;
        public override void Apply(double time)
        {
            var color = Interpolation.ValueAt(time, StartColor, EndColor, StartTime, EndTime, Easing);
            Grid.Colour = color;
        }
    }

    public class GridThicknessCommand : GridCommand
    {
        public float StartThickness { get; set; } = 0.005f;
        public float EndThickness { get; set; } = 0.005f;
        public override void Apply(double time)
        {
            var lineThickness = Interpolation.ValueAt(time, StartThickness, EndThickness, StartTime, EndTime, Easing);
            Grid.LineThickness = lineThickness;
        }
    }

    public abstract class BackgroundCommand : Command
    {
        public Box Background = new Box();
    }

    public class BackgroundColorCommand : BackgroundCommand
    {
        public Color4 StartColor { get; set; } = Color4.White;
        public Color4 EndColor { get; set; } = Color4.White;
        public override void Apply(double time)
        {
            var color = Interpolation.ValueAt(time, StartColor, EndColor, StartTime, EndTime, Easing);
            Background.Colour = color;
        }
    }

    public abstract class NotesCommand : Command
    {
        public Notes Notes = new Notes();
    }

    public class NotesShowCommand : NotesCommand
    {
        public float StartShow { get; set; } = 100.0f;
        public float EndShow { get; set; } = 100.0f;
        public override void Apply(double time) {
            var show = Interpolation.ValueAt(time, StartShow, EndShow, StartTime, EndTime, Easing);
            Notes.ShowTime = show;
        }
    }
}
