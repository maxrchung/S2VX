using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Utils;
using osuTK;
using osuTK.Input;
using S2VX.Game.Editor;
using S2VX.Game.Story;
using System.IO;
using System.Linq;

namespace S2VX.Game.Tests.VisualTests.EditorScreenTests {
    public class NotesTimelineTests : S2VXTestScene {
        private EditorScreen Editor { get; set; }
        private S2VXTrack Track { get; set; }
        private S2VXStory Story { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            var storyPath = Path.Combine("VisualTests", "EditorScreenTests", "ValidStory.s2ry");
            Story = new S2VXStory();
            Story.Open(storyPath, true);
            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            Track = S2VXTrack.Open(audioPath, audio);

            Add(new ScreenStack(Editor = new EditorScreen(Story, Track)));
        }

        [Test]
        public void SnapToTick_AfterTimingChange_SnapsToRightTick() {
            AddStep("Seek after timing change", () => Editor.Seek(100));
            AddStep("Snap to the right", () => Editor.NotesTimeline.SnapToTick(false));
            AddAssert("Snapped to right time", () => Precision.AlmostEquals(Track.CurrentTime, 350));
        }

        [Test]
        public void SnapToTick_AfterTimingChange_SnapsToLeftTick() {
            AddStep("Seek after timing change", () => Editor.Seek(300));
            AddStep("Snap to the left", () => Editor.NotesTimeline.SnapToTick(true));
            AddAssert("Snapped to right time", () => Precision.AlmostEquals(Track.CurrentTime, 100));
        }

        [Test]
        public void SnapToTick_TransitionTimingChange_SnapsToTimeBasedOnCurrentBPM() {
            AddStep("Seek to beginning", () => Editor.Seek(0));
            AddStep("Snap to the right", () => Editor.NotesTimeline.SnapToTick(false));
            AddAssert("Snapped to right time", () => Precision.AlmostEquals(Track.CurrentTime, 125));
        }

        [Test]
        public void OnToolDrag_DragTimelineNoteRight_ScrollsTimelineRight() {
            var oldFirstVisibleTick = int.MinValue;
            AddStep("Seek to beginning", () => Editor.Seek(0));
            AddStep("Init value to compare with", () => oldFirstVisibleTick = Editor.NotesTimeline.FirstVisibleTick);
            AddStep("Move mouse to note", () => InputManager.MoveMouseTo(Editor.NotesTimeline.NoteToTimelineNote.Values.First()));
            AddStep("LMouse down", () => InputManager.PressButton(MouseButton.Left));
            AddStep("Move mouse past right edge of NotesTimeline", () =>
                InputManager.MoveMouseTo(Editor.NotesTimeline.TickBarContent, new Vector2(Editor.NotesTimeline.TickBarContent.DrawWidth, 0)));
            AddStep("LMouse up", () => InputManager.ReleaseButton(MouseButton.Left));
            AddStep("Undo note move", () => {
                InputManager.PressKey(Key.LControl);
                InputManager.PressKey(Key.Z);
                InputManager.ReleaseKey(Key.LControl);
                InputManager.ReleaseKey(Key.Z);
            });
            AddAssert("NotesTimeline ticks have scrolled right", () => oldFirstVisibleTick < Editor.NotesTimeline.FirstVisibleTick);
        }

        [Test]
        public void OnToolDrag_DragTimelineNoteLeft_ScrollsTimelineLeft() {
            var oldFirstVisibleTick = int.MinValue;
            AddStep("Seek to 2.1s", () => Editor.Seek(2100));
            AddStep("Init value to compare with", () => oldFirstVisibleTick = Editor.NotesTimeline.FirstVisibleTick);
            AddStep("Move mouse to note", () => InputManager.MoveMouseTo(Editor.NotesTimeline.NoteToTimelineNote.Values.First()));
            AddStep("LMouse down", () => InputManager.PressButton(MouseButton.Left));
            AddStep("Move mouse past left edge of NotesTimeline", () =>
                InputManager.MoveMouseTo(Editor.NotesTimeline.TickBarContent, new Vector2(-Editor.NotesTimeline.TickBarContent.DrawWidth, 0)));
            AddStep("LMouse up", () => InputManager.ReleaseButton(MouseButton.Left));
            AddStep("Undo note move", () => {
                InputManager.PressKey(Key.LControl);
                InputManager.PressKey(Key.Z);
                InputManager.ReleaseKey(Key.LControl);
                InputManager.ReleaseKey(Key.Z);
            });
            AddAssert("NotesTimeline ticks have scrolled left", () => oldFirstVisibleTick > Editor.NotesTimeline.FirstVisibleTick);
        }
    }
}
