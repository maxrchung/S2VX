using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
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
        private Camera camera = new Camera();

        private Box background = new Box
        {
            Colour = Color4.CornflowerBlue,
            RelativeSizeAxes = Axes.Both
        };

        private Grid grid = new Grid();

        [Cached]
        private Notes notes = new Notes();

        [Cached]
        private Approaches approaches = new Approaches();

        private List<Command> commands = new List<Command>();
        private int nextActive = 0;
        private HashSet<Command> actives = new HashSet<Command>();

        [BackgroundDependencyLoader]
        private void load()
        {
            var dir = Directory.GetCurrentDirectory();
            var story = File.ReadAllText(@"..\..\..\story.json");
            var command = JsonConvert.DeserializeObject<CameraMoveCommand>(story);
            commands.Sort();

            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                camera,
                background,
                notes,
                grid,
                approaches
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
                active.Apply(time);

                // Remove finished commands
                if (active.EndTime >= time)
                {
                    newActives.Add(active);
                }
                else
                {
                    // Ensure command end will always trigger
                    active.Apply(time);
                }
            }
            actives = newActives;
        }
    }
}
