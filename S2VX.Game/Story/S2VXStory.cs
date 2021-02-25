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

        public string StoryPath { get; set; }

        public Camera Camera { get; } = new Camera();
        public RelativeBox Background { get; } = new RelativeBox();
        public Grid Grid { get; } = new Grid();
        public Notes Notes { get; } = new Notes();
        public Approaches Approaches { get; } = new Approaches();

        public IEnumerable<S2VXCommand> DefaultCommands { get; } = S2VXCommand.GetDefaultCommands();
        public List<S2VXCommand> Commands { get; private set; } = new List<S2VXCommand>();
        private int NextActive { get; set; }

        private List<S2VXCommand> Actives { get; set; } = new List<S2VXCommand>();

        private static JsonConverter[] Converters { get; } = {
            new CommandConverter(),
            new Vector2Converter(),
            new HoldNoteConverter(),
            new NoteConverter(),
        };

        public EditorSettings EditorSettings { get; private set; } = new EditorSettings();

        public DifficultySettings DifficultySettings { get; private set; } = new DifficultySettings();

        public S2VXStory() { }

        public S2VXStory(string filePath, bool isForEditor) =>
            Open(filePath, isForEditor);

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

        public void Reset() {
            ClearActives();
            Commands = new List<S2VXCommand>();
            Notes.SetChildren(new List<S2VXNote>());
            Approaches.SetChildren(new List<Approach>());
            EditorSettings = new EditorSettings();
            DifficultySettings = new DifficultySettings();
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

        public void AddHoldNote(HoldNote note) {
            Notes.AddNote(note);
            var approach = Approaches.AddHoldApproach(note);
            note.Approach = approach;
        }

        public void RemoveNote(S2VXNote note) {
            Notes.RemoveNote(note);
            Approaches.RemoveApproach(note);
        }

        // Before starting the Story in the PlayScreen, we want to explicitly
        // remove GameNotes up to some certain track time. This is so that we
        // won't hear Miss hitsounds and prematurely calculate score.
        public void RemoveNotesUpTo(double trackTime) {
            for (var index = Notes.Children.Count - 1; index >= 0; --index) {
                if (Notes.Children[index].HitTime > trackTime) {
                    break;
                }
                RemoveNote(Notes.Children[index]);
            }
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
            notes.AddRange((
                isForEditor
                    ? JsonConvert.DeserializeObject<IEnumerable<EditorHoldNote>>(story["HoldNotes"].ToString()).Cast<S2VXNote>()
                    : JsonConvert.DeserializeObject<IEnumerable<GameHoldNote>>(story["HoldNotes"].ToString()).Cast<S2VXNote>()
            ).ToList());
            notes.Sort();
            Notes.SetChildren(notes);
            var approaches = JsonConvert.DeserializeObject<List<Approach>>(story[nameof(Notes)].ToString());
            approaches.AddRange(JsonConvert.DeserializeObject<List<HoldApproach>>(story["HoldNotes"].ToString()).Cast<Approach>());
            approaches.Sort();
            Approaches.SetChildren(approaches);

            // Set approach references
            for (var i = 0; i < notes.Count; ++i) {
                notes[i].Approach = approaches[i];
            }

            EditorSettings = JsonConvert.DeserializeObject<EditorSettings>(story[nameof(EditorSettings)].ToString());
            DifficultySettings = JsonConvert.DeserializeObject<DifficultySettings>(story[nameof(DifficultySettings)].ToString());
            DifficultySettings.Calculate(this);

            StoryPath = path;
        }

        public void Save(string path) {
            var notes = Notes.GetNonHoldNotes();
            notes.Sort();
            var holdNotes = Notes.GetHoldNotes();
            holdNotes.Sort();
            DifficultySettings.Calculate(this);

            var obj = new {
                Commands,
                Notes = notes,
                HoldNotes = holdNotes,
                EditorSettings,
                DifficultySettings
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
            var isActiveAdded = false;
            while (NextActive < Commands.Count && Commands[NextActive].StartTime <= Time.Current) {
                Actives.Add(Commands[NextActive++]);
                isActiveAdded = true;
            }

            // Actives need to be sorted by EndTime to be processed in correct order. For example:
            // Commands (0-10000, 4000-11000, 6000-8000) will be sorted to (6000-8000, 0-10000, 4000-11000). 
            // Command with 11000 needs to be applied last. Command 6000-8000 will be effectively ignored. 
            if (isActiveAdded) {
                Actives = Actives.OrderBy(command => command.EndTime).ToList();
            }

            var newActives = new List<S2VXCommand>();
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
