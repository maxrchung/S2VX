using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using osu.Framework.Testing;
using S2VX.Game.SongSelection;
using System.IO;

namespace S2VX.Game.Tests.HeadlessTests.SongSelectionScreenTests {
    [HeadlessTest]
    public class ImportTests : S2VXTestScene {
        [Cached]
        private S2VXGameBase GameBase { get; } = new();
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

        // There is no cleanup due to thumbnail.jpg being perpetually in-use by the update thread which will never run
        [Test]
        public void Import_ValidMP3_HasValidStory() {
            AddStep("Import valid MP3", () => SongSelectionScreen.Import(Path.Combine(AudioDirectory, AudioFileName)));
            AddAssert("Required story files exist", () =>
                Directory.Exists(NewStoryDirectory) &&
                File.Exists(Path.Combine(NewStoryDirectory, "audio.mp3")) &&
                File.Exists(Path.Combine(NewStoryDirectory, "leaderboard.json")) &&
                File.Exists(Path.Combine(NewStoryDirectory, "metadata.json")) &&
                File.Exists(Path.Combine(NewStoryDirectory, "story.s2ry")) &&
                File.Exists(Path.Combine(NewStoryDirectory, "thumbnail.jpg")));
        }
    }
}
