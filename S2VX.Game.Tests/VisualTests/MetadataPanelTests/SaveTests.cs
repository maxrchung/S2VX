using NUnit.Framework;
using osu.Framework.Testing;
using S2VX.Game.Editor.UserInterface;
using S2VX.Game.Story.Settings;
using System.IO;

namespace S2VX.Game.Tests.VisualTests {
    public class SaveTests : S2VXTestScene {
        private static string TestDirectory { get; } = Path.Combine("VisualTests", "MetadataPanelTests");
        private MetadataPanel Panel { get; set; }

        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Clear", () => Clear());
            AddStep("Add metadata panel", () => Add(Panel = new MetadataPanel(TestDirectory)));
        }

        public static MetadataSettings ReadMetadata() => MetadataSettings.Load(TestDirectory);

        [Test]
        public void Save_TitleChange_UpdatesTitle() {
            AddStep("Update panel", () => Panel.TxtTitle.Text = "CHANGED");
            AddStep("Save metadata", () => Panel.BtnSave.Click());
            AddAssert("Updates title", () => ReadMetadata().SongTitle == "CHANGED");
        }

        [Test]
        public void Save_ArtistChange_UpdatesArtist() {
            AddStep("Update panel", () => Panel.TxtArtist.Text = "CHANGED");
            AddStep("Save metadata", () => Panel.BtnSave.Click());
            AddAssert("Updates title", () => ReadMetadata().SongArtist == "CHANGED");
        }

        [Test]
        public void Save_AuthorChange_UpdatesAuthor() {
            AddStep("Update panel", () => Panel.TxtAuthor.Text = "CHANGED");
            AddStep("Save metadata", () => Panel.BtnSave.Click());
            AddAssert("Updates title", () => ReadMetadata().StoryAuthor == "CHANGED");
        }

        [Test]
        public void Save_DescriptionChange_UpdatesDescription() {
            AddStep("Update panel", () => Panel.TxtDescription.Text = "CHANGED");
            AddStep("Save metadata", () => Panel.BtnSave.Click());
            AddAssert("Updates title", () => ReadMetadata().MiscDescription == "CHANGED");
        }
    }
}
