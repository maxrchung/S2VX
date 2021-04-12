using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using osuTK;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;

namespace S2VX.Game.Tests.VisualTests.S2VXCursorTests {
    public class CursorUpdateTests : S2VXTestScene {
        [Cached]
        private S2VXStory Story { get; set; } = new();

        [Cached]
        private S2VXCursor Cursor { get; set; } = new();

        [BackgroundDependencyLoader]
        private void Load() {
            Add(Story);
            Add(Cursor);
        }

        // All tests will have a note that starts to appear in 1 second
        [SetUpSteps]
        public void SetUpSteps() => AddStep("Reset story", () => Story.Reset());

        [Test]
        public void UpdateSize_CameraScaleCommand_UpdatesCursorSize() {
            AddStep("Add scale command", () => Story.AddCommand(new CameraScaleCommand {
                StartValue = new Vector2(0.5f),
                EndValue = new Vector2(0.5f)
            }));
            AddAssert("Updates cursor size", () =>
                Cursor.ActiveCursor.Size == new Vector2(0.5f * S2VXGameBase.GameWidth / 4)
            );
        }

        [Test]
        public void UpdateRotation_CameraRotateCommand_HasCameraRotation() {
            AddStep("Add rotate command", () => Story.AddCommand(new CameraRotateCommand {
                StartValue = 1,
                EndValue = 1
            }));
            AddAssert("Has camera rotation", () => Cursor.ActiveCursor.Rotation == 1);
        }
    }
}
