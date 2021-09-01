using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK;
using osuTK.Input;
using S2VX.Game.Editor;
using S2VX.Game.Editor.ToolState;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;
using System.IO;
using System.Linq;

namespace S2VX.Game.Tests.VisualTests {
    public class SelectToolStateTests : S2VXTestScene {
        private EditorScreen Editor { get; set; }
        private S2VXStory Story { get; set; } = new();
        private EditorHoldNote HoldNote { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            var audioPath = Path.Combine("TestTracks", "1-minute-of-silence.mp3");
            Add(new ScreenStack(Editor = new(Story, S2VXTrack.Open(audioPath, audio))));
        }

        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Pause editor", () => Editor.Play(false));
            AddStep("Restart editor", () => Editor.Restart());
            AddStep("Reset story", () => Story.Reset());
            AddStep("Set tool state", () => Editor.SetToolState(new HoldNoteToolState()));
            AddMultiPointHoldNote();
            AddStep("Set editor hold note", () => HoldNote = (EditorHoldNote)Story.Notes.GetHoldNotes().First());
            AddStep("Set tool state", () => Editor.SetToolState(new SelectToolState()));
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

        private void DragStart() {
            AddStep("Move mouse to start", () => MoveMouseTo(HoldNote.StartAnchor));
            AddStep("Press start", () => InputManager.PressButton(MouseButton.Left));
            AddStep("Drag start", () => MoveMouseTo(Editor, new(-100, -100)));
            AddStep("Release start", () => InputManager.ReleaseButton(MouseButton.Left));
        }

        [Test]
        public void UpdateNoteCoordinates_UpdateStartAnchor_UpdatesStartCoordinates() {
            var oldCoordinates = new Vector2();
            AddStep("Set old coordinates", () => oldCoordinates = HoldNote.Coordinates);
            DragStart();
            AddAssert("Updates start coordinates", () => oldCoordinates != HoldNote.Coordinates);
        }

        [Test]
        public void UpdateNoteCoordinates_UpdateStartAnchorUndo_RevertsCoordinatesUpdate() {
            var oldCoordinates = new Vector2();
            AddStep("Set old coordinates", () => oldCoordinates = HoldNote.Coordinates);
            DragStart();
            Undo();
            AddAssert("Reverts coordinates update", () => oldCoordinates == HoldNote.Coordinates);
        }

        [Test]
        public void UpdateNoteCoordinates_UpdateStartAnchorRedo_RevertsCoordinatesUpdate() {
            var oldCoordinates = new Vector2();
            AddStep("Set old coordinates", () => oldCoordinates = HoldNote.Coordinates);
            DragStart();
            Undo();
            Redo();
            AddAssert("Updates start coordinates", () => oldCoordinates != HoldNote.Coordinates);
        }

        private void DragMid() {
            AddStep("Move mouse to mid", () => MoveMouseTo(HoldNote.MidAnchors[0]));
            AddStep("Press mid", () => InputManager.PressButton(MouseButton.Left));
            AddStep("Drag mid", () => MoveMouseTo(Editor, new(-100, -100)));
            AddStep("Release mid", () => InputManager.ReleaseButton(MouseButton.Left));
        }

        [Test]
        public void UpdateHoldNoteMidCoordinates_UpdateMidAnchor_UpdatesMidCoordinates() {
            var oldCoordinates = new Vector2();
            AddStep("Set old coordinates", () => oldCoordinates = HoldNote.MidCoordinates.First());
            DragMid();
            AddAssert("Updates mid coordinates", () => oldCoordinates != HoldNote.MidCoordinates.First());
        }

        [Test]
        public void UpdateHoldNoteMidCoordinates_UpdateMidAnchorUndo_RevertsCoordinatesUpdate() {
            var oldCoordinates = new Vector2();
            AddStep("Set old coordinates", () => oldCoordinates = HoldNote.MidCoordinates.First());
            DragMid();
            Undo();
            AddAssert("Reverts coordinates update", () => oldCoordinates == HoldNote.MidCoordinates.First());
        }

        [Test]
        public void UpdateHoldNoteMidCoordinates_UpdateMidAnchorRedo_RevertsCoordinatesUpdate() {
            var oldCoordinates = new Vector2();
            AddStep("Set old coordinates", () => oldCoordinates = HoldNote.MidCoordinates.First());
            DragMid();
            Undo();
            Redo();
            AddAssert("Updates mid coordinates", () => oldCoordinates != HoldNote.MidCoordinates.First());
        }

        private void DragEnd() {
            AddStep("Move mouse to end", () => MoveMouseTo(HoldNote.EndAnchor));
            AddStep("Press end", () => InputManager.PressButton(MouseButton.Left));
            AddStep("Drag end", () => MoveMouseTo(Editor, new(-100, -100)));
            AddStep("Release end", () => InputManager.ReleaseButton(MouseButton.Left));
        }

        [Test]
        public void UpdateHoldNoteEndCoordinates_UpdateEndAnchor_UpdatesEndCoordinates() {
            var oldCoordinates = new Vector2();
            AddStep("Set old coordinates", () => oldCoordinates = HoldNote.EndCoordinates);
            DragEnd();
            AddAssert("Updates end coordinates", () => oldCoordinates != HoldNote.EndCoordinates);
        }

        [Test]
        public void UpdateHoldNoteEndCoordinates_UpdateEndAnchorUndo_RevertsCoordinatesUpdate() {
            var oldCoordinates = new Vector2();
            AddStep("Set old coordinates", () => oldCoordinates = HoldNote.EndCoordinates);
            DragEnd();
            Undo();
            AddAssert("Reverts coordinates update", () => oldCoordinates == HoldNote.EndCoordinates);
        }

        [Test]
        public void UpdateHoldNoteEndCoordinates_UpdateEndAnchorRedo_RevertsCoordinatesUpdate() {
            var oldCoordinates = new Vector2();
            AddStep("Set old coordinates", () => oldCoordinates = HoldNote.EndCoordinates);
            DragEnd();
            Undo();
            Redo();
            AddAssert("Updates end coordinates", () => oldCoordinates != HoldNote.EndCoordinates);
        }
    }
}
