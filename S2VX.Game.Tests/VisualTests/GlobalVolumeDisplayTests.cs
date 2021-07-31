using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using S2VX.Game.Play;
using S2VX.Game.Story;
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
        private void SetUpSteps() => AddStep("Reset volume to 0", () => Audio.Volume.Value = 0);

        [Test]
        public void GlobalVolumeDisplay_ScrollWheelUp_VolumeIsTen() {
            AddStep("Move mouse over song selection screen", () => InputManager.MoveMouseTo(PlayScreen));
            AddStep("Scroll wheel up", () => InputManager.ScrollVerticalBy(1));
            AddAssert("Volume is 0.1", () => Audio.Volume.Value == 0.1);
        }

        //[TestCase(1)]
        //[TestCase(2)]
        //[TestCase(3)]
        //[TestCase(4)]
        //[TestCase(5)]
        //[TestCase(6)]
        //[TestCase(7)] // Tests scrolling up after reaching maximum AR
        //public void EditorApproachRateDisplay_ScrollWheelUp_ApproachRateIsCorrect(int numScrolls) {
        //    AddStep("Move mouse over approach rate display", () => InputManager.MoveMouseTo(Editor.EditorInfoBar.ApproachRateDisplay));
        //    for (var i = 0; i < numScrolls; ++i) {
        //        AddStep("Scroll wheel up", () => InputManager.ScrollVerticalBy(1));
        //    }
        //    var expectedAR = Math.Clamp(Math.Pow(2, numScrolls), 1, 64);
        //    AddAssert($"Editor approach rate is {expectedAR}", () => Editor.EditorApproachRate == expectedAR);
        //}

        //[Test]
        //public void EditorApproachRateDisplay_ScrollWheelDownTwoTimes_ApproachRateIsStillOne() {
        //    AddStep("Move mouse over approach rate display", () => InputManager.MoveMouseTo(Editor.EditorInfoBar.ApproachRateDisplay));
        //    AddStep("Scroll wheel down", () => InputManager.ScrollVerticalBy(-1));
        //    AddStep("Scroll wheel down", () => InputManager.ScrollVerticalBy(-1));
        //    AddAssert("Editor approach rate is 1", () => Editor.EditorApproachRate == 1);
        //}
    }
}
