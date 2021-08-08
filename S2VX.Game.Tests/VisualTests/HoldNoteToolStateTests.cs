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
    public class HoldNoteToolStateTests : S2VXTestScene {

        // Control case - no hold notes
        // Left click - escape - no hold notes
        // Left click right click at same time - no hold note created
        // Left click right click - hold note created
        // Left click left click right click - hold note created with anchors
        // Left click left click right click ctrl z - hold note removed
        // Left click left click right click ctrl z+ctrl+shift+z - hold note readded

    }
}
