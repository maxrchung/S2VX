using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK.Input;
using S2VX.Game.EndGame;
using S2VX.Game.Play;
using S2VX.Game.SongSelection;
using System.IO;

namespace S2VX.Game.Tests.VisualTests {
    public class SelectToolStateTests : S2VXTestScene {
        // With hold note, adjust start point - start point modified
        // Ctrl+z - start point removed
        // Ctrl+shift+z - start point readded
        // With hold note, adjust end point - end point modified
        // Ctrl+z - end point removed
        // Ctrl+shift+z - end point readded
        // With hold note with multiple mid coordinates, adjust mid coordinate - mid coordinate modified
        // Ctrl+z - mid point removed
        // Ctrl+shift+z - mid point readded
    }
}
