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
        public double GameTime { get; set; } = 0;
        public bool IsPlaying { get; set; } = false;

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

        private DrawableTrack track = null;

        private List<Command> commands { get; set; } = new List<Command>();
        private int nextActive { get; set; } = 0;
        private HashSet<Command> actives { get; set; } = new HashSet<Command>();

        [BackgroundDependencyLoader]
        private void load(AudioManager audioManager)
        {
            track = new DrawableTrack(audioManager.Tracks.Get(@"Camellia_MEGALOVANIA_Remix.mp3"));
            track.VolumeTo(0.05f);

            load(@"../../../story.json");

            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                Camera,
                Background,
                Notes,
                Grid,
                Approaches,
                new BasicMenu(Direction.Horizontal, true)
                {
                    Width = 1,
                    Height = 0.05f,
                    RelativeSizeAxes = Axes.Both,
                    Items = new[]
                    {
                        new MenuItem("File")
                        {
                            Items = new[]
                            {
                                new MenuItem("Open", open),
                                new MenuItem("Save As", save)
                            }
                        }
                    }
                }
            };
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    Play(!IsPlaying);
                    break;
                case Key.X:
                    Restart();
                    break;
            }
            return true;
        }

        private void Play(bool isPlaying)
        {
            if (isPlaying)
                track.Start();
            else
                track.Stop();
            IsPlaying = isPlaying;
        }

        private void Restart()
        {
            GameTime = 0;
            nextActive = 0;
            actives.Clear();
            track.Restart();
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

        private void load(string path)
        {
            var text = File.ReadAllText(path);
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

            Play(false);
            Restart();
        }

        private void open()
        {
            Play(false);
            var dialog = new CommonOpenFileDialog();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                load(dialog.FileName);
            } else
            {
                Play(true);
            }
        }

        private void save()
        {
            Play(false);
            var dialog = new CommonSaveFileDialog();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Console.WriteLine("Save");
            } else
            {
                Play(true);
            }
        }
    }
}
