using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;
using S2VX.Game.Editor;
using S2VX.Game.Story.Command;
using S2VX.Game.Story.JSONConverters;
using S2VX.Game.Story.Note;
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
        public RelativeBox Background { get; } = new RelativeBox {
            Colour = Color4.Black,
        };
        public Grid Grid { get; } = new Grid();
        public Notes Notes { get; } = new Notes();
        public Approaches Approaches { get; } = new Approaches();

        public List<S2VXCommand> Commands { get; private set; } = new List<S2VXCommand>();
        private int NextActive { get; set; }

        private HashSet<S2VXCommand> Actives { get; set; } = new HashSet<S2VXCommand>();

        private static JsonConverter[] Converters { get; } = {
            new CommandConverter(),
            new Vector2Converter(),
            new NoteConverter()
        };

        private EditorSettings EditorSettings = new EditorSettings();

        public EditorSettings GetEditorSettings() => EditorSettings;
        public void SetEditorSettings(EditorSettings value) => EditorSettings = value;

        [BackgroundDependencyLoader]
        private void Load() {
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

        // Before starting the Story in the PlayScreen, we want to explicitly
        // remove GameNotes up to some certain track time. This is so that we
        // won't hear Miss hitsounds and prematurely calculate score.
        public void RemoveNotesUpTo(double trackTime) {
            while (Notes.Children.Count > 0 && Notes.Children.Last().EndTime < trackTime) {
                // This seems somewhat inefficient since I believe Children has
                // to be reshuffled each removal, but I don't think osu! has
                // easy ways of removing multiple internal children at once. You
                // can't just use Notes.SetChildren() directly because calling
                // this would invalidate all of the existing Notes.
                RemoveNote(Notes.Children.Last());
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
            notes.Sort();
            Notes.SetChildren(notes);
            var approaches = JsonConvert.DeserializeObject<List<Approach>>(story[nameof(Notes)].ToString());
            Approaches.SetChildren(approaches);

            // Set approach references
            for (var i = 0; i < notes.Count; ++i) {
                notes[i].Approach = approaches[i];
            }

            SetEditorSettings(JsonConvert.DeserializeObject<EditorSettings>(story[nameof(EditorSettings)].ToString()));
        }

        public void Save(string path) {
            var notes = Notes.Children;
            notes.Sort();
            var obj = new {
                Commands,
                Notes = notes,
                EditorSettings = GetEditorSettings(),
            };
            var serialized = JsonConvert.SerializeObject(obj, Formatting.Indented, Converters);
            File.WriteAllText(path, serialized);
        }

        protected override void Update() {
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
