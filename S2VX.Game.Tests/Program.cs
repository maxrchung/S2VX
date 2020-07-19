using osu.Framework;
using osu.Framework.Platform;

namespace S2VX.Game.Tests
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableHost("visual-tests"))
            using (var game = new S2VXTestBrowser())
            {
                host.Run(game);
            }
        }
    }
}
