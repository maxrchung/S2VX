using S2VX.Game.Story;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace StoryMerge {
    public class StoryMerger {
        private string[] Inputs { get; set; }
        private string Output { get; set; }

        public StoryMerger(string[] inputs, string output) {
            Inputs = inputs;
            Output = output;
        }

        public async Task<Result> Merge() {
            var result = ValidateParameters();
            if (!result.IsSuccessful) {
                return result;
            }

            result = ValidateInputs();
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

        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        public Result ValidateInputs() {
            foreach (var input in Inputs) {
                var story = new S2VXStory();
                try {
                    story.Open(input, false);
                } catch (Exception e) {
                    return new Result {
                        IsSuccessful = false,
                        Message = $"Failed to load: {input}\n{e.Message}"
                    };
                }
            }

            return new Result { IsSuccessful = true };
        }
    }
}
