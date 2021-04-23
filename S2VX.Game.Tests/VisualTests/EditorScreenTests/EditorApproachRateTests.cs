using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osu.Framework.Utils;
using S2VX.Game.Editor;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;
using S2VX.Game.Story.Note;
using System.IO;

namespace S2VX.Game.Tests.VisualTests.EditorScreenTests {
    public class EditorApproachRateTests : S2VXTestScene {
        private EditorScreen Editor { get; set; }
        private S2VXTrack Track { get; set; }
        private S2VXStory Story { get; set; }
        private EditorNote NoteToTest { get; set; }
        private EditorHoldNote HoldNoteToTest { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            var storyPath = Path.Combine("VisualTests", "EditorScreenTests", "ValidStory.s2ry");
            Story = new S2VXStory();
            Story.Open(storyPath, true);

            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            Track = S2VXTrack.Open(audioPath, audio);

            Add(new ScreenStack(Editor = new EditorScreen(Story, Track)));
        }

        [SetUpSteps]

        public void SetUpSteps() {
            AddStep("Pause editor", () => Editor.Play(false));
            AddStep("Restart editor", () => Editor.Restart());
            AddStep("Reset story", () => Story.Reset());
            AddStep("Add showtime command at 1000", () => Story.AddCommand(new NotesShowTimeCommand {
                StartValue = 1000,
                EndValue = 1000,
            }));
            AddStep("Add note", () => Story.AddNote(NoteToTest = new EditorNote {
                HitTime = 10000,
            }));
            AddStep("Add hold note", () => Story.AddNote(HoldNoteToTest = new EditorHoldNote {
                HitTime = 10000,
                EndTime = 10001,
            }));
        }

        [Test]
        public void EditorApproachRate_ApproachRateOne_NoteIsNotVisible() {
            AddStep("Add showtime command at 1000", () => Story.AddCommand(new NotesShowTimeCommand {
                StartValue = 1000,
                EndValue = 1000,
            }));
            AddAssert("Note is not visible", () => NoteToTest.Alpha == 1);
        }

        [Test]
        public void EditorApproachRate_ApproachRateTwo_NoteIsVisible() {

        }

        [Test]
        public void EditorApproachRate_ApproachRateOne_HoldNoteIsNotVisible() {
            AddStep("Add showtime command at 1000", () => Story.AddCommand(new NotesShowTimeCommand {
                StartValue = 1000,
                EndValue = 1000,
            }));
            AddAssert("Note is not visible", () => NoteToTest.Alpha == 1);
        }

        [Test]
        public void EditorApproachRate_ApproachRateTwo_HoldNoteIsVisible() {

        }
    }
}
