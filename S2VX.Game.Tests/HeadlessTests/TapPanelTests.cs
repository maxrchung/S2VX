using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osu.Framework.Timing;
using S2VX.Game.Editor;
using S2VX.Game.Editor.Containers;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;
using S2VX.Game.Story.Note;
using System.IO;

namespace S2VX.Game.Tests.HeadlessTests {
    [HeadlessTest]
    public class TapPanelTests : S2VXTestScene {
        private TapPanel TapPanel { get; set; }

        private StopwatchClock TapClock { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            TapClock = new();
            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            var editor = new EditorScreen(new S2VXStory(), S2VXTrack.Open(audioPath, audio));
            TapPanel = editor.TapPanel;
            TapPanel.Clock = new FramedClock(TapClock);
            Add(new ScreenStack(editor));
        }

        // All tests will have a note that starts to appear in 1 second
        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Reset clock", () => TapPanel.Clock = new FramedClock(TapClock));
            AddStep("Reset tap panel", () => TapPanel.TapReceptor.Reset());
        }

        [Test]
        public void ProcessTap_0Taps_Is0Taps() =>
            AddAssert("Is 0 taps", () => TapPanel.TapReceptor.TapsLabel.Value == "Taps: 0");

        [Test]
        public void ProcessTap_0Taps_Is0BPM() =>
            AddAssert("Is 0 taps", () => TapPanel.TapReceptor.BPMLabel.Value == "BPM: 0");

        [Test]
        public void ProcessTap_1ClickIn1Second_Is1Tap() =>
            AddAssert("Is 0 taps", () => TapPanel.TapReceptor.BPMLabel.Value == "BPM: 0");

        [Test]
        public void ProcessTap_1KeyUpIn1Second_Is1Tap() =>
            AddAssert("Is 0 taps", () => TapPanel.TapReceptor.BPMLabel.Value == "BPM: 0");

        [Test]
        public void ProcessTap_1ClickIn1Second_Is60BPM() =>
            AddAssert("Is 0 taps", () => TapPanel.TapReceptor.BPMLabel.Value == "BPM: 0");

        [Test]
        public void ProcessTap_1KeyUpIn1Second_Is60BPM() =>
            AddAssert("Is 60 BPM", () => TapPanel.TapReceptor.BPMLabel.Value == "BPM: 0");

        [Test]
        public void ProcessTap_1Click1KeyUpIn1Second_Is2Taps() =>
            AddAssert("Is 2 taps", () => TapPanel.TapReceptor.BPMLabel.Value == "BPM: 0");

        [Test]
        public void ProcessTap_1Click1KeyUpIn1Second_Is120BPM() =>
            AddAssert("Is 120 BPM", () => TapPanel.TapReceptor.BPMLabel.Value == "BPM: 0");
    }
}
