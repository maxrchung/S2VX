using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using S2VX.Game.Editor;
using S2VX.Game.Play;
using S2VX.Game.Story;
using System.IO;

namespace S2VX.Game.Tests.HeadlessTests.S2VXCursorTests {
    [HeadlessTest]
    public class CursorReset : S2VXTestScene {
        [Cached]
        private ScreenStack ScreenStack { get; set; } = new();

        [Cached]
        private S2VXCursor Cursor { get; set; } = new();

        [Resolved]
        private AudioManager Audio { get; set; }

        private string AudioPath { get; } = Path.Combine("TestTracks", "1-second-of-silence.mp3");
        private EditorScreen EditorScreen { get; set; }
        private PlayScreen PlayScreen { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            var story = new S2VXStory();
            var track = S2VXTrack.Open(AudioPath, Audio);
            ScreenStack.Push(EditorScreen = new EditorScreen(story, track));

            story = new S2VXStory();
            track = S2VXTrack.Open(AudioPath, Audio);
            ScreenStack.Push(PlayScreen = new PlayScreen(false, story, track));

            Add(ScreenStack);
            Add(Cursor);
        }

        [Test]
        public void Reset_EditorScreenExit_ResetsCursorProperties() {
            AddStep("Update cursor rotation", () => Cursor.ActiveCursor.Rotation = 1);
            AddStep("Exit editor screen", () => EditorScreen.OnExiting(null));
            AddAssert("Resets cursor properties", () => Cursor.ActiveCursor.Rotation == 0);
        }

        [Test]
        public void Reset_PlayScreenExit_ResetsCursorProperties() {
            AddStep("Update cursor rotation", () => Cursor.ActiveCursor.Rotation = 1);
            AddStep("Exit play screen", () => PlayScreen.OnExiting(null));
            AddAssert("Resets cursor properties", () => Cursor.ActiveCursor.Rotation == 0);
        }
    }
}
