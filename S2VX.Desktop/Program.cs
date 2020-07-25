using System;
using osu.Framework;
using osu.Framework.Platform;
using S2VX.Game;

namespace S2VX.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (var host = Host.GetSuitableHost(@"S2VX"))
            using (var game = new S2VXGame())
            {
                try
                {
                    host.Run(game);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
