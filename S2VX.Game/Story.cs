using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game
{
    public class Story : CompositeDrawable
    {
        [Cached]
        public Camera Camera = new Camera();

        public Box Background = new Box
        {
            Colour = Color4.CornflowerBlue,
            RelativeSizeAxes = Axes.Both
        };

        public Grid Grid = new Grid();

        [Cached]
        public Notes Notes = new Notes();

        [Cached]
        public Approaches Approaches = new Approaches();

        private List<Command> commands = new List<Command>();
        private int nextActive = 0;
        private HashSet<Command> actives = new HashSet<Command>();

        [BackgroundDependencyLoader]
        private void load()
        {
            var story = File.ReadAllText(@"..\..\..\story.json");
            var serializedCommands = JsonConvert.DeserializeObject<List<JObject>>(story);
            foreach (var serializedCommand in serializedCommands)
            {
                var serial = serializedCommand.ToString();
                Command command = null;
                switch (Enum.Parse(typeof(Commands), serializedCommand["Type"].ToString()))
                {
                    case Commands.CameraMove:
                        command = JsonConvert.DeserializeObject<CameraMoveCommand>(serial); break;
                    case Commands.CameraRotate:
                        command = JsonConvert.DeserializeObject<CameraRotateCommand>(serial); break;
                    case Commands.CameraScale:
                        command = JsonConvert.DeserializeObject<CameraScaleCommand>(serial); break;
                    case Commands.GridAlpha:
                        command = JsonConvert.DeserializeObject<GridAlphaCommand>(serial); break;
                    case Commands.GridColor:
                        command = JsonConvert.DeserializeObject<GridColorCommand>(serial); break;
                    case Commands.GridThickness:
                        command = JsonConvert.DeserializeObject<GridThicknessCommand>(serial); break;
                    case Commands.BackgroundColor:
                        command = JsonConvert.DeserializeObject<BackgroundColorCommand>(serial); break;
                    case Commands.NotesAlpha:
                        command = JsonConvert.DeserializeObject<NotesAlphaCommand>(serial); break;
                    case Commands.NotesColor:
                        command = JsonConvert.DeserializeObject<NotesColorCommand>(serial); break;
                    case Commands.NotesFadeInTime:
                        command = JsonConvert.DeserializeObject<NotesFadeInTimeCommand>(serial); break;
                    case Commands.NotesShowTime:
                        command = JsonConvert.DeserializeObject<NotesShowTimeCommand>(serial); break;
                    case Commands.NotesFadeOutTime:
                        command = JsonConvert.DeserializeObject<NotesFadeOutTimeCommand>(serial); break;
                    case Commands.ApproachesDistance:
                        command = JsonConvert.DeserializeObject<ApproachesDistanceCommand>(serial); break;
                    case Commands.ApproachesThickness:
                        command = JsonConvert.DeserializeObject<ApproachesThicknessCommand>(serial); break;
                }
                if (command != null)
                {
                    commands.Add(command);
                }
            }
            commands.Sort();

            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                Camera,
                Background,
                Notes,
                Grid,
                Approaches
            };
        }

        protected override void Update()
        {
            var time = Time.Current;
            // Add new active commands
            while (nextActive < commands.Count && commands[nextActive].StartTime <= time)
            {
                actives.Add(commands[nextActive++]);
            }

            var newActives = new HashSet<Command>();
            foreach (var active in actives)
            {
                // Run active commands
                active.Apply(time, this);

                // Remove finished commands
                if (active.EndTime >= time)
                {
                    newActives.Add(active);
                }
                else
                {
                    // Ensure command end will always trigger
                    active.Apply(time, this);
                }
            }
            actives = newActives;
        }
    }
}
