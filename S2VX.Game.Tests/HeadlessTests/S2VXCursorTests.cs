using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using S2VX.Game.SongSelection;

namespace S2VX.Game.Tests.HeadlessTests {
    [HeadlessTest]
    public class S2VXCursorTests : S2VXTestScene {
        [Cached]
        private S2VXCursor Cursor { get; set; } = new S2VXCursor();

        [BackgroundDependencyLoader]
        private void Load() => Add(Cursor);

        [Test]
        public void Reset_SongSelectionEntering_ResetsCursorProperties() {
            AddStep("Update cursor rotation", () => Cursor.Rotation = 0.5f);
            AddStep("Enter song selection screen", () => new SongSelectionScreen().OnEntering(new SongSelectionScreen()));
            AddAssert("Resets cursor properties", () => Cursor.Rotation == 0);
        }
    }
}
