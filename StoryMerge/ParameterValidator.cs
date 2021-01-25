using System.IO;

namespace StoryMerge {
    public static class ParameterValidator {
        public static Result Validate(string[] inputs, string output) {
            if (inputs == null || inputs.Length < 2) {
                return new Result {
                    IsSuccessful = false,
                    Message = "2 or more inputs must be provided"
                };
            }

            if (string.IsNullOrWhiteSpace(output)) {
                return new Result {
                    IsSuccessful = false,
                    Message = "1 output must be provided"
                };
            }

            foreach (var path in inputs) {
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
