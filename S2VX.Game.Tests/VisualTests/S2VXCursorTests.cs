using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using osuTK;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;

namespace S2VX.Game.Tests.VisualTests {
    public class S2VXCursorTests : S2VXTestScene {
        [Cached]
        private S2VXStory Story { get; set; } = new S2VXStory();
        [Cached]
        private S2VXCursor Cursor { get; set; } = new S2VXCursor();

        [BackgroundDependencyLoader]
        private void Load() => Add(Story);

        // All tests will have a note that starts to appear in 1 second
        [SetUpSteps]
        public void SetUpSteps() => AddStep("Reset story", () => Story.Reset());

        [Test]
        public void CreateCursor_Defaults_IsDefaultColor() =>
            AddAssert("Is default color", () =>
                Cursor.ActiveCursor.Colour == S2VXColorConstants.Yellow);

        [Test]
        public void CreateCursor_Defaults_HasDefaultSize() =>
            AddAssert("Has default size", () =>
                Cursor.ActiveCursor.Size == new Vector2(20));

        [Test]
        public void CreateCursor_Defaults_HasNoRotation() =>
            AddAssert("Has no rotation", () =>
                Cursor.ActiveCursor.Rotation == 0);

        [Test]
        public void SetSize_CameraScaleCommand_UpdatesCursorSize() {
            AddStep("Add scale command", () => Story.AddCommand(new CameraScaleCommand {
                StartValue = new Vector2(0.5f),
                EndValue = new Vector2(0.5f)
            }));
            AddAssert("Updates cursor size", () => Cursor.Scale == Story.Camera.Scale / 2);
        }

        [Test]
        public void SetRotation_CameraRotateCommand_HasCameraRotation() {
            AddStep("Add rotate command", () => Story.AddCommand(new CameraRotateCommand {
                StartValue = 0.5f,
                EndValue = 0.5f
            }));
            AddAssert("Has camera rotation", () => Cursor.Rotation == Story.Camera.Rotation);
        }
    }
}
