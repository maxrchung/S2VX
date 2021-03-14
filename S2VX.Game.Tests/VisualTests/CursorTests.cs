using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using osuTK;
using S2VX.Game.SongSelection;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;

namespace S2VX.Game.Tests.VisualTests {
    public class CursorTests : S2VXTestScene {
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
        public void Load_Defaults_IsDefaultColor() =>
            AddAssert("Is default color", () => Cursor.Color == S2VXColorConstants.Yellow);

        [Test]
        public void Load_Defaults_HasDefaultScale() =>
            AddAssert("Has default scale", () => Cursor.Scale == Story.Camera.Scale / 2);

        [Test]
        public void Load_Defaults_HasNoRotation() =>
            AddAssert("Has no rotation", () => Cursor.Rotation == 0);

        [Test]
        public void StoryUpdate_CameraScaleCommand_HasHalfCameraScale() {
            AddStep("Add scale command", () => Story.AddCommand(new CameraScaleCommand {
                StartValue = new Vector2(0.5f),
                EndValue = new Vector2(0.5f)
            }));
            AddAssert("Has half camera scale", () => Cursor.Scale == Story.Camera.Scale / 2);
        }

        [Test]
        public void StoryUpdate_CameraRotateCommand_HasCameraRotation() {
            AddStep("Add rotate command", () => Story.AddCommand(new CameraRotateCommand {
                StartValue = 0.5f,
                EndValue = 0.5f
            }));
            AddAssert("Has camera rotation", () => Cursor.Rotation = Story.Camera.Rotation);
        }

        [Test]
        public void Reset_SongSelectionEntering_ResetsCursorProperties() {
            AddStep("Update cursor rotation", () => Cursor.Rotation = 0.5f);
            AddStep("Enter song selection screen", () => new SongSelectionScreen().OnEntering(new SongSelectionScreen()));
            AddAssert("Resets cursor properties", () => Cursor.Rotation == 0);
        }
    }
}
