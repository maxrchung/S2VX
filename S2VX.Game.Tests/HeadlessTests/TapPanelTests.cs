using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using osu.Framework.Timing;
using osuTK.Input;
using S2VX.Game.Editor.Containers;
using S2VX.Game.Editor.UserInterface;

namespace S2VX.Game.Tests.HeadlessTests {
    [HeadlessTest]
    public class TapPanelTests : S2VXTestScene {
        private TapPanel TapPanel { get; set; } = new();
        private TapReceptor TapReceptor { get; set; }
        private StopwatchClock TapClock { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            TapClock = new();
            TapPanel.Clock = new FramedClock(TapClock);
            TapReceptor = TapPanel.TapReceptor;
            TapPanel.ToggleVisibility();
            Add(TapPanel);
        }

        // All tests will have a note that starts to appear in 1 second
        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Reset clock", () => TapPanel.Clock = new FramedClock(TapClock = new()));
            AddStep("Reset tap panel", () => TapReceptor.Reset());
            AddStep("Move mouse", () => InputManager.MoveMouseTo(TapReceptor));
        }

        public void MouseClick() => AddStep("Click", () => InputManager.Click(MouseButton.Left));
        public void PressReleaseKey() => AddStep("Key press", () => InputManager.Key(Key.F));
        public void Seek(double time) => AddStep($"Seek to {time}", () => TapClock.Seek(time));
        public void AssertTaps(int taps) => AddAssert($"Is {taps} taps", () => TapReceptor.TapsLabel.Value == $"Taps: {taps}");
        public void AssertBPM(int bpm) => AddAssert($"Is {bpm} BPM", () => TapReceptor.BPMLabel.Value == $"BPM: {bpm}");

        [Test]
        public void ProcessTap_0Taps_Is0Taps() => AssertTaps(0);

        [Test]
        public void ProcessTap_0Taps_Is0BPM() => AssertTaps(0);

        [Test]
        public void ProcessTap_1TapIn1Second_Is1Tap() {
            MouseClick();
            AssertTaps(1);
        }

        [Test]
        public void ProcessTap_1TapIn1Second_Is0BPM() {
            MouseClick();
            AssertBPM(0);
        }

        [Test]
        public void ProcessTap_2TapsIn1Second_Is2Taps() {
            MouseClick();
            Seek(1000);
            MouseClick();
            AssertTaps(2);
        }

        [Test]
        public void ProcessTap_2TapsIn1Second_Is60BPM() {
            MouseClick();
            Seek(1000);
            MouseClick();
            AssertBPM(60);
        }

        [Test]
        public void ProcessTap_3TapsIn1Second_Is3Taps() {
            MouseClick();
            Seek(500);
            MouseClick();
            Seek(1000);
            MouseClick();
            AssertTaps(3);
        }

        [Test]
        public void ProcessTap_3TapsIn1Second_Is120BPM() {
            MouseClick();
            Seek(500);
            MouseClick();
            Seek(1000);
            MouseClick();
            AssertBPM(120);
        }

        [Test]
        public void ProcessTap_3KeyPressesIn1Second_Is3Taps() {
            PressReleaseKey();
            Seek(500);
            PressReleaseKey();
            Seek(1000);
            PressReleaseKey();
            AssertTaps(3);
        }

        [Test]
        public void ProcessTap_3KeyPressesIn1Second_Is120BPM() {
            PressReleaseKey();
            Seek(500);
            PressReleaseKey();
            Seek(1000);
            PressReleaseKey();
            AssertBPM(120);
        }
    }
}
