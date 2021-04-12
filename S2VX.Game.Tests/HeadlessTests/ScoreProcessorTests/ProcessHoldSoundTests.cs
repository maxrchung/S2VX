using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using S2VX.Game.Play;
using S2VX.Game.Play.UserInterface;
using S2VX.Game.Story;
using System.IO;

namespace S2VX.Game.Tests.HeadlessTests.ScoreProcessorTests {
    [HeadlessTest]
    public class ProcessHoldSoundTests : S2VXTestScene {
        [Resolved]
        private S2VXCursor Cursor { get; set; }

        private PlayScreen PlayScreen { get; set; }
        private ScoreProcessor GetProcessor() => PlayScreen.ScoreProcessor;

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            Add(new ScreenStack(PlayScreen = new PlayScreen(false, new S2VXStory(), S2VXTrack.Open(audioPath, audio))));
        }

        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Reset cursor", () => Cursor.Reset());
            AddStep("Reset score processor", () => GetProcessor().Reset());
        }

        private void ProcessHold(double scoreTime, bool isPress) =>
            AddStep("Process note", () => GetProcessor().ProcessHold(scoreTime, 0, isPress, 0, 1000));

        [Test]
        public void ProcessHold_PressBeforeDuring_PlaysNoSound() {
            ProcessHold(-1, true);
            AddAssert("Plays no sound", () => GetProcessor().Hit.PlayCount == 0);
        }

        [Test]
        public void ProcessHold_ReleaseBeforeDuring_PlaysNoSound() {
            ProcessHold(-1, false);
            AddAssert("Plays no sound", () => GetProcessor().Hit.PlayCount == 0);
        }

        [Test]
        public void ProcessHold_PressDuring_PlaysNoSound() {
            ProcessHold(500, true);
            AddAssert("Plays no sound", () => GetProcessor().Hit.PlayCount == 0);
        }

        [Test]
        public void ProcessHold_ReleaseDuring_PlaysMissSound() {
            ProcessHold(500, false);
            AddAssert("Plays miss sound", () => GetProcessor().Miss.PlayCount == 1);
        }

        [Test]
        public void ProcessHold_EndWithPress_PlaysHitSound() {
            ProcessHold(1001, true);
            AddAssert("Plays hit sound", () => GetProcessor().Hit.PlayCount == 1);
        }

        [Test]
        public void ProcessHold_EndWithRelease_PlaysMissSound() {
            ProcessHold(1001, false);
            AddAssert("Plays miss sound", () => GetProcessor().Miss.PlayCount == 1);
        }
    }
}
