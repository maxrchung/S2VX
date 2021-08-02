using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osuTK.Input;
using S2VX.Game.Editor;
using S2VX.Game.Story;
using System.IO;

namespace S2VX.Game.Tests.VisualTests.EditorScreenTests {
    public class PreviewGameTests : S2VXTestScene {

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            var storyPath = Path.Combine("VisualTests", "EditorScreenTests", "ValidStory.s2ry");
            var story = new S2VXStory();
            story.Open(storyPath, true);

            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            var drawableTrack = S2VXTrack.Open(audioPath, audio);

            var editorScreen = new EditorScreen(story, drawableTrack);
            var screenStack = new ScreenStack(editorScreen);
            Add(screenStack);
        }

        [Test]
        public void OnKeyDown_PreviewGameplayShortcut_EntersPreviewGameplay() =>
            AddStep("Press G key", () => InputManager.PressKey(Key.G));
    }
}
