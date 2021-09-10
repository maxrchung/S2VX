using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK.Input;
using S2VX.Game.Editor;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;
using System.IO;
using System.Linq;

namespace S2VX.Game.Tests.VisualTests.EditorScreenTests {
    public class EditorHideTests : S2VXTestScene {
        private S2VXStory Story { get; set; }
        private EditorScreen Editor { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            var storyPath = Path.Combine("VisualTests", "EditorScreenTests", "ValidStory.s2ry");
            Story = new S2VXStory();
            Story.Open(storyPath, true);

            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            var drawableTrack = S2VXTrack.Open(audioPath, audio);

            Editor = new EditorScreen(Story, drawableTrack);
            var screenStack = new ScreenStack(Editor);
            Add(screenStack);
        }

        [SetUpSteps]
        private void SetUpSteps() {
            AddStep("Set visible", () => Editor.EditorUIVisibility.Value = Visibility.Visible);
            AddStep("Press Ctrl key", () => InputManager.PressKey(Key.ControlLeft));
            AddStep("Press and release H key", () => InputManager.Key(Key.H));
            AddStep("Release Ctrl key", () => InputManager.ReleaseKey(Key.ControlLeft));
        }

        [Test]
        public void OnKeyDown_EditorUI_IsHidden() =>
            AddAssert("Editor UI is hidden", () => Editor.EditorUI.State.Value == Visibility.Hidden);

        [Test]
        public void OnKeyDown_HoldNoteAnchors_AreHidden() =>
            AddAssert("Hold note anchors are hidden", () => {
                var holdNotes = Story.Notes.GetHoldNotes().Cast<EditorHoldNote>();
                var hiddenCount = holdNotes.Count(holdNote => holdNote.AnchorContainer.State.Value == Visibility.Hidden);
                return hiddenCount == holdNotes.Count();
            });
    }
}
