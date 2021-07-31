using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Testing;
using osu.Framework.Utils;
using System;

namespace S2VX.Game.Tests.VisualTests {
    public class GlobalVolumeTests : S2VXTestScene {
        [Resolved]
        private AudioManager Audio { get; set; }

        [Cached]
        private GlobalVolumeDisplay VolumeDisplay { get; set; } = new();

        [BackgroundDependencyLoader]
        private void Load() => Add(VolumeDisplay);

        [SetUpSteps]
        private void SetUpSteps() => AddStep("Reset volume to 0.5", () => Audio.Volume.Value = 0.5);

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)] // Tests scrolling up after reaching maximum volume
        public void GlobalVolumeDisplay_ScrollWheelUp_VolumeIsTen(int numScrolls) {
            for (var i = 0; i < numScrolls; ++i) {
                AddStep("Increase volume once", () => VolumeDisplay.VolumeIncrease());
            }
            var expectedVol = Math.Clamp(0.5 + 0.1 * numScrolls, 0d, 1d);
            AddAssert($"Volume is {expectedVol} Here:", () => Precision.AlmostEquals(Audio.Volume.Value, expectedVol));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)] // Tests scrolling down after reaching minimum volume
        public void GlobalVolumeDisplay_ScrollWheelUp_VolumeIsTeasn(int numScrolls) {
            for (var i = 0; i < numScrolls; ++i) {
                AddStep("Decrease volume once", () => VolumeDisplay.VolumeDecrease());
            }
            var expectedVol = Math.Clamp(0.5 - 0.1 * numScrolls, 0d, 1d);
            AddAssert($"Volume is {expectedVol} Here:", () => Precision.AlmostEquals(Audio.Volume.Value, expectedVol));
        }
    }
}
