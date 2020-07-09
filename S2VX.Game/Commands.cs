using System;
using Newtonsoft.Json;
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
        public abstract Commands Type { get; set; }
        public double StartTime { get; set; } = 0;
        public double EndTime { get; set; } = 0;
        public Easing Easing { get; set; } = Easing.None;
        public abstract void Apply(double time, Story story);

        public int CompareTo(Command other)
        {
            return StartTime.CompareTo(other.StartTime);
        }

        public static Command Load(Commands type, string data, Story story)
        {
            Command command = null;
            switch (type)
            {
                case Commands.CameraMove: command = JsonConvert.DeserializeObject<CameraMoveCommand>(data); break;
                case Commands.CameraRotate: command = JsonConvert.DeserializeObject<CameraRotateCommand>(data); break;
                case Commands.CameraScale: command = JsonConvert.DeserializeObject<CameraScaleCommand>(data); break;
                case Commands.GridAlpha: command = JsonConvert.DeserializeObject<GridAlphaCommand>(data); break;
                case Commands.GridColor: command = JsonConvert.DeserializeObject<GridColorCommand>(data); break;
                case Commands.GridThickness: command = JsonConvert.DeserializeObject<GridThicknessCommand>(data); break;
                case Commands.BackgroundColor: command = JsonConvert.DeserializeObject<BackgroundColorCommand>(data); break;
                case Commands.NotesAlpha: command = JsonConvert.DeserializeObject<NotesAlphaCommand>(data); break;
                case Commands.NotesColor: command = JsonConvert.DeserializeObject<NotesColorCommand>(data); break;
                case Commands.NotesFadeInTime: command = JsonConvert.DeserializeObject<NotesFadeInTimeCommand>(data); break;
                case Commands.NotesShowTime: command = JsonConvert.DeserializeObject<NotesShowTimeCommand>(data); break;
                case Commands.NotesFadeOutTime: command = JsonConvert.DeserializeObject<NotesFadeOutTimeCommand>(data); break;
                case Commands.ApproachesDistance: command = JsonConvert.DeserializeObject<ApproachesDistanceCommand>(data); break;
                case Commands.ApproachesThickness: command = JsonConvert.DeserializeObject<ApproachesThicknessCommand>(data); break;
            }
            return command;
        }
    }


    public class CameraMoveCommand : Command
    {
        public override Commands Type { get; set; } = Commands.CameraMove;
        public Vector2 StartPosition { get; set; } = Vector2.Zero;
        public Vector2 EndPosition { get; set; } = Vector2.Zero;
        public override void Apply(double time, Story story)
        {
            var position = Interpolation.ValueAt(time, StartPosition, EndPosition, StartTime, EndTime, Easing);
            story.Camera.Position = position;
        }
    }

    public class CameraRotateCommand : Command
    {
        public override Commands Type { get; set; } = Commands.CameraRotate;
        public float StartRotation { get; set; } = 0;
        public float EndRotation { get; set; } = 0;
        public override void Apply(double time, Story story)
        {
            var rotation = Interpolation.ValueAt(time, StartRotation, EndRotation, StartTime, EndTime, Easing);
            story.Camera.Rotation = rotation;
        }
    }

    public class CameraScaleCommand : Command
    {
        public override Commands Type { get; set; } = Commands.CameraScale;
        public Vector2 StartScale { get; set; } = new Vector2(0.1f);
        public Vector2 EndScale { get; set; } = new Vector2(0.1f);
        public override void Apply(double time, Story story)
        {
            var scale = Interpolation.ValueAt(time, StartScale, EndScale, StartTime, EndTime, Easing);
            story.Camera.Scale = scale;
        }
    }

    public abstract class GridCommand : Command
    {
        public Grid Grid = new Grid();
    }

    public class GridAlphaCommand : Command
    {
        public override Commands Type { get; set; } = Commands.GridAlpha;
        public float StartAlpha { get; set; } = 1;
        public float EndAlpha { get; set; } = 1;
        public override void Apply(double time, Story story)
        {
            var alpha = Interpolation.ValueAt(time, StartAlpha, EndAlpha, StartTime, EndTime, Easing);
            story.Grid.Alpha = alpha;
        }
    }

    public class GridColorCommand : Command
    {
        public override Commands Type { get; set; } = Commands.GridColor;
        public Color4 StartColor { get; set; } = Color4.White;
        public Color4 EndColor { get; set; } = Color4.White;
        public override void Apply(double time, Story story)
        {
            var color = Interpolation.ValueAt(time, StartColor, EndColor, StartTime, EndTime, Easing);
            story.Grid.Colour = color;
        }
    }

    public class GridThicknessCommand : Command
    {
        public override Commands Type { get; set; } = Commands.GridThickness;
        public float StartThickness { get; set; } = 0.005f;
        public float EndThickness { get; set; } = 0.005f;
        public override void Apply(double time, Story story)
        {
            var thickness = Interpolation.ValueAt(time, StartThickness, EndThickness, StartTime, EndTime, Easing);
            story.Grid.Thickness = thickness;
        }
    }

    public class BackgroundColorCommand : Command
    {
        public override Commands Type { get; set; } = Commands.BackgroundColor;
        public Color4 StartColor { get; set; } = Color4.White;
        public Color4 EndColor { get; set; } = Color4.White;
        public override void Apply(double time, Story story)
        {
            var color = Interpolation.ValueAt(time, StartColor, EndColor, StartTime, EndTime, Easing);
            story.Background.Colour = color;
        }
    }

    public class NotesFadeInTimeCommand : Command
    {
        public override Commands Type { get; set; } = Commands.NotesFadeInTime;
        public float StartFadeInTime { get; set; } = 100.0f;
        public float EndFadeInTime { get; set; } = 100.0f;
        public override void Apply(double time, Story story)
        {
            var fadeInTime = Interpolation.ValueAt(time, StartFadeInTime, EndFadeInTime, StartTime, EndTime, Easing);
            story.Notes.FadeInTime = fadeInTime;
        }
    }

    public class NotesShowTimeCommand : Command
    {
        public override Commands Type { get; set; } = Commands.NotesShowTime;
        public float StartShowTime { get; set; } = 100.0f;
        public float EndShowTime { get; set; } = 100.0f;
        public override void Apply(double time, Story story)
        {
            var showTime = Interpolation.ValueAt(time, StartShowTime, EndShowTime, StartTime, EndTime, Easing);
            story.Notes.ShowTime = showTime;
        }
    }

    public class NotesFadeOutTimeCommand : Command
    {
        public override Commands Type { get; set; } = Commands.NotesFadeOutTime;
        public float StartFadeOutTime { get; set; } = 100.0f;
        public float EndFadeOutTime { get; set; } = 100.0f;
        public override void Apply(double time, Story story)
        {
            var fadeOutTime = Interpolation.ValueAt(time, StartFadeOutTime, EndFadeOutTime, StartTime, EndTime, Easing);
            story.Notes.FadeOutTime = fadeOutTime;
        }
    }

    public class NotesAlphaCommand : Command
    {
        public override Commands Type { get; set; } = Commands.NotesAlpha;
        public float StartAlpha { get; set; } = 1;
        public float EndAlpha { get; set; } = 1;
        public override void Apply(double time, Story story)
        {
            var alpha = Interpolation.ValueAt(time, StartAlpha, EndAlpha, StartTime, EndTime, Easing);
            story.Notes.Alpha = alpha;
        }
    }
    public class NotesColorCommand : Command
    {
        public override Commands Type { get; set; } = Commands.NotesColor;
        public Color4 StartColor { get; set; } = Color4.White;
        public Color4 EndColor { get; set; } = Color4.White;
        public override void Apply(double time, Story story)
        {
            var color = Interpolation.ValueAt(time, StartColor, EndColor, StartTime, EndTime, Easing);
            story.Notes.Colour = color;
        }
    }

    public class ApproachesDistanceCommand : Command
    {
        public override Commands Type { get; set; } = Commands.ApproachesDistance;
        public float StartDistance { get; set; } = 0.5f;
        public float EndDistance { get; set; } = 0.5f;
        public override void Apply(double time, Story story)
        {
            var distance = Interpolation.ValueAt(time, StartDistance, EndDistance, StartTime, EndTime, Easing);
            story.Approaches.Distance = distance;
        }
    }

    public class ApproachesThicknessCommand : Command
    {
        public override Commands Type { get; set; } = Commands.ApproachesThickness;
        public float StartThickness { get; set; } = 0.005f;
        public float EndThickness { get; set; } = 0.005f;
        public override void Apply(double time, Story story)
        {
            var thickness = Interpolation.ValueAt(time, StartThickness, EndThickness, StartTime, EndTime, Easing);
            story.Approaches.Thickness = thickness;
        }
    }
}
