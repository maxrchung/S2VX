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
        private void Load() => Add(Processor);

        [SetUpSteps]
        public void SetUpSteps() {
            // Tried to put this line into Load() but ran into an issue where if
            // I ran a test by itself through the Test Explorer, the Load()
            // wasn't triggered correctly
            Notes = Story.Notes;

            AddStep("Reset score processor", () => Processor.Reset());
        }

        private void ProcessHit(double scoreTime) =>
            AddStep("Process note", () => Processor.ProcessHit(scoreTime, 0));

        private void ProcessHold(double scoreTime, bool isPress) =>
            AddStep("Process note", () => Processor.ProcessHold(scoreTime, 0, isPress, 0, 1000));

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

        [Test]
        public void ProcessHit_NoComboBreak_IncrementsCombo() {
            ProcessHit(10);
            ProcessHit(20);
            ProcessHit(30);
            AddAssert("Increments combo", () => Processor.Combo == 3);
        }

        [Test]
        public void ProcessHit_ComboBreak_ResetsCombo() {
            ProcessHit(10);
            ProcessHit(20);
            ProcessHit(30);
            ProcessHit(4000);
            ProcessHit(50);
            AddAssert("Resets combo", () => Processor.Combo == 1);
        }

        [Test]
        public void ProcessHit_ComboBreak_UpdatesMaxCombo() {
            ProcessHit(10);
            ProcessHit(20);
            ProcessHit(30);
            ProcessHit(4000);
            ProcessHit(50);
            AddAssert("Updates max combo", () => Processor.MaxCombo == 3);
        }

        [Test]
        public void ProcessHold_ReleaseDuring_AddsToMissCount() {
            ProcessHold(500, false);
            AddAssert("Colors cursor miss", () => Processor.MissCount == 1);
        }

        [Test]
        public void Accuracy_1Perfect1Miss_IsHalf() {
            ProcessHit(0);
            ProcessHit(Notes.MissThreshold + 1);
            AddAssert("Is half", () => Processor.Accuracy == 0.5);
        }

        [Test]
        public void Median_EvenNumberOfScores_IsAverageOfTwoMiddle() {
            ProcessHit(40);
            ProcessHit(20);
            ProcessHit(10);
            ProcessHit(30);
            AddAssert("Is average of two middle", () => Processor.Median() == 25);
        }

        [Test]
        public void Median_OddNumberOfScores_IsMiddle() {
            ProcessHit(20);
            ProcessHit(10);
            ProcessHit(30);
            AddAssert("Is middle", () => Processor.Median() == 20);
        }
    }
}
