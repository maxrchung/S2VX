using System.IO;
using System.Threading.Tasks;

namespace StoryMerge {
    public class StoryMerger {
        private string[] Inputs { get; }
        private string Output { get; }

        public StoryMerger(string[] inputs, string output) {
            Inputs = inputs;
            Output = output;
        }

        public async Task<MergeResult> Merge() {
            var validation = ValidateParameters();
            if (!validation.IsSuccessful) {
                return validation;
            }


            await File.WriteAllTextAsync(Output, "");
            return new MergeResult {
                IsSuccessful = true,
                Message = $"Merged {string.Join(',', Inputs)} to {Output}"
            };
        }

        private MergeResult ValidateParameters() {
            var result = new MergeResult { IsSuccessful = true };

            if (Inputs == null || Inputs.Length <= 2) {
                result = new MergeResult {
                    IsSuccessful = false,
                    Message = "2 or more inputs must be provided"
                };
            }

            if (Output == null) {
                result = new MergeResult {
                    IsSuccessful = false,
                    Message = "1 output must be provided"
                };
            }

            foreach (var path in Inputs) {
                if (!File.Exists(path)) {
                    result = new MergeResult {
                        IsSuccessful = false,
                        Message = $"Input file does not exist: {path}"
                    };
                }
            }

            if (!File.Exists(Output)) {
                result = new MergeResult {
                    IsSuccessful = false,
                    Message = $"Output file does not exist: {Output}"
                };
            }

            return result;
        }
    }
}
