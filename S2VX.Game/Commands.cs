using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game
{
    public enum Commands
    {
        None,
        CameraMove,
        CameraRotate,
        CameraScale,
        GridAlpha,
        GridColor,
        GridThickness,
        BackgroundColor,
        NotesFadeInTime,
        NotesShowTime,
        NotesFadeOutTime,
        NotesAlpha,
        NotesColor,
        Approaches,
        ApproachesDistance,
        ApproachesThickness
    }

    public abstract class Command : IComparable<Command>
    {
        public Commands Type = Commands.None;
        public double StartTime { get; set; } = 0;
        public double EndTime { get; set; } = 0;
        public Easing Easing { get; set; } = Easing.None;
        public abstract void Apply(double time);

        public int CompareTo(Command other)
        {
            return StartTime.CompareTo(other.StartTime);
        }

        public static Command Load(string data, Camera camera, Notes notes, Approaches approaches)
        {
            var data =
        }
    }

    public abstract class CameraCommand : Command
    {
        public Camera Camera = new Camera();
    }

    public class CameraMoveCommand : CameraCommand
    {
        public Commands Type = Commands.CameraMove;
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
        public Commands Type = Commands.CameraRotate;
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
        public Commands Type = Commands.CameraScale;
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
        public Commands Type = Commands.GridAlpha;
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
        public Commands Type = Commands.GridColor;
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
        public Commands Type = Commands.GridThickness;
        public float StartThickness { get; set; } = 0.005f;
        public float EndThickness { get; set; } = 0.005f;
        public override void Apply(double time)
        {
            var thickness = Interpolation.ValueAt(time, StartThickness, EndThickness, StartTime, EndTime, Easing);
            Grid.Thickness = thickness;
        }
    }

    public abstract class BackgroundCommand : Command
    {
        public Box Background = new Box();
    }

    public class BackgroundColorCommand : BackgroundCommand
    {
        public Commands Type = Commands.BackgroundColor;
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

    public class NotesFadeInTimeCommand : NotesCommand
    {
        public Commands Type = Commands.NotesFadeInTime;
        public float StartFadeInTime { get; set; } = 100.0f;
        public float EndFadeInTime { get; set; } = 100.0f;
        public override void Apply(double time)
        {
            var fadeInTime = Interpolation.ValueAt(time, StartFadeInTime, EndFadeInTime, StartTime, EndTime, Easing);
            Notes.FadeInTime = fadeInTime;
        }
    }

    public class NotesShowTimeCommand : NotesCommand
    {
        public Commands Type = Commands.NotesShowTime;
        public float StartShowTime { get; set; } = 100.0f;
        public float EndShowTime { get; set; } = 100.0f;
        public override void Apply(double time)
        {
            var showTime = Interpolation.ValueAt(time, StartShowTime, EndShowTime, StartTime, EndTime, Easing);
            Notes.ShowTime = showTime;
        }
    }

    public class NotesFadeOutTimeCommand : NotesCommand
    {
        public Commands Type = Commands.NotesFadeOutTime;
        public float StartFadeOutTime { get; set; } = 100.0f;
        public float EndFadeOutTime { get; set; } = 100.0f;
        public override void Apply(double time)
        {
            var fadeOutTime = Interpolation.ValueAt(time, StartFadeOutTime, EndFadeOutTime, StartTime, EndTime, Easing);
            Notes.FadeOutTime = fadeOutTime;
        }
    }

    public class NotesAlphaCommand : NotesCommand
    {
        public Commands Type = Commands.NotesAlpha;
        public float StartAlpha { get; set; } = 1;
        public float EndAlpha { get; set; } = 1;
        public override void Apply(double time)
        {
            var alpha = Interpolation.ValueAt(time, StartAlpha, EndAlpha, StartTime, EndTime, Easing);
            Notes.Alpha = alpha;
        }
    }
    public class NotesColorCommand : NotesCommand
    {
        public Commands Type = Commands.NotesColor;
        public Color4 StartColor { get; set; } = Color4.White;
        public Color4 EndColor { get; set; } = Color4.White;
        public override void Apply(double time)
        {
            var color = Interpolation.ValueAt(time, StartColor, EndColor, StartTime, EndTime, Easing);
            Notes.Colour = color;
        }
    }

    public abstract class ApproachesCommand : Command
    {
        public Approaches Approaches = new Approaches();
    }

    public class ApproachesDistanceCommand : ApproachesCommand
    {
        public Commands Type = Commands.ApproachesDistance;
        public float StartDistance { get; set; } = 0.5f;
        public float EndDistance { get; set; } = 0.5f;
        public override void Apply(double time)
        {
            var distance = Interpolation.ValueAt(time, StartDistance, EndDistance, StartTime, EndTime, Easing);
            Approaches.Distance = distance;
        }
    }

    public class ApproachesThicknessCommand : ApproachesCommand
    {
        public Commands Type = Commands.ApproachesThickness;
        public float StartThickness { get; set; } = 0.005f;
        public float EndThickness { get; set; } = 0.005f;
        public override void Apply(double time)
        {
            var thickness = Interpolation.ValueAt(time, StartThickness, EndThickness, StartTime, EndTime, Easing);
            Approaches.Thickness = thickness;
        }
    }
}
