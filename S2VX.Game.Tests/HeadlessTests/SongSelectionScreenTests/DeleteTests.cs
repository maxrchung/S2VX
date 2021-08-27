using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using osu.Framework.Testing;
using S2VX.Game.SongSelection;
using System.IO;

namespace S2VX.Game.Tests.HeadlessTests.SongSelectionScreenTests {
    [HeadlessTest]
    public class DeleteTests : S2VXTestScene {
        [Cached]
        private ScreenStack Screens { get; } = new();

        private SongSelectionScreen SongSelectionScreen { get; } = new();
        private static string StoryDirectory { get; } = "Stories";
        private static string AudioDirectory { get; } = Path.Combine("HeadlessTests", "SongSelectionScreenTests");
        private static string AudioFileName { get; } = "1-second-of-silence.mp3";
        private static string NewStoryDirectory { get; } = Path.Combine(StoryDirectory, Path.GetFileNameWithoutExtension(AudioFileName));

        [BackgroundDependencyLoader]
        private void Load() {
            Screens.Push(SongSelectionScreen);
            Add(Screens);
        }

        [Test]
        public void Delete_Story_DeletesStory() {
            AddStep("Import valid MP3", () => SongSelectionScreen.Import(Path.Combine(AudioDirectory, AudioFileName)));
            AddStep("Delete the imported story", () => SongSelectionScreen.DeleteSelectionItem(Path.GetFileNameWithoutExtension(AudioFileName)));
            AddAssert("Directory with same name is deleted", () =>
                !Directory.Exists(NewStoryDirectory));
        }
    }
}
