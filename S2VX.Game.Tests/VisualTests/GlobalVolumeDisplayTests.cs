using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osu.Framework.Utils;
using S2VX.Game.Play;
using S2VX.Game.Story;
using System;
using System.IO;

namespace S2VX.Game.Tests.VisualTests {
    public class GlobalVolumeDisplayTests : S2VXTestScene {
        [Resolved]
        private AudioManager Audio { get; set; }

        [Cached]
        private ScreenStack ScreenStack { get; } = new();
        [Cached]
        private GlobalVolumeDisplay VolumeDisplay { get; set; } = new();
        [Cached]
        private S2VXStory Story { get; set; } = new S2VXStory();
        private PlayScreen PlayScreen { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            Add(new ScreenStack(PlayScreen = new PlayScreen(false, Story, S2VXTrack.Open(audioPath, Audio))));
            Add(VolumeDisplay);
        }

        [SetUpSteps]
        private void SetUpSteps() => AddStep("Reset volume to 0.5", () => Audio.Volume.Value = 0.5d);

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)] // Tests scrolling up after reaching maximum volume
        public void GlobalVolumeDisplay_ScrollWheelUp_VolumeIsCorrect(int numScrolls) {
            AddStep("Move mouse over playScreen", () => MoveMouseTo(PlayScreen));
            for (var i = 0; i < numScrolls; ++i) {
                AddStep("Scroll wheel up", () => InputManager.ScrollVerticalBy(1));
            }
            var expectedVol = Math.Clamp(0.5 + 0.1 * numScrolls, 0d, 1d);
            AddAssert($"Volume is {expectedVol}", () => Precision.AlmostEquals(Audio.Volume.Value, expectedVol));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)] // Tests scrolling down after reaching minimum volume
        public void GlobalVolumeDisplay_ScrollWheelDown_VolumeIsCorrect(int numScrolls) {
            AddStep("Move mouse over playScreen", () => MoveMouseTo(PlayScreen));
            for (var i = 0; i < numScrolls; ++i) {
                AddStep("Scroll wheel down", () => InputManager.ScrollVerticalBy(-1));
            }
            var expectedVol = Math.Clamp(0.5 - 0.1 * numScrolls, 0d, 1d);
            AddAssert($"Volume is {expectedVol}", () => Precision.AlmostEquals(Audio.Volume.Value, expectedVol));
        }
    }
}
