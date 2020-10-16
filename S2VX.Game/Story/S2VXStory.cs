using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using S2VX.Game.Story.Command;
using S2VX.Game.Story.JSONConverters;
using S2VX.Game.Story.Note;
using S2VX.Game.Story.Settings;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace S2VX.Game.Story {
    // Per Microsoft docs, class names should not conflict with their namespace,
    // so I've prepended S2VX to fix these problems
    public class S2VXStory : CompositeDrawable {
        public double BPM { get; set; }
        public double Offset { get; set; }

        public Camera Camera { get; } = new Camera();
        public RelativeBox Background { get; } = new RelativeBox();
        public Grid Grid { get; } = new Grid();
        public Notes Notes { get; } = new Notes();
        public Approaches Approaches { get; } = new Approaches();

        public IEnumerable<S2VXCommand> DefaultCommands { get; } = S2VXCommand.GetDefaultCommands();
        public List<S2VXCommand> Commands { get; private set; } = new List<S2VXCommand>();
        private int NextActive { get; set; }

        private HashSet<S2VXCommand> Actives { get; set; } = new HashSet<S2VXCommand>();

        private static JsonConverter[] Converters { get; } = {
            new CommandConverter(),
            new Vector2Converter(),
            new NoteConverter()
        };

        public EditorSettings EditorSettings { get; private set; } = new EditorSettings();

        public MetadataSettings MetadataSettings { get; private set; } = new MetadataSettings();

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                Camera,
                Background,
                Grid,
                Notes,
                Approaches
            };
        }

        public void ClearActives() {
            NextActive = 0;
            Actives.Clear();
        }

        public void AddCommand(S2VXCommand command) {
            Commands.Add(command);
            Commands.Sort();
            ClearActives();
        }

        public void RemoveCommand(S2VXCommand command) {
            Commands.Remove(command);
            ClearActives();
        }

        public void RemoveCommand(int index) {
            Commands.RemoveAt(index);
            ClearActives();
        }

        public void AddNote(S2VXNote note) {
            Notes.AddNote(note);
            var approach = Approaches.AddApproach(note);
            note.Approach = approach;
        }

        public void RemoveNote(S2VXNote note) {
            Notes.RemoveNote(note);
            Approaches.RemoveApproach(note);
        }

        public void Open(string path, bool isForEditor) {
            Commands.Clear();
            var text = File.ReadAllText(path);
            var story = JObject.Parse(text);
            var serializedCommands = JsonConvert.DeserializeObject<List<JObject>>(story[nameof(Commands)].ToString());
            foreach (var serializedCommand in serializedCommands) {
                var command = S2VXCommand.FromJson(serializedCommand);
                Commands.Add(command);
            }
            Commands.Sort();

            var notes = (
                isForEditor
                    ? JsonConvert.DeserializeObject<IEnumerable<EditorNote>>(story[nameof(Notes)].ToString()).Cast<S2VXNote>()
                    : JsonConvert.DeserializeObject<IEnumerable<GameNote>>(story[nameof(Notes)].ToString()).Cast<S2VXNote>()
            ).ToList();
            notes.Sort();
            Notes.SetChildren(notes);
            var approaches = JsonConvert.DeserializeObject<List<Approach>>(story[nameof(Notes)].ToString());
            Approaches.SetChildren(approaches);

            // Set approach references
            for (var i = 0; i < notes.Count; ++i) {
                notes[i].Approach = approaches[i];
            }

            EditorSettings = JsonConvert.DeserializeObject<EditorSettings>(story[nameof(EditorSettings)].ToString());
            MetadataSettings = JsonConvert.DeserializeObject<MetadataSettings>(story[nameof(MetadataSettings)].ToString());
            
        }

        public void Save(string path) {
            var notes = Notes.Children;
            notes.Sort();
            var obj = new {
                Commands,
                Notes = notes,
                EditorSettings,
                MetadataSettings
            };
            var serialized = JsonConvert.SerializeObject(obj, Formatting.Indented, Converters);
            File.WriteAllText(path, serialized);
        }

        protected override void Update() {
            // If at 0, apply defaults
            if (NextActive == 0) {
                foreach (var command in DefaultCommands) {
                    command.Apply(0, this);
                }
            }

            // Add new active commands
            while (NextActive < Commands.Count && Commands[NextActive].StartTime <= Time.Current) {
                Actives.Add(Commands[NextActive++]);
            }

            var newActives = new HashSet<S2VXCommand>();
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
