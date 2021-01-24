
using S2VX.Game.Story;
using System.Collections.Generic;
using System.IO;

namespace StoryMerge {
    public class ParameterValidator {
        private string[] Inputs { get; set; }
        private string Output { get; set; }
        private List<S2VXStory> InputStories { get; set; } = new List<S2VXStory>();

        public ParameterValidator(string[] inputs, string output) {
            Inputs = inputs;
            Output = output;
        }

        public Result Validate() {
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
                        Message = $"Input file does not exist: \"{path}\""
                    };
                }
            }

            return new Result { IsSuccessful = true };
        }

    }
}
