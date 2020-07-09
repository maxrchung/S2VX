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
                var type = Enum.Parse<Commands>(serializedCommand["Type"].ToString());
                var command = Command.Load(type, serializedCommand.ToString(), this);
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
