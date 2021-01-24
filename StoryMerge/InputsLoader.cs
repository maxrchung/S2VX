using Newtonsoft.Json;
using S2VX.Game.Story;
using System.Collections.Generic;

namespace StoryMerge {
    public class InputsLoader {
        private string[] Inputs { get; set; }
        public List<S2VXStory> LoadedStories { get; private set; } = new List<S2VXStory>();

        public InputsLoader(string[] inputs) => Inputs = inputs;

        public static void LoadInputs() { }

        public Result Load() {
            foreach (var input in Inputs) {
                var story = new S2VXStory();
                try {
                    story.Open(input, true);
                } catch (JsonReaderException e) {
                    return new Result {
                        IsSuccessful = false,
                        Message = $"Input file failed to load: \"{input}\"\n{e.Message}"
                    };
                }
                LoadedStories.Add(story);
            }

            return new Result { IsSuccessful = true };
        }
    }
}
