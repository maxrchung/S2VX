using S2VX.Game.Story;
using System;
using System.Collections.Generic;

namespace StoryMerge {
    public static class InputsLoader {
        public static (Result result, List<S2VXStory> loadedStories) Load(string[] inputs) {
            var loadedStories = new List<S2VXStory>();
            foreach (var input in inputs) {
                var story = new S2VXStory();
                try {
                    story.Open(input, true);
                } catch (Exception e) {
                    return (
                        new Result {
                            IsSuccessful = false,
                            Message = $"Input file failed to load: \"{input}\"\n{e.Message}"
                        },
                        loadedStories
                    );
                }
                loadedStories.Add(story);
            }

            return (new Result { IsSuccessful = true }, loadedStories);
        }
    }
}
