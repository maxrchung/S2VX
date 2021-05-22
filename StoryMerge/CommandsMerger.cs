using S2VX.Game.Story;
using System.Collections.Generic;

namespace StoryMerge {
    public static class CommandsMerger {
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
        public static Result Merge(IEnumerable<S2VXStory> inputStories, S2VXStory outputStory) {
            var infos = new List<CommandTimeInfo>();
            foreach (var input in inputStories) {
                foreach (var command in input.Commands) {
                    outputStory.AddCommand(command);
                    infos.Add(new CommandTimeInfo(command));
                }
            }
            infos.Sort();

            var messages = new List<string>();
            var dictInfo = new Dictionary<string, CommandTimeInfo>();
            foreach (var info in infos) {
                var type = info.Type;
                if (!dictInfo.TryGetValue(type, out var latestInfo)) {
                    dictInfo[type] = info;
                    continue;
                }

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
