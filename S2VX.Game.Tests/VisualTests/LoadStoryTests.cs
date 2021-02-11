using NUnit.Framework;
using S2VX.Game.Editor;
using S2VX.Game.Play;
using System.IO;

namespace S2VX.Game.Tests.VisualTests {
    public class LoadStoryTests : S2VXTestScene {
        private static string StoryDirectory { get; } = Path.Combine("VisualTests", "LoadTrackTests");
        private static string AudioPath { get; } = Path.Combine(StoryDirectory, "1-second-of-silence.mp3");

        public void AddEditorScreen(string storyFileName) {
            var storyPath = Path.Combine(StoryDirectory, storyFileName);
            AddStep("Add editor screen", () => Add(new EditorScreen(storyPath, AudioPath)));
        }

        public void AddPlayScreen(string storyFileName) {
            var storyPath = Path.Combine(StoryDirectory, storyFileName);
            AddStep("Add editor screen", () => Add(new PlayScreen(false, storyPath, AudioPath)));
        }

        [Test]
        public void LoadEditor_ValidStory_HasNoUnhandledException() =>
            AddEditorScreen("ValidStory.s2ry");

        [Test]
        public void LoadEditor_EmptyFile_HasNoUnhandledException() =>
            AddEditorScreen("EmptyFile.s2ry");

        [Test]
        public void LoadEditor_MalformedJson_HasNoUnhandledException() =>
            AddEditorScreen("MalformedJSON.s2ry");

        [Test]
        public void LoadEditor_MissingHoldNotesProperty_HasNoUnhandledException() =>
            AddEditorScreen("MissingHoldNotesProperty.s2ry");

        [Test]
        public void LoadEditor_MissingEditorSettings_HasNoUnhandledException() =>
            AddEditorScreen("MissingEditorSettings.s2ry");

        [Test]
        public void LoadPlay_ValidStory_HasNoUnhandledException() =>
            AddPlayScreen("ValidStory.s2ry");

        [Test]
        public void LoadPlay_EmptyFile_HasNoUnhandledException() =>
            AddPlayScreen("EmptyFile.s2ry");

        [Test]
        public void LoadPlay_MalformedJson_HasNoUnhandledException() =>
            AddPlayScreen("MalformedJSON.s2ry");

        [Test]
        public void LoadPlay_MissingHoldNotesProperty_HasNoUnhandledException() =>
            AddPlayScreen("MissingHoldNotesProperty.s2ry");

        [Test]
        public void LoadPlay_MissingEditorSettings_HasNoUnhandledException() =>
            AddPlayScreen("MissingEditorSettings.s2ry");
    }
}
