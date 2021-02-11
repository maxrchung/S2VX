using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using S2VX.Game.Editor;
using System.IO;

namespace S2VX.Game.Tests.VisualTests {
    public class EditorScreenTests : S2VXTestScene {
        [Cached]
        private ScreenStack ScreenStack { get; set; } = new ScreenStack();

        [BackgroundDependencyLoader]
        private void Load() =>
            Add(ScreenStack);

        private static string StoryDirectory { get; } = Path.Combine("VisualTests", "LoadStoryTests");
        private static string AudioPath { get; } = Path.Combine(StoryDirectory, "1-second-of-silence.mp3");

        private void TestLoadStory(string storyFileName) {
            var storyPath = Path.Combine(StoryDirectory, storyFileName);
            AddStep("Add editor screen", () => ScreenStack.Push(new EditorScreen(storyPath, AudioPath)));
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
