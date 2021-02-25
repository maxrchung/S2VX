using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osuTK.Input;
using S2VX.Game.Editor;
using S2VX.Game.Story;

namespace S2VX.Game.Tests.VisualTests {
    public class EditorScreenTests : S2VXTestScene {
        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            var story = new S2VXStory();
            var audioPath = "VisualTests/TestTracks/1-second-of-silence.mp3";
            var drawableTrack = S2VXUtils.LoadDrawableTrack(audioPath, audio);
            var editorScreen = new EditorScreen(null, story, drawableTrack);
            var screenStack = new ScreenStack(editorScreen);
            Add(screenStack);
        }

        [Test]
        public void OnKeyDown_PreviewGameplayShortcut_EntersPreviewGameplay() =>
            AddStep("Enters preview gameplay", () => InputManager.PressKey(Key.G));
    }
}
