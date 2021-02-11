using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using S2VX.Game.Play;
using System.IO;

namespace S2VX.Game.Tests.VisualTests {
    public class PlayScreenTests : S2VXTestScene {
        [Cached]
        private ScreenStack ScreenStack { get; set; } = new ScreenStack();

        [BackgroundDependencyLoader]
        private void Load() =>
            AddStep("Add screen stack", () => Add(ScreenStack));

        private static string StoryDirectory { get; } = Path.Combine("VisualTests", "LoadStoryTests");
        private static string AudioPath { get; } = Path.Combine(StoryDirectory, "1-second-of-silence.mp3");

        private void TestLoadStory(string storyFileName) {
            var storyPath = Path.Combine(StoryDirectory, storyFileName);
            AddStep("Add play screen", () =>
                ScreenStack.Push(new PlayScreen(false, storyPath, AudioPath))
            );
        }

        [Test]
        public void LoadStory_ValidStory_HasNoUnhandledException() =>
            TestLoadStory("ValidStory.s2ry");

        [Test]
        public void LoadStory_EmptyFile_HasNoUnhandledException() =>
            TestLoadStory("EmptyFile.s2ry");

        [Test]
        public void LoadStory_MalformedJson_HasNoUnhandledException() =>
            TestLoadStory("MalformedJSON.s2ry");

        [Test]
        public void LoadStory_MissingHoldNotesProperty_HasNoUnhandledException() =>
            TestLoadStory("MissingHoldNotesProperty.s2ry");

        [Test]
        public void LoadStory_MissingEditorSettings_HasNoUnhandledException() =>
            TestLoadStory("MissingEditorSettings.s2ry");
    }
}
