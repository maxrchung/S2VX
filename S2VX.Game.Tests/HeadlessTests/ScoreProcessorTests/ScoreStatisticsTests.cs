using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using S2VX.Game.Play.UserInterface;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Tests.HeadlessTests.ScoreProcessorTests {
    [HeadlessTest]
    public class ScoreStatisticsTests : S2VXTestScene {
        [Cached]
        private S2VXStory Story { get; } = new();
        private Notes Notes { get; set; }
        private ScoreProcessor Processor { get; } = new();

        [BackgroundDependencyLoader]
        private void Load() {
            Notes = Story.Notes;
            Add(Processor);
        }

        [SetUpSteps]
        public void SetUpSteps() => AddStep("Reset score processor", () => Processor.Reset());

        private void ProcessHit(double scoreTime) =>
            AddStep("Process note", () => Processor.ProcessHit(scoreTime, 0));

        [Test]
        public void ProcessHit_PerfectHit_AddsToPerfectCount() {
            ProcessHit(0);
            AddAssert("Adds to perfect count", () => Processor.PerfectCount == 1);
        }

        [Test]
        public void ProcessHit_EarlyHit_AddsToEarlyCount() {
            ProcessHit(-Notes.PerfectThreshold - 1);
            AddAssert("Adds to early count", () => Processor.EarlyCount == 1);
        }

        [Test]
        public void ProcessHit_LateHit_AddsToLateCount() {
            ProcessHit(Notes.PerfectThreshold + 1);
            AddAssert("Adds to late count", () => Processor.LateCount == 1);
        }

        [Test]
        public void ProcessHit_EarlyMissHit_AddsToMissCount() {
            ProcessHit(-Notes.HitThreshold - 1);
            AddAssert("Adds to miss count", () => Processor.MissCount == 1);
        }

        [Test]
        public void ProcessHit_LateMissHit_AddsToMissCount() {
            ProcessHit(Notes.HitThreshold + 1);
            AddAssert("Adds to miss count", () => Processor.MissCount == 1);
        }

        [Test]
        public void ProcessHit_BeforeMissHit_DoesNotAddCount() {
            ProcessHit(-Notes.MissThreshold - 1);
            AddAssert("Does not add count", () =>
                Processor.PerfectCount == 0 &&
                Processor.EarlyCount == 0 &&
                Processor.LateCount == 0 &&
                Processor.MissCount == 0
            );
        }

        [Test]
        public void ProcessHit_AfterMissHit_AddsToMissCount() {
            ProcessHit(Notes.MissThreshold + 1);
            AddAssert("Adds to miss count", () => Processor.MissCount == 1);
        }
    }
}
