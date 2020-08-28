using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using osuTK.Graphics;
using System.Collections.Generic;
using System.IO;

namespace S2VX.Game.Story {
    // Per Microsoft docs, class names should not conflict with their namespace,
    // so I've prepended S2VX to fix these problems
    public class S2VXStory : CompositeDrawable {
        public double BPM { get; set; }
        public double Offset { get; set; }

        public Camera Camera { get; } = new Camera();
        public RelativeBox Background { get; } = new RelativeBox {
            Colour = Color4.Black,
        };
        public Grid Grid { get; } = new Grid();
        public Notes Notes { get; } = new Notes();

        public List<Command> Commands { get; private set; } = new List<Command>();
        private int NextActive { get; set; }
        private HashSet<Command> Actives { get; set; } = new HashSet<Command>();

        private static JsonConverter[] Converters { get; } = { new Vector2Converter(), new NoteConverter() };

        [BackgroundDependencyLoader]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                Camera,
                Background,
                Grid,
                Notes,
            };
        }

        public void ClearActives() {
            NextActive = 0;
            Actives.Clear();
        }

        public void AddCommand(Command command) {
            Commands.Add(command);
            Commands.Sort();
            ClearActives();
        }

        public void RemoveCommand(int index) {
            Commands.RemoveAt(index);
            ClearActives();
        }

        public void AddNote(Vector2 position, double time) => Notes.AddNote(position, time);

        public void DeleteNote(Note note) => Notes.DeleteNote(note);

        public void Open(string path) {
            Commands.Clear();
            var text = File.ReadAllText(path);
            var story = JObject.Parse(text);
            var serializedCommands = JsonConvert.DeserializeObject<List<JObject>>(story[nameof(Commands)].ToString());
            foreach (var serializedCommand in serializedCommands) {
                var command = Command.FromJson(serializedCommand);
                Commands.Add(command);
            }
            Commands.Sort();

            var notes = JsonConvert.DeserializeObject<List<Note>>(story[nameof(Notes)].ToString());
            Notes.SetChildren(notes);
        }

        public void Save(string path) {
            var obj = new {
                Commands,
                Notes = Notes.Children
            };
            var serialized = JsonConvert.SerializeObject(obj, Formatting.Indented, Converters);
            File.WriteAllText(path, serialized);
        }

        protected override void Update() {
            // Add new active commands
            while (NextActive < Commands.Count && Commands[NextActive].StartTime <= Time.Current) {
                Actives.Add(Commands[NextActive++]);
            }

            var newActives = new HashSet<Command>();
            foreach (var active in Actives) {
                // Run active commands
                active.Apply(Time.Current, this);

                // Remove finished commands
                if (active.EndTime >= Time.Current) {
                    newActives.Add(active);
                } else {
                    // Ensure command end will always trigger
                    active.Apply(active.EndTime, this);
                }
            }
            Actives = newActives;
        }
    }
}
