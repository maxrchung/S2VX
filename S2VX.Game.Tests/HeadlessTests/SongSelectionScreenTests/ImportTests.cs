using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using osu.Framework.Testing;
using S2VX.Game.SongSelection;
using System.IO;

namespace S2VX.Game.Tests.HeadlessTests.SongSelectionScreenTests {
    //[HeadlessTest]
    public class ImportTests : S2VXTestScene {
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

        [SetUpSteps]
        public void SetUpSteps() {
            // If tests are run back-to-back, the thumbnail may still be in-use by the previous SongSelectionScreen
            AddWaitStep("Wait a moment", 15);
            AddStep("Delete any existing files", () => Directory.Delete(StoryDirectory, true));
        }

        [Test]
        public void Import_ValidMP3_CreatesNewDirectory() {
            AddStep("Import valid MP3", () => SongSelectionScreen.Import(Path.Combine(AudioDirectory, AudioFileName)));
            AddAssert("Directory with same name is created", () =>
                Directory.Exists(NewStoryDirectory));
        }

        [Test]
        public void Import_ValidMP3_HasAudio() {
            AddStep("Import valid MP3", () => SongSelectionScreen.Import(Path.Combine(AudioDirectory, AudioFileName)));
            AddAssert("audio.mp3 exists in the new directory", () =>
                File.Exists(Path.Combine(NewStoryDirectory, "audio.mp3")));
        }

        [Test]
        public void Import_ValidMP3_HasLeaderboard() {
            AddStep("Import valid MP3", () => SongSelectionScreen.Import(Path.Combine(AudioDirectory, AudioFileName)));
            AddAssert("leaderboard.json exists in the new directory", () =>
                File.Exists(Path.Combine(NewStoryDirectory, "leaderboard.json")));
        }

        [Test]
        public void Import_ValidMP3_HasMetadata() {
            AddStep("Import valid MP3", () => SongSelectionScreen.Import(Path.Combine(AudioDirectory, AudioFileName)));
            AddAssert("metadata.json exists in the new directory", () =>
                File.Exists(Path.Combine(NewStoryDirectory, "metadata.json")));
        }

        [Test]
        public void Import_ValidMP3_HasStory() {
            AddStep("Import valid MP3", () => SongSelectionScreen.Import(Path.Combine(AudioDirectory, AudioFileName)));
            AddAssert("story.s2ry exists in the new directory", () =>
                File.Exists(Path.Combine(NewStoryDirectory, "story.s2ry")));
        }

        [Test]
        public void Import_ValidMP3_HasThumbnail() {
            AddStep("Import valid MP3", () => SongSelectionScreen.Import(Path.Combine(AudioDirectory, AudioFileName)));
            AddAssert("thumbnail.jpg exists in the new directory", () =>
                File.Exists(Path.Combine(NewStoryDirectory, "thumbnail.jpg")));
        }
    }
}
