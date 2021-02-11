using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Threading.Tasks;

[assembly: CLSCompliant(false)]
namespace StoryMerge {
    internal class Program {
        private static async Task<int> Main(string[] args) {
            var root = new RootCommand {
                new Option<string[]>(
                    new []{"--inputs", "-i" },
                    "File inputs to merge"
                ),
                new Option<string>(
                    new []{"--output", "-o" },
                    "File path to output to"
                ),
            };

            root.Handler = CommandHandler.Create(
                (string[] inputs, string output, IConsole console) => {
                    var result = StoryMerger.Merge(inputs, output);
                    console.Out.WriteLine();
                    console.Out.WriteLine($"Merge was {(result.IsSuccessful ? "successful" : "not successful")}");
                    console.Out.WriteLine(result.Message);
                    console.Out.WriteLine();

                    // Invoking help manually: https://github.com/dotnet/command-line-api/issues/1087#issuecomment-730634029
                    if (!result.IsSuccessful) {
                        root.InvokeAsync("--help");
                    }

                });
            return await root.InvokeAsync(args);
        }
    }
}
