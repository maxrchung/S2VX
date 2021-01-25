using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Threading.Tasks;

namespace StoryMerge {
    internal class Program {
        private static async Task<int> Main(string[] args) {
            var cmd = new RootCommand
            {
                new Option<string[]>(
                    new []{"--inputs", "-i" },
                    "File inputs to merge"
                ),
                new Option<string>(
                    new []{"--output", "-o" },
                    "File path to output to"
                ),
            };

            cmd.Handler = CommandHandler.Create<string[], string, IConsole>(MergeStories);

            return await cmd.InvokeAsync(args);
        }

        private static void MergeStories(string[] inputs, string output, IConsole console) {
            var result = StoryMerger.Merge(inputs, output);
            console.Out.WriteLine($"Merge was {(result.IsSuccessful ? "successful" : "not successful")}");
            console.Out.WriteLine(result.Message);
        }
    }
}
