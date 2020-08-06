using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace S2VX.Game
{
    public class Story : CompositeDrawable
    {
        public double GameTime { get; private set; } = 0;
        public bool IsPlaying { get; private set; } = false;

        public Vector2 MousePosition { get; private set; } = Vector2.Zero;

        public Camera Camera { get; } = new Camera();
        public RelativeBox Background = new RelativeBox
        {
            Colour = Color4.Black,
        };
        public Grid Grid { get; } = new Grid();
        public Notes Notes { get; } = new Notes();
        public Approaches Approaches { get; } = new Approaches();

        public DrawableTrack Track { get; private set; } = null;

        public List<Command> Commands { get; private set; } = new List<Command>();
        private int nextActive { get; set; } = 0;
        private HashSet<Command> actives { get; set; } = new HashSet<Command>();

        private static JsonConverter[] converters { get; } = { new Vector2Converter(), new NoteConverter() };

        [BackgroundDependencyLoader]
        private void load(AudioManager audioManager)
        {
            Track = new DrawableTrack(audioManager.Tracks.Get(@"Camellia_MEGALOVANIA_Remix.mp3"));
            Track.VolumeTo(0.05f);

            Open(@"../../../story.json");

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

        public void AddCommand(Command command)
        {
            Commands.Add(command);
            Commands.Sort();
            Seek(GameTime);
        }

        public void RemoveCommand(int index)
        {
            Commands.RemoveAt(index);
            Seek(GameTime);
        }

        public void Play(bool isPlaying)
        {
            if (isPlaying)
            {
                Track.Start();
            }
            else
            {
                Track.Stop();
            }
            IsPlaying = isPlaying;
        }

        public void Restart()
        {
            GameTime = 0;
            nextActive = 0;
            actives.Clear();
            Track.Restart();
            if (!IsPlaying)
            {
                Play(false);
            }
        }

        public void Seek(double time)
        {
            nextActive = 0;
            actives.Clear();
            GameTime = time;
            Track.Seek(time);
        }

        public void Open(string path)
        {
            var text = File.ReadAllText(path);
            var story = JObject.Parse(text);
            var serializedCommands = JsonConvert.DeserializeObject<List<JObject>>(story["Commands"].ToString());
            foreach (var serializedCommand in serializedCommands)
            {
                var command = Command.FromJson(serializedCommand);
                Commands.Add(command);
            }
            Commands.Sort();

            var notes = JsonConvert.DeserializeObject<List<Note>>(story["Notes"].ToString());
            Notes.Children = notes;
            var approaches = JsonConvert.DeserializeObject<List<Approach>>(story["Notes"].ToString());
            Approaches.Children = approaches;

            Restart();
            Play(false);
        }

        public void Save(string path)
        {
            var obj = new
            {
                Commands,
                Notes = Notes.Children
            };
            var serialized = JsonConvert.SerializeObject(obj, Formatting.Indented, converters);
            File.WriteAllText(path, serialized);
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {

            MousePosition = ToLocalSpace(e.CurrentState.Mouse.Position);
            return true;
        }

        protected override void Update()
        {
            if (IsPlaying)
            {
                GameTime += Time.Elapsed;
            }

            // Add new active commands
            while (nextActive < Commands.Count && Commands[nextActive].StartTime <= GameTime)
            {
                actives.Add(Commands[nextActive++]);
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
                    active.Apply(active.EndTime, this);
                }
            }
            actives = newActives;
        }
    }
}
