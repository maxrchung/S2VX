using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using S2VX.Game.Play.Score;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Tests.HeadlessTests.ScoreProcessorTests {
    [HeadlessTest]
    public class ProcessHitSoundTests : S2VXTestScene {
        [Cached]
        private S2VXStory Story { get; } = new();

        private Notes Notes { get; set; }
        private ScoreProcessor Processor { get; } = new();

        [BackgroundDependencyLoader]
        private void Load() => Add(Processor);

        [SetUpSteps]
        public void SetUpSteps() {
            Notes = Story.Notes;
            AddStep("Reset score processor", () => Processor.Reset());
        }

        private void ProcessHit(double scoreTime) =>
            AddStep("Process note", () => Processor.ProcessHit(scoreTime, 0));

        [Test]
        public void ProcessHit_PerfectHit_PlaysHitSound() {
            ProcessHit(0);
            AddAssert("Plays hit sound", () => Processor.Hit.PlayCount == 1);
        }

        [Test]
        public void ProcessHit_EarlyHit_PlaysHitSound() {
            ProcessHit(-Notes.PerfectThreshold - 1);
            AddAssert("Plays hit sound", () => Processor.Hit.PlayCount == 1);
        }

        [Test]
        public void ProcessHit_LateHit_PlaysHitSound() {
            ProcessHit(Notes.PerfectThreshold + 1);
            AddAssert("Plays hit sound", () => Processor.Hit.PlayCount == 1);
        }

        [Test]
        public void ProcessHit_EarlyMissHit_PlaysMissSound() {
            ProcessHit(-Notes.HitThreshold - 1);
            AddAssert("Plays miss sound", () => Processor.Miss.PlayCount == 1);
        }

        [Test]
        public void ProcessHit_LateMissHit_PlaysMissSound() {
            ProcessHit(Notes.HitThreshold + 1);
            AddAssert("Plays miss sound", () => Processor.Miss.PlayCount == 1);
        }

        [Test]
        public void ProcessHit_BeforeMissHit_PlaysNoSound() {
            ProcessHit(-Notes.MissThreshold - 1);
            AddAssert("Plays no sound", () => Processor.Miss.PlayCount == 0);
        }

        [Test]
        public void ProcessHit_AfterMissHit_PlaysMissSound() {
            ProcessHit(Notes.MissThreshold + 1);
            AddAssert("Plays miss sound", () => Processor.Miss.PlayCount == 1);
        }
    }
}
