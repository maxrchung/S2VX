using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using S2VX.Game.SongSelection;
using System.Diagnostics.CodeAnalysis;

namespace S2VX.Game.Tests.VisualTests.S2VXCursorTests {
    [SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "ScreenStack is an osu!framework type")]
    public class WithScreenStack : S2VXTestScene {
        [Cached]
        private ScreenStack ScreenStack { get; set; } = new ScreenStack();

        [Cached]
        private S2VXCursor Cursor { get; set; } = new S2VXCursor();

        [BackgroundDependencyLoader]
        private void Load() {
            Add(ScreenStack);
            Add(Cursor);
        }

        [Test]
        public void Reset_SongSelectionEntering_ResetsCursorProperties() {
            AddStep("Update cursor rotation", () => Cursor.ActiveCursor.Rotation = 1);
            AddStep("Push song selection screen", () => ScreenStack.Push(new SongSelectionScreen()));
            AddAssert("Resets cursor properties", () => Cursor.ActiveCursor.Rotation == 0);
        }
    }
}
