using S2VX.Game.Story;
using System.Collections.Generic;

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
            var result = new ParameterValidator(Inputs, Output).Validate();
            if (!result.IsSuccessful) {
                return result;
            }

            var loader = new InputsLoader(Inputs);
            result = loader.Load();
            if (!result.IsSuccessful) {
                return result;
            }

            var outputStory = new S2VXStory();
            var notesResult = new NotesMerger(loader.LoadedStories, outputStory).Merge();
            var commandsResult = new CommandsMerger(loader.LoadedStories, outputStory).Merge();
            outputStory.Save(Output);

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
    }
}
