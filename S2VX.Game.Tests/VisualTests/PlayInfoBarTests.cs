using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK.Input;
using S2VX.Game.Configuration;
using S2VX.Game.Play;
using S2VX.Game.Story;
using System.IO;

namespace S2VX.Game.Tests.VisualTests {
    public class PlayInfoBarTests : S2VXTestScene {
        [Resolved]
        private S2VXConfigManager Config { get; set; }

        [Resolved]
        private AudioManager Audio { get; set; }

        [Cached]
        private GlobalVolumeDisplay VolumeDisplay { get; set; } = new();

        private S2VXStory Story { get; set; } = new S2VXStory();
        private PlayScreen PlayScreen { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            Add(new ScreenStack(PlayScreen = new PlayScreen(false, Story, S2VXTrack.Open(audioPath, Audio))));
        }

        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Unhide score info", () => Config.SetValue(S2VXSetting.ScoreVisibility, true));
            AddStep("Rehide hit error bar", () => Config.SetValue(S2VXSetting.HitErrorBarVisibility, false));
        }

        [Test]
        public void OnKeyDown_ShiftTabDown_HidesScoreInfo() {
            AddStep("Press shift", () => InputManager.PressKey(Key.ShiftLeft));
            AddStep("Press tab", () => InputManager.PressKey(Key.Tab));
            AddStep("Release shift", () => InputManager.ReleaseKey(Key.ShiftLeft));
            AddStep("Release tab", () => InputManager.ReleaseKey(Key.Tab));
            AddAssert("Score info is hidden", () => PlayScreen.ScoreProcessor.Alpha == 0);
        }

        [Test]
        public void OnKeyDown_ShiftE_ShowsHitErrorBar() {
            AddStep("Press shift", () => InputManager.PressKey(Key.ShiftLeft));
            AddStep("Press e", () => InputManager.PressKey(Key.E));
            AddStep("Release shift", () => InputManager.ReleaseKey(Key.ShiftLeft));
            AddStep("Release e", () => InputManager.ReleaseKey(Key.E));
            AddAssert("Hit error bar is visible", () => PlayScreen.HitErrorBar.Alpha == 1);
        }
    }
}
