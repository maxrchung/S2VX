using osu.Framework;
using osu.Framework.Platform;
using S2VX.Game;

namespace S2VX.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableHost(@"S2VX"))
            using (osu.Framework.Game game = new S2VXGame())
                host.Run(game);
        }
    }
}
