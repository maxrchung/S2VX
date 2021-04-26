using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
//using osu.Framework.Utils;
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
            AddStep("Reload editor settings", () => Editor.LoadEditorSettings());
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
            AddStep("Decrease editor approach rate multiplier to 1", () => Editor.ApproachRateDecrease());
            AddStep("Seek right before note is visible", () => Editor.Seek(NoteToTest.HitTime - Story.Notes.ShowTime - Story.Notes.FadeInTime));
            AddAssert("Note is not visible", () => NoteToTest.Alpha == 0);
        }

        [Test]
        public void EditorApproachRate_ApproachRateTwo_NoteIsVisible() {
            AddStep("Seek right before note would be visible", () => Editor.Seek(NoteToTest.HitTime - Story.Notes.ShowTime - Story.Notes.FadeInTime));
            AddAssert("Note is visible", () => NoteToTest.Alpha != 0);
        }

        [Test]
        public void EditorApproachRate_ApproachRateTwo_NoteIsNotVisible() {
            AddStep("Seek right before note is visible", () => Editor.Seek(NoteToTest.HitTime - Story.Notes.ShowTime * 2 - Story.Notes.FadeInTime));
            AddAssert("Note is not visible", () => NoteToTest.Alpha == 0);
        }

        [Test]
        public void EditorApproachRate_ApproachRateFour_NoteIsVisible() {
            AddStep("Increase editor approach rate multiplier to 4", () => Editor.ApproachRateIncrease());
            AddStep("Seek right before note would be visible", () => Editor.Seek(NoteToTest.HitTime - Story.Notes.ShowTime * 2 - Story.Notes.FadeInTime));
            AddAssert("Note is visible", () => NoteToTest.Alpha != 0);
        }

        [Test]
        public void EditorApproachRate_ApproachRateFour_NoteIsNotVisible() {
            AddStep("Increase editor approach rate multiplier to 4", () => Editor.ApproachRateIncrease());
            AddStep("Seek right before note would be visible", () => Editor.Seek(NoteToTest.HitTime - Story.Notes.ShowTime * 4 - Story.Notes.FadeInTime));
            AddAssert("Note is visible", () => NoteToTest.Alpha == 0);
        }

        [Test]
        public void EditorApproachRate_ApproachRateEight_NoteIsVisible() {
            AddStep("Increase editor approach rate multiplier to 4", () => Editor.ApproachRateIncrease());
            AddStep("Increase editor approach rate multiplier to 8", () => Editor.ApproachRateIncrease());
            AddStep("Seek right before note would be visible", () => Editor.Seek(NoteToTest.HitTime - Story.Notes.ShowTime * 4 - Story.Notes.FadeInTime));
            AddAssert("Note is visible", () => NoteToTest.Alpha != 0);
        }

        [Test]
        public void EditorApproachRate_ApproachRateOne_HoldNoteIsNotVisible() {
            AddStep("Decrease editor approach rate multiplier to 1", () => Editor.ApproachRateDecrease());
            AddStep("Seek right before hold note is visible", () => Editor.Seek(HoldNoteToTest.HitTime - Story.Notes.ShowTime - Story.Notes.FadeInTime));
            AddAssert("Hold Note is not visible", () => HoldNoteToTest.Alpha == 0);
        }

        [Test]
        public void EditorApproachRate_ApproachRateTwo_HoldNoteIsVisible() {
            AddStep("Seek right before hold note would be visible", () => Editor.Seek(HoldNoteToTest.HitTime - Story.Notes.ShowTime - Story.Notes.FadeInTime));
            AddAssert("Hold Note is visible", () => HoldNoteToTest.Alpha != 0);
        }

        [Test]
        public void EditorApproachRate_ApproachRateTwo_HoldNoteIsNotVisible() {
            AddStep("Seek right before hold note is visible", () => Editor.Seek(NoteToTest.HitTime - Story.Notes.ShowTime * 2 - Story.Notes.FadeInTime));
            AddAssert("Hold Note is not visible", () => HoldNoteToTest.Alpha == 0);
        }

        [Test]
        public void EditorApproachRate_ApproachRateFour_HoldNoteIsVisible() {
            AddStep("Increase editor approach rate multiplier to 4", () => Editor.ApproachRateIncrease());
            AddStep("Seek right before hold note would be visible", () => Editor.Seek(NoteToTest.HitTime - Story.Notes.ShowTime * 2 - Story.Notes.FadeInTime));
            AddAssert("Hold Note is visible", () => HoldNoteToTest.Alpha != 0);
        }

        [Test]
        public void EditorApproachRate_ApproachRateFour_HoldNoteIsNotVisible() {
            AddStep("Increase editor approach rate multiplier to 4", () => Editor.ApproachRateIncrease());
            AddStep("Seek right before hold note would be visible", () => Editor.Seek(NoteToTest.HitTime - Story.Notes.ShowTime * 4 - Story.Notes.FadeInTime));
            AddAssert("Hold Note is visible", () => HoldNoteToTest.Alpha == 0);
        }

        [Test]
        public void EditorApproachRate_ApproachRateEight_HoldNoteIsVisible() {
            AddStep("Increase editor approach rate multiplier to 4", () => Editor.ApproachRateIncrease());
            AddStep("Increase editor approach rate multiplier to 8", () => Editor.ApproachRateIncrease());
            AddStep("Seek right before hold note would be visible", () => Editor.Seek(NoteToTest.HitTime - Story.Notes.ShowTime * 4 - Story.Notes.FadeInTime));
            AddAssert("Hold Note is visible", () => HoldNoteToTest.Alpha != 0);
        }
    }
}
