using S2VX.Game.Story;
using S2VX.Game.Story.Note;
using System.Collections.Generic;

namespace StoryMerge {
    public class NotesMerger {
        private List<S2VXStory> InputStories { get; set; }
        private S2VXStory OutputStory { get; set; }

        public NotesMerger(List<S2VXStory> inputStories, S2VXStory outputStory) {
            InputStories = inputStories;
            OutputStory = outputStory;
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
        public Result Merge() {
            var infos = new List<NoteTimeInfo>();
            foreach (var input in InputStories) {
                foreach (var note in input.Notes.GetNonHoldNotes()) {
                    OutputStory.AddNote(CopyNote(note));
                    infos.Add(new NoteTimeInfo(note));
                }
                foreach (var holdNote in input.Notes.GetHoldNotes()) {
                    OutputStory.AddHoldNote(CopyHoldNote(holdNote));
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

        private static S2VXNote CopyNote(S2VXNote note) =>
            new EditorNote {
                HitTime = note.HitTime,
                Coordinates = note.Coordinates,
            };

        private static HoldNote CopyHoldNote(HoldNote holdNote) =>
            new EditorHoldNote {
                HitTime = holdNote.HitTime,
                EndTime = holdNote.EndTime,
                Coordinates = holdNote.Coordinates,
                EndCoordinates = holdNote.EndCoordinates
            };
    }
}
