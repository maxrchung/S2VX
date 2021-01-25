using S2VX.Game.Story;

namespace StoryMerge {
    public static class StoryMerger {
        public static Result Merge(string[] inputs, string output) {
            var validateResult = ParameterValidator.Validate(inputs, output);
            if (!validateResult.IsSuccessful) {
                return validateResult;
            }

            var (loadResult, loadedStories) = InputsLoader.Load(inputs);
            if (!loadResult.IsSuccessful) {
                return validateResult;
            }

            var outputStory = new S2VXStory();
            var notesResult = NotesMerger.Merge(loadedStories, outputStory);
            var commandsResult = CommandsMerger.Merge(loadedStories, outputStory);
            outputStory.Save(output);

            var messages = new[] {
                $"Merged {inputs.Length} stories into \"{output}\"",
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
