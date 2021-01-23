using Newtonsoft.Json;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;
using System.Collections.Generic;
using System.IO;

namespace StoryMerge {
    public class StoryMerger {
        private string[] Inputs { get; set; }
        private string Output { get; set; }
        private List<S2VXStory> InputStories { get; set; } = new List<S2VXStory>();

        public StoryMerger(string[] inputs, string output) {
            Inputs = inputs;
            Output = output;
        }

        public Result Merge() {
            var result = ValidateParameters();
            if (!result.IsSuccessful) {
                return result;
            }

            result = LoadInputs();
            if (!result.IsSuccessful) {
                return result;
            }

            var story = new S2VXStory();
            var notesResult = MergeNotes(story);
            var commandsResult = MergeCommands(story);
            story.Save(Output);

            var messages = new[] {
                $"Merged {Inputs.Length} stories into \"{Output}\"",
                notesResult.Message,
                commandsResult.Message
            };
            var message = string.Join("\n\n", messages);

            return new Result {
                IsSuccessful = true,
                Message = message
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

        /// <summary>
        /// Note that conflicts will be reported for any notes that share the
        /// same start/end times. This is to prevent a player from trying to
        /// play multiple notes at one time.
        /// 
        /// Example time ranges that are not conflicts:
        /// (0-0, 500-500, 1000-1000)
        /// (0-0, 500-1000)
        /// 
        /// Example time ranges that are conflicts:
        /// (0-0, 0-0)
        /// (0-0, 0-1000)
        /// (0-1000, 500-1500)
        /// </summary>
        public Result MergeNotes(S2VXStory story) {
            var infos = new List<NoteTimeInfo>();
            foreach (var input in InputStories) {
                foreach (var note in input.Notes.GetNonHoldNotes()) {
                    story.AddNote(CopyNote(note));
                    infos.Add(new NoteTimeInfo(note));
                }
                foreach (var holdNote in input.Notes.GetHoldNotes()) {
                    story.AddHoldNote(CopyHoldNote(holdNote));
                    infos.Add(new NoteTimeInfo(holdNote));
                }
            }
            infos.Sort();

            var messages = new List<string>();
            NoteTimeInfo latestInfo = null;
            foreach (var info in infos) {
                if (latestInfo == null) {
                    latestInfo = info;
                    continue;
                }

                if (info.StartTime > latestInfo.EndTime && info.EndTime > latestInfo.EndTime) {
                    latestInfo = info;
                } else {
                    messages.Add($"Note conflict:\n{latestInfo}\n{info}");
                    if (info.EndTime > latestInfo.EndTime) {
                        latestInfo = info;
                    }
                }
            }

            if (messages.Count == 0) {
                messages.Add("No note conflicts found");
            }

            return new Result {
                IsSuccessful = true,
                Message = string.Join("\n\n", messages)
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

        /// <summary>
        /// Note that different from notes, commands can share time boundaries.
        /// For example, (0-0, 0-1000) is valid. However, duplicate commands,
        /// e.g. (0-0, 0-0) are not allowed.
        /// 
        /// Example time ranges that are not conflicts:
        /// (0-0, 500-500, 1000-1000)
        /// (0-0, 500-1000)
        /// (0-0, 0-1000)
        /// 
        /// Example time ranges that are conflicts:
        /// (0-0, 0-0)
        /// (0-1000, 500-1500)
        /// </summary>
        public Result MergeCommands(S2VXStory story) {
            var infos = new List<CommandTimeInfo>();
            foreach (var input in InputStories) {
                foreach (var command in input.Commands) {
                    story.AddCommand(command);
                    infos.Add(new CommandTimeInfo(command));
                }
            }
            infos.Sort();

            var messages = new List<string>();
            var dictInfo = new Dictionary<string, CommandTimeInfo>();
            foreach (var info in infos) {
                var type = info.Type;
                if (!dictInfo.ContainsKey(type)) {
                    dictInfo[type] = info;
                    continue;
                }

                var latestInfo = dictInfo[type];
                if (info.StartTime == latestInfo.StartTime && info.EndTime == latestInfo.EndTime) {
                    messages.Add($"Command conflict:\n{latestInfo}\n{info}");
                } else if (info.StartTime >= latestInfo.EndTime && info.EndTime >= latestInfo.EndTime) {
                    dictInfo[type] = info;
                } else {
                    messages.Add($"Command conflict:\n{latestInfo}\n{info}");
                    if (info.EndTime > latestInfo.EndTime) {
                        dictInfo[type] = info;
                    }
                }
            }

            if (messages.Count == 0) {
                messages.Add("No command conflicts found");
            }

            return new Result {
                IsSuccessful = true,
                Message = string.Join("\n\n", messages)
            };
        }
    }
}
