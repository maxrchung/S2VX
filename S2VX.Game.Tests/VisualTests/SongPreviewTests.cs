using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using S2VX.Game.SongSelection.Containers;
using System.IO;

namespace S2VX.Game.Tests.VisualTests {
    public class SongPreviewTests : S2VXTestScene {
        [Cached]
        private ScreenStack Screens { get; set; } = new ScreenStack();

        private SongPreview SongPreview { get; set; }
        private static string StoryDirectory { get; } = Path.Combine("VisualTests", "SongPreviewTests");
        private static string AudioFileName { get; } = "1-second-of-silence.mp3";

        [BackgroundDependencyLoader]
        private void Load() =>
            Add(Screens);

        private void AddSongPreview(string storyFileName) =>
            AddStep(
                "Add song preview",
                () => Add(SongPreview = new SongPreview(StoryDirectory, storyFileName, AudioFileName))
            );

        [Test]
        public void BtnEdit_EmptyFile_HasRedBorder() {
            AddSongPreview("EmptyFile.s2ry");
            AddStep("Click edit button", () => SongPreview.BtnEdit.Click());
            AddAssert("Has red border", () => SongPreview.BtnEdit.BorderThickness == 5);
        }

        [Test]
        public void BtnEdit_MalformedJSON_HasRedBorder() {
            AddSongPreview("MalformedJSON.s2ry");
            AddStep("Click edit button", () => SongPreview.BtnEdit.Click());
            AddAssert("Has red border", () => SongPreview.BtnEdit.BorderThickness == 5);
        }

        [Test]
        public void BtnEdit_MissingEditorSettings_HasRedBorder() {
            AddSongPreview("MissingEditorSettings.s2ry");
            AddStep("Click edit button", () => SongPreview.BtnEdit.Click());
            AddAssert("Has red border", () => SongPreview.BtnEdit.BorderThickness == 5);
        }

        [Test]
        public void BtnEdit_MissingHoldNotes_HasRedBorder() {
            AddSongPreview("MissingHoldNotes.s2ry");
            AddStep("Click edit button", () => SongPreview.BtnEdit.Click());
            AddAssert("Has red border", () => SongPreview.BtnEdit.BorderThickness == 5);
        }

        [Test]
        public void BtnEdit_ValidStory_HasNoBorder() {
            AddSongPreview("ValidStory.s2ry");
            AddStep("Click edit button", () => SongPreview.BtnEdit.Click());
            AddAssert("Has no border", () => SongPreview.BtnEdit.BorderThickness == 0);
        }

        [Test]
        public void BtnPlay_EmptyFile_HasRedBorder() {
            AddSongPreview("EmptyFile.s2ry");
            AddStep("Click play button", () => SongPreview.BtnPlay.Click());
            AddAssert("Has red border", () => SongPreview.BtnPlay.BorderThickness == 5);
        }

        [Test]
        public void BtnPlay_MalformedJSON_HasRedBorder() {
            AddSongPreview("MalformedJSON.s2ry");
            AddStep("Click play button", () => SongPreview.BtnPlay.Click());
            AddAssert("Has red border", () => SongPreview.BtnPlay.BorderThickness == 5);
        }

        [Test]
        public void BtnPlay_MissingEditorSettings_HasRedBorder() {
            AddSongPreview("MissingEditorSettings.s2ry");
            AddStep("Click play button", () => SongPreview.BtnPlay.Click());
            AddAssert("Has red border", () => SongPreview.BtnPlay.BorderThickness == 5);
        }

        [Test]
        public void BtnPlay_MissingHoldNotes_HasRedBorder() {
            AddSongPreview("MissingHoldNotes.s2ry");
            AddStep("Click play button", () => SongPreview.BtnPlay.Click());
            AddAssert("Has red border", () => SongPreview.BtnPlay.BorderThickness == 5);
        }

        [Test]
        public void BtnPlay_ValidStory_HasNoBorder() {
            AddSongPreview("ValidStory.s2ry");
            AddStep("Click play button", () => SongPreview.BtnPlay.Click());
            AddAssert("Has no border", () => SongPreview.BtnPlay.BorderThickness == 0);
        }
    }
}
