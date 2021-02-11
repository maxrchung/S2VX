using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using S2VX.Game.Editor;
using S2VX.Game.Play;
using System.IO;

namespace S2VX.Game.Tests.VisualTests {
    public class LoadStoryTests : S2VXTestScene {
        [Cached]
        private ScreenStack ScreenStack { get; set; } = new ScreenStack();

        [BackgroundDependencyLoader]
        private void Load() =>
            Add(ScreenStack);

        private static string StoryDirectory { get; } = Path.Combine("VisualTests", "LoadStoryTests");
        private static string AudioPath { get; } = Path.Combine(StoryDirectory, "1-second-of-silence.mp3");

        private void TestEditorScreen(string storyFileName) {
            var storyPath = Path.Combine(StoryDirectory, storyFileName);
            AddStep("Add editor screen", () => ScreenStack.Push(new EditorScreen(storyPath, AudioPath)));
            AddStep("Exit screen", () => ScreenStack.Exit());
        }

        private void TestPlayScreen(string storyFileName) {
            var storyPath = Path.Combine(StoryDirectory, storyFileName);
            AddStep("Add editor screen", () => ScreenStack.Push(new PlayScreen(false, storyPath, AudioPath)));
            AddStep("Exit screen", () => ScreenStack.Exit());
        }

        [Test]
        public void LoadEditor_ValidStory_HasNoUnhandledException() =>
            TestEditorScreen("ValidStory.s2ry");

        [Test]
        public void LoadEditor_EmptyFile_HasNoUnhandledException() =>
            TestEditorScreen("EmptyFile.s2ry");

        [Test]
        public void LoadEditor_MalformedJson_HasNoUnhandledException() =>
            TestEditorScreen("MalformedJSON.s2ry");

        [Test]
        public void LoadEditor_MissingHoldNotesProperty_HasNoUnhandledException() =>
            TestEditorScreen("MissingHoldNotesProperty.s2ry");

        [Test]
        public void LoadEditor_MissingEditorSettings_HasNoUnhandledException() =>
            TestEditorScreen("MissingEditorSettings.s2ry");

        [Test]
        public void LoadPlay_ValidStory_HasNoUnhandledException() =>
            TestPlayScreen("ValidStory.s2ry");

        [Test]
        public void LoadPlay_EmptyFile_HasNoUnhandledException() =>
            TestPlayScreen("EmptyFile.s2ry");

        [Test]
        public void LoadPlay_MalformedJson_HasNoUnhandledException() =>
            TestPlayScreen("MalformedJSON.s2ry");

        [Test]
        public void LoadPlay_MissingHoldNotesProperty_HasNoUnhandledException() =>
            TestPlayScreen("MissingHoldNotesProperty.s2ry");

        [Test]
        public void LoadPlay_MissingEditorSettings_HasNoUnhandledException() =>
            TestPlayScreen("MissingEditorSettings.s2ry");
    }
}
