using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game
{
    public class Story : CompositeDrawable
    {
        public double GameTime { get; set; } = 0;
        public bool IsPlaying { get; set; } = true;

        [Cached]
        public Camera Camera { get; } = new Camera();
        public RelativeBox Background = new RelativeBox
        {
            Colour = Color4.Black,
        };
        public Grid Grid { get; } = new Grid();
        [Cached]
        public Notes Notes { get; } = new Notes();
        [Cached]
        public Approaches Approaches { get; } = new Approaches();

        private List<Command> commands { get; set; } = new List<Command>();
        private int nextActive { get; set; } = 0;
        private HashSet<Command> actives { get; set; } = new HashSet<Command>();

        public Track Track = null;

        public void Restart()
        {
            GameTime = 0;
            nextActive = 0;
            actives.Clear();
        }
        
        [BackgroundDependencyLoader]
        private void load(AudioManager audioManager)
        {
            Track = audioManager.Tracks.Get("");

            var text = File.ReadAllText(@"Stories/Camellia_MEGALOVANIA_Remix/story.json");
            var story = JObject.Parse(text);
            var serializedCommands = JsonConvert.DeserializeObject<List<JObject>>(story["Commands"].ToString());
            foreach (var serializedCommand in serializedCommands)
            {
                var type = Enum.Parse<Commands>(serializedCommand["Type"].ToString());
                var command = Command.Load(type, serializedCommand.ToString(), this);
                commands.Add(command);
            }
            commands.Sort();

            var notes = JsonConvert.DeserializeObject<List<Note>>(story["Notes"].ToString());
            Notes.Children = notes;
            var approaches = JsonConvert.DeserializeObject<List<Approach>>(story["Notes"].ToString());
            Approaches.Children = approaches;

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
            if (IsPlaying)
            {
                GameTime += Time.Elapsed;
            }

            // Add new active commands
            while (nextActive < commands.Count && commands[nextActive].StartTime <= GameTime)
            {
                actives.Add(commands[nextActive++]);
            }

            var newActives = new HashSet<Command>();
            foreach (var active in actives)
            {
                // Run active commands
                active.Apply(GameTime, this);

                // Remove finished commands
                if (active.EndTime >= GameTime)
                {
                    newActives.Add(active);
                }
                else
                {
                    // Ensure command end will always trigger
                    active.Apply(GameTime, this);
                }
            }
            actives = newActives;
        }
    }
}
