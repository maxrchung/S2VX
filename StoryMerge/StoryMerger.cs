using Newtonsoft.Json;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StoryMerge {
    public class StoryMerger {
        private string[] Inputs { get; set; }
        private string Output { get; set; }
        private List<S2VXStory> InputStories { get; set; } = new List<S2VXStory>();

        public StoryMerger(string[] inputs, string output) {
            Inputs = inputs;
            Output = output;
        }

        public async Task<Result> Merge() {
            var result = ValidateParameters();
            if (!result.IsSuccessful) {
                return result;
            }

            result = LoadInputs();
            if (!result.IsSuccessful) {
                return result;
            }

            await File.WriteAllTextAsync(Output, "");
            return new Result {
                IsSuccessful = true,
                Message = $"Merged {string.Join(',', Inputs)} to {Output}"
            };
        }

        public Result ValidateParameters() {
            if (Inputs == null || Inputs.Length < 2) {
                return new Result {
                    IsSuccessful = false,
                    Message = "2 or more inputs must be provided"
                };
            }

            if (string.IsNullOrWhiteSpace(Output)) {
                return new Result {
                    IsSuccessful = false,
                    Message = "1 output must be provided"
                };
            }

            foreach (var path in Inputs) {
                if (!File.Exists(path)) {
                    return new Result {
                        IsSuccessful = false,
                        Message = $"Input file does not exist: {path}"
                    };
                }
            }

            return new Result { IsSuccessful = true };
        }

        public Result LoadInputs() {
            foreach (var input in Inputs) {
                var story = new S2VXStory();
                try {
                    story.Open(input, true);
                } catch (JsonReaderException e) {
                    return new Result {
                        IsSuccessful = false,
                        Message = $"Failed to load: {input}\n{e.Message}"
                    };
                }
                InputStories.Add(story);
            }

            return new Result { IsSuccessful = true };
        }

        public Result MergeNotes(S2VXStory story) {
            var infos = new List<NoteInfo>();
            foreach (var input in InputStories) {
                foreach (var note in input.Notes.GetNonHoldNotes()) {
                    story.AddNote(CopyNote(note));
                    infos.Add(new NoteInfo(note));
                }
                foreach (var holdNote in input.Notes.GetHoldNotes()) {
                    story.AddHoldNote(CopyHoldNote(holdNote));
                    infos.Add(new NoteInfo(holdNote));
                }
            }
            infos.Sort();

            var messages = new List<string>();
            var latestInfo = infos.FirstOrDefault();
            for (var i = 1; i < infos.Count; ++i) {
                var info = infos[i];
                if (info.StartTime >= latestInfo.EndTime && info.EndTime > latestInfo.EndTime) {
                    latestInfo = info;
                } else {
                    messages.Add($"Note conflict:\n{latestInfo}\n{info}");
                    if (info.EndTime > latestInfo.EndTime) {
                        latestInfo = info;
                    }
                }
            }

            return new Result {
                IsSuccessful = true,
                Message = string.Join('\n', messages)
            };
        }

        public static S2VXNote CopyNote(S2VXNote note) =>
            new EditorNote {
                HitTime = note.HitTime,
                Coordinates = note.Coordinates,
            };

        public static HoldNote CopyHoldNote(HoldNote holdNote) =>
            new EditorHoldNote {
                HitTime = holdNote.HitTime,
                EndTime = holdNote.EndTime,
                Coordinates = holdNote.Coordinates,
                EndCoordinates = holdNote.EndCoordinates
            };
    }
}
