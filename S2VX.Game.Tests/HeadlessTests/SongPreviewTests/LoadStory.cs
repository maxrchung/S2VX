using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using osu.Framework.Testing;
using S2VX.Game.SongSelection.Containers;
using System.IO;

namespace S2VX.Game.Tests.HeadlessTests.SongPreviewTests {
    [HeadlessTest]
    public class LoadStory : S2VXTestScene {
        [Cached]
        private ScreenStack Screens { get; set; } = new();

        private SongPreview SongPreview { get; set; }
        private static string StoryDirectory { get; } = Path.Combine("VisualTests", "SongPreviewTests");
        private static string AudioFileName { get; } = "1-second-of-silence.mp3";

        [BackgroundDependencyLoader]
        private void Load() =>
            Add(Screens);

        private void AddSongPreview(string storyFileName) =>
            AddStep(
                "Add song preview",
                () => Add(SongPreview = new(StoryDirectory, storyFileName, AudioFileName))
            );

        private void ClickEditButton() =>
            AddStep("Click edit button", () => SongPreview.BtnEdit.Click());

        private void ClickPlayButton() =>
            AddStep("Click play button", () => SongPreview.BtnPlay.Click());

        private void AssertEditHasRedBorder() =>
            AddAssert("Has red border", () => SongPreview.BtnEdit.BorderThickness == 5);

        private void AssertEditHasNoBorder() =>
            AddAssert("Has no border", () => SongPreview.BtnEdit.BorderThickness == 0);

        private void AssertPlayHasRedBorder() =>
            AddAssert("Has red border", () => SongPreview.BtnPlay.BorderThickness == 5);

        private void AssertPlayHasNoBorder() =>
            AddAssert("Has no border", () => SongPreview.BtnPlay.BorderThickness == 0);

        [Test]
        public void BtnEdit_EmptyFile_HasRedBorder() {
            AddSongPreview("EmptyFile.s2ry");
            ClickEditButton();
            AssertEditHasRedBorder();
        }

        [Test]
        public void BtnEdit_MalformedJSON_HasRedBorder() {
            AddSongPreview("MalformedJSON.s2ry");
            ClickEditButton();
            AssertEditHasRedBorder();
        }

        [Test]
        public void BtnEdit_MissingEditorSettings_HasRedBorder() {
            AddSongPreview("MissingEditorSettings.s2ry");
            ClickEditButton();
            AssertEditHasRedBorder();
        }

        [Test]
        public void BtnEdit_MissingHoldNotes_HasRedBorder() {
            AddSongPreview("MissingHoldNotes.s2ry");
            ClickEditButton();
            AssertEditHasRedBorder();
        }

        [Test]
        public void BtnEdit_ValidStory_HasNoBorder() {
            AddSongPreview("ValidStory.s2ry");
            ClickEditButton();
            AssertEditHasNoBorder();
        }

        [Test]
        public void BtnPlay_EmptyFile_HasRedBorder() {
            AddSongPreview("EmptyFile.s2ry");
            ClickPlayButton();
            AssertPlayHasRedBorder();
        }

        [Test]
        public void BtnPlay_MalformedJSON_HasRedBorder() {
            AddSongPreview("MalformedJSON.s2ry");
            ClickPlayButton();
            AssertPlayHasRedBorder();
        }

        [Test]
        public void BtnPlay_MissingEditorSettings_HasRedBorder() {
            AddSongPreview("MissingEditorSettings.s2ry");
            ClickPlayButton();
            AssertPlayHasRedBorder();
        }

        [Test]
        public void BtnPlay_MissingHoldNotes_HasRedBorder() {
            AddSongPreview("MissingHoldNotes.s2ry");
            ClickPlayButton();
            AssertPlayHasRedBorder();
        }

        [Test]
        public void BtnPlay_ValidStory_HasNoBorder() {
            AddSongPreview("ValidStory.s2ry");
            ClickPlayButton();
            AssertPlayHasNoBorder();
        }
    }
}
