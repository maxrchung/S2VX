using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using S2VX.Game.SongSelection;

namespace S2VX.Game.Tests.VisualTests.S2VXCursorTests {
    public class CursorReset : S2VXTestScene {
        [Cached]
        private ScreenStack ScreenStack { get; set; } = new();

        private SongSelectionScreen SongSelectionScreen { get; set; }

        [Cached]
        private S2VXCursor Cursor { get; set; } = new S2VXCursor();

        [BackgroundDependencyLoader]
        private void Load() {
            ScreenStack.Push(SongSelectionScreen = new());
            Add(ScreenStack);
            Add(Cursor);
        }

        [Test]
        public void Reset_SongSelectionEntering_ResetsCursorProperties() {
            AddStep("Update cursor rotation", () => Cursor.ActiveCursor.Rotation = 1);

            // Instead of calling OnEntering manually, I tried doing
            // ScreenStack.Push(new SongSelectionScreen). But this was
            // unreliable in whether it would actually call
            // SongSelectionScreen's OnEntering or not.
            AddStep("Enter song selection screen", () => SongSelectionScreen.OnEntering(null));

            AddAssert("Resets cursor properties", () => Cursor.ActiveCursor.Rotation == 0);
        }
    }
}
