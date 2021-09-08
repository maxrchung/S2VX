using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK.Input;
using S2VX.Game.Editor;
using S2VX.Game.Editor.ToolState;
using S2VX.Game.Story;
using System.IO;
using System.Linq;

namespace S2VX.Game.Tests.VisualTests {
    public class HoldNoteToolStateTests : S2VXTestScene {
        private EditorScreen Editor { get; set; }
        private S2VXStory Story { get; set; } = new();

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            var audioPath = Path.Combine("TestTracks", "1-minute-of-silence.mp3");
            Add(new ScreenStack(Editor = new(Story, S2VXTrack.Open(audioPath, audio))));
        }

        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Set tool state", () => Editor.SetToolState(new HoldNoteToolState()));
            AddStep("Pause editor", () => Editor.Play(false));
            AddStep("Restart editor", () => Editor.Restart());
            AddStep("Reset story", () => Story.Reset());
        }

        [Test]
        public void AddHoldNote_NewStory_HasNoNotes() => AddAssert("Has no notes", () => Story.Notes.Children.Count == 0);

        [Test]
        public void AddHoldNote_Escape_DoesNotAddNote() {
            AddStep("Move mouse to start", () => MoveMouseTo(Editor, new()));
            AddStep("Add start", () => InputManager.Click(MouseButton.Left));
            AddStep("Press escape", () => InputManager.PressKey(Key.Escape));
            AddStep("Release escape", () => InputManager.ReleaseKey(Key.Escape));
            AddStep("Seek to end", () => Editor.Seek(2000));
            AddStep("Move mouse to end", () => MoveMouseTo(Editor, new(200, 200)));
            AddStep("Try and add end", () => InputManager.Click(MouseButton.Right));
            AddAssert("Does not add note", () => Story.Notes.Children.Count == 0);
        }

        [Test]
        public void AddHoldNote_SameHitAndEndTime_DoesNotAddNote() {
            AddStep("Move mouse to start", () => MoveMouseTo(Editor, new()));
            AddStep("Add start", () => InputManager.Click(MouseButton.Left));
            AddStep("Move mouse to end", () => MoveMouseTo(Editor, new(200, 200)));
            AddStep("Try and add end", () => InputManager.Click(MouseButton.Right));
            AddAssert("Does not add note", () => Story.Notes.Children.Count == 0);
        }

        // Adds a hold note that goes from (0,0) at 0 milliseconds to (200,200) at 2000 milliseconds
        private void AddStraightHoldNote() {
            AddStep("Move mouse to start", () => MoveMouseTo(Editor, new()));
            AddStep("Add start", () => InputManager.Click(MouseButton.Left));
            AddStep("Seek to end", () => Editor.Seek(2000));
            AddStep("Move mouse to end", () => MoveMouseTo(Editor, new(200, 200)));
            AddStep("Add end", () => InputManager.Click(MouseButton.Right));
        }

        private void Undo() =>
            AddStep("Undo", () => {
                InputManager.PressKey(Key.ControlLeft);
                InputManager.Key(Key.Z);
                InputManager.ReleaseKey(Key.ControlLeft);
            });

        private void Redo() =>
            AddStep("Redo", () => {
                InputManager.PressKey(Key.ControlLeft);
                InputManager.PressKey(Key.ShiftLeft);
                InputManager.Key(Key.Z);
                InputManager.ReleaseKey(Key.ControlLeft);
                InputManager.ReleaseKey(Key.ShiftLeft);
            });

        [Test]
        public void AddHoldNote_StraightHoldNote_AddsNote() {
            AddStraightHoldNote();
            AddAssert("Adds note", () => Story.Notes.Children.Count == 1);
        }

        [Test]
        public void AddHoldNote_StraightHoldNote_DoesNotHaveMidCoordinates() {
            AddStraightHoldNote();
            AddAssert("Does not have mid coordinates", () => Story.Notes.GetHoldNotes().First().MidCoordinates.Count == 0);
        }

        [Test]
        public void AddHoldNote_StraightHoldNoteUndo_RemovesAddNote() {
            AddStraightHoldNote();
            Undo();
            AddAssert("Removes add note", () => Story.Notes.Children.Count == 0);
        }

        [Test]
        public void AddHoldNote_StraightHoldNoteRedo_AddsNote() {
            AddStraightHoldNote();
            Undo();
            Redo();
            AddAssert("Adds note", () => Story.Notes.Children.Count == 1);
        }

        // Adds a hold note that goes from (0,0) at 0 milliseconds to (200, 0) at 1000 milliseconds to (200,200) at 2000 milliseconds
        private void AddMultiPointHoldNote() {
            AddStep("Move mouse to start", () => MoveMouseTo(Editor, new()));
            AddStep("Add start", () => InputManager.Click(MouseButton.Left));
            AddStep("Seek to end", () => Editor.Seek(1000));
            AddStep("Move mouse to mid", () => MoveMouseTo(Editor, new(200, 0)));
            AddStep("Add mid", () => InputManager.Click(MouseButton.Left));
            AddStep("Seek to end", () => Editor.Seek(2000));
            AddStep("Move mouse to end", () => MoveMouseTo(Editor, new(200, 200)));
            AddStep("Add end", () => InputManager.Click(MouseButton.Right));
        }

        [Test]
        public void AddHoldNote_MultiPointHoldNote_AddsNote() {
            AddMultiPointHoldNote();
            AddAssert("Adds note", () => Story.Notes.Children.Count == 1);
        }

        [Test]
        public void AddHoldNote_MultiPointHoldNote_HasMidCoordinates() {
            AddMultiPointHoldNote();
            AddAssert("Has mid coordinates", () => Story.Notes.GetHoldNotes().First().MidCoordinates.Count == 1);
        }

        [Test]
        public void AddHoldNote_MultiPointHoldNoteUndo_RemovesNote() {
            AddMultiPointHoldNote();
            Undo();
            AddAssert("Removes note", () => Story.Notes.Children.Count == 0);
        }

        [Test]
        public void AddHoldNote_MultiPointHoldNoteRedo_AddsNote() {
            AddMultiPointHoldNote();
            Undo();
            Redo();
            AddAssert("Adds note", () => Story.Notes.Children.Count == 1);
        }
    }
}
