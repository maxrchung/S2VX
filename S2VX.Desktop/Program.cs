using osu.Framework;
using S2VX.Game;
using System;

// Yeah, so I'm not sure if we need to be compliant. I don't think it matters
// unless we're aiming to target multiple .NET languages?
[assembly: CLSCompliant(false)]
namespace S2VX.Desktop {
    public static class Program {
        public static void Main() {
            using var host = Host.GetSuitableHost(@"S2VX");
            using var game = new S2VXGame();
            host.Run(game);
        }
    }
}
