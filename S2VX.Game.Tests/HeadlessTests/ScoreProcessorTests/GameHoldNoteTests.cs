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
    public class GameHoldNoteTests : S2VXTestScene {
        [Resolved]
        private S2VXCursor Cursor { get; set; }

        private S2VXStory Story { get; set; } = new S2VXStory();
        private PlayScreen PlayScreen { get; set; }
        private ScoreProcessor GetProcessor() => PlayScreen.ScoreProcessor;

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            Add(new ScreenStack(PlayScreen = new PlayScreen(false, Story, S2VXTrack.Open(audioPath, audio))));
        }

        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Reset cursor", () => Cursor.Reset());
            AddStep("Reset score processor", () => GetProcessor().Reset());
        }

        [Test]
        public void Process_PerfectHit_ColorsCursorPerfect() {
        }

        [Test]
        public void Process_EarlyHit_ColorsCursorEarly() {
        }

        [Test]
        public void Process_LateHit_ColorsCursorLate() {
        }

        [Test]
        public void Process_EarlyMissHit_ColorsCursorMiss() {
        }

        [Test]
        public void Process_LateMissHit_ColorsCursorMiss() {
        }

        [Test]
        public void Process_BeforeMissHit_DoesNotColorCursor() {
        }

        [Test]
        public void Process_AfterMissHit_ColorCursorMiss() {
        }

        [Test]
        public void Process_PressDuring_ColorCursorLate() {
        }

        [Test]
        public void Process_ReleaseDuring_ColorCursorMiss() {
        }

        [Test]
        public void Process_EndWithHold_DoesNotColorCursor() {
        }

        [Test]
        public void Process_EndWithoutHold_ColorCursorMiss() {
        }


        [Test]
        public void Process_PerfectHit_PlaysHitSound() {
        }

        [Test]
        public void Process_EarlyMissHit_PlaysMissSound() {
        }

        [Test]
        public void Process_LateMissHit_PlaysMissSound() {
        }

        [Test]
        public void Process_EarlyHit_PlaysHitSound() {
        }

        [Test]
        public void Process_LateHit_PlaysHitSound() {
        }

        [Test]
        public void Process_BeforeMissHit_PlaysNoSound() {
        }

        [Test]
        public void Process_AfterMissHit_PlaysMissSound() {
        }

        [Test]
        public void Process_PressDuring_PlaysNoSound() {
        }

        [Test]
        public void Process_ReleaseDuring_PlaysMissSound() {
        }

        [Test]
        public void Process_EndWithHold_PlaysHitSound() {
        }

        [Test]
        public void Process_EndWithoutHold_PlaysMissSound() {
        }
    }
}
