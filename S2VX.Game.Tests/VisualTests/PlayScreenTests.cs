﻿using NUnit.Framework;
using osu.Framework.Screens;
using S2VX.Game.Play;
using System.IO;

namespace S2VX.Game.Tests.VisualTests {
    public class PlayScreenTests : S2VXTestScene {
        private static string StoryDirectory { get; } = Path.Combine("VisualTests", "LoadStoryTests");
        private static string AudioPath { get; } = Path.Combine(StoryDirectory, "1-second-of-silence.mp3");

        private void LoadStory(string storyFileName) {
            var storyPath = Path.Combine(StoryDirectory, storyFileName);
            AddStep("Add screen stack", () => Add(new ScreenStack(new PlayScreen(false, storyPath, AudioPath))));
        }

        [Test]
        public void LoadStory_ValidStory_HasNoUnhandledException() =>
            LoadStory("ValidStory.s2ry");

        [Test]
        public void LoadStory_EmptyFile_HasNoUnhandledException() =>
            LoadStory("EmptyFile.s2ry");

        [Test]
        public void LoadStory_MalformedJson_HasNoUnhandledException() =>
            LoadStory("MalformedJSON.s2ry");

        [Test]
        public void LoadStory_MissingHoldNotes_HasNoUnhandledException() =>
            LoadStory("MissingHoldNotes.s2ry");

        [Test]
        public void LoadStory_MissingEditorSettings_HasNoUnhandledException() =>
            LoadStory("MissingEditorSettings.s2ry");
    }
}
