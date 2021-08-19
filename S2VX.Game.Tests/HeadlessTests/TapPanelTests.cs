using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osu.Framework.Timing;
using osuTK.Input;
using S2VX.Game.Editor;
using S2VX.Game.Editor.Containers;
using S2VX.Game.Editor.UserInterface;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;
using S2VX.Game.Story.Note;
using System.IO;

namespace S2VX.Game.Tests.HeadlessTests {
    [HeadlessTest]
    public class TapPanelTests : S2VXTestScene {
        private TapPanel TapPanel { get; set; }
        private TapReceptor TapReceptor { get; set; }


        private StopwatchClock TapClock { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            TapClock = new();
            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            var editor = new EditorScreen(new S2VXStory(), S2VXTrack.Open(audioPath, audio));
            TapPanel = editor.TapPanel;
            TapPanel.Clock = new FramedClock(TapClock);
            TapReceptor = TapPanel.TapReceptor;
            Add(new ScreenStack(editor));
        }

        // All tests will have a note that starts to appear in 1 second
        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Reset clock", () => TapPanel.Clock = new FramedClock(TapClock));
            AddStep("Reset tap panel", () => TapReceptor.Reset());
            AddStep("Move mouse", () => InputManager.MoveMouseTo(TapReceptor));
        }

        public void MouseClick() => AddStep("Click", () => InputManager.Click());
        public void KeyUp() => AddStep("Key up", () => InputManager.PressKey(Key.F));
        public void Seek(double time) => AddStep($"Seek to {time}", () => TapClock.Seek(time));
        public void AssertTaps(int taps) => AddAssert($"Is {taps} taps", () => TapReceptor.TapsLabel.Value == $"Taps: {taps}");
        public void AssertBPM(int bpm) => AddAssert($"Is {bpm} BPM", () => TapReceptor.BPMLabel.Value == $"BPM: {bpm}");

        [Test]
        public void ProcessTap_0Taps_Is0Taps() => AssertTaps(0);

        [Test]
        public void ProcessTap_0Taps_Is0BPM() => AssertTaps(0);

        [Test]
        public void ProcessTap_1ClickIn1Second_Is1Tap() {
            MouseClick();
            AssertTaps(1);
        }

        [Test]
        public void ProcessTap_1ClickIn1Second_Is0BPM() {
            MouseClick();
            AssertBPM(0);
        }

        [Test]
        public void ProcessTap_2KeyUpsIn1Second_Is2Taps() {
            KeyUp();
            Seek(1000);
            KeyUp();
            AssertTaps(2);
        }

        [Test]
        public void ProcessTap_2KeyUpsIn1Second_Is60BPM() {
            KeyUp();
            Seek(1000);
            KeyUp();
            AssertBPM(60);
        }

        [Test]
        public void ProcessTap_3TapsIn1Second_Is3Taps() {
            MouseClick();
            Seek(500);
            KeyUp();
            Seek(1000);
            MouseClick();
            AssertTaps(3);
        }

        [Test]
        public void ProcessTap_3TapsIn1Second_Is120BPM() {
            KeyUp();
            Seek(500);
            MouseClick();
            Seek(1000);
            KeyUp();
            AssertBPM(120);
        }
    }
}
