using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using S2VX.Game.Play.Score;
using S2VX.Game.Story;

namespace S2VX.Game.Tests.HeadlessTests.ScoreProcessorTests {
    [HeadlessTest]
    public class ProcessHoldSoundTests : S2VXTestScene {
        [Cached]
        private S2VXStory Story { get; } = new();

        private ScoreProcessor Processor { get; } = new();

        [BackgroundDependencyLoader]
        private void Load() => Add(Processor);

        [SetUpSteps]
        public void SetUpSteps() => AddStep("Reset score processor", () => Processor.Reset());

        private void ProcessHold(double scoreTime, bool isPress) =>
            AddStep("Process note", () => Processor.ProcessHold(scoreTime, 0, isPress, 0, 1000));

        [Test]
        public void ProcessHold_PressBeforeDuring_PlaysNoSound() {
            ProcessHold(-1, true);
            AddAssert("Plays no sound", () => Processor.Hit.PlayCount == 0);
        }

        [Test]
        public void ProcessHold_ReleaseBeforeDuring_PlaysNoSound() {
            ProcessHold(-1, false);
            AddAssert("Plays no sound", () => Processor.Hit.PlayCount == 0);
        }

        [Test]
        public void ProcessHold_PressDuring_PlaysNoSound() {
            ProcessHold(500, true);
            AddAssert("Plays no sound", () => Processor.Hit.PlayCount == 0);
        }

        [Test]
        public void ProcessHold_ReleaseDuring_PlaysMissSound() {
            ProcessHold(500, false);
            AddAssert("Plays miss sound", () => Processor.Miss.PlayCount == 1);
        }

        [Test]
        public void ProcessHold_EndWithPress_PlaysHitSound() {
            ProcessHold(1001, true);
            AddAssert("Plays hit sound", () => Processor.Hit.PlayCount == 1);
        }

        [Test]
        public void ProcessHold_EndWithRelease_PlaysMissSound() {
            ProcessHold(1001, false);
            AddAssert("Plays miss sound", () => Processor.Miss.PlayCount == 1);
        }
    }
}
