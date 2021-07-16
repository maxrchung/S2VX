using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using S2VX.Game.Play.Score;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Tests.HeadlessTests.ScoreProcessorTests {
    [HeadlessTest]
    public class ProcessHitCursorTests : S2VXTestScene {
        [Resolved]
        private S2VXCursor Cursor { get; set; }

        [Cached]
        private S2VXStory Story { get; } = new();

        private ScoreProcessor ScoreProcessor { get; } = new();

        [BackgroundDependencyLoader]
        private void Load() => Add(ScoreProcessor);

        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Reset cursor", () => Cursor.Reset());
            AddStep("Reset score processor", () => ScoreProcessor.Reset());
        }

        private void ProcessHit(double scoreTime) =>
            AddStep("Process note", () => ScoreProcessor.ProcessHit(scoreTime, 0));

        [Test]
        public void ProcessHit_PerfectHit_ColorsCursorPerfect() {
            ProcessHit(0);
            AddAssert("Colors cursor perfect", () => Cursor.ActiveCursor.Colour == Notes.PerfectColor);
        }

        [Test]
        public void ProcessHit_EarlyHit_ColorsCursorEarly() {
            ProcessHit(-Notes.PerfectThreshold - 1);
            AddAssert("Colors cursor early", () => Cursor.ActiveCursor.Colour == Notes.EarlyColor);
        }

        [Test]
        public void ProcessHit_LateHit_ColorsCursorLate() {
            ProcessHit(Notes.PerfectThreshold + 1);
            AddAssert("Colors cursor late", () => Cursor.ActiveCursor.Colour == Notes.LateColor);
        }

        [Test]
        public void ProcessHit_EarlyMissHit_ColorsCursorMiss() {
            ProcessHit(-Notes.HitThreshold - 1);
            AddAssert("Colors cursor miss", () => Cursor.ActiveCursor.Colour == Notes.MissColor);
        }

        [Test]
        public void ProcessHit_LateMissHit_ColorsCursorMiss() {
            ProcessHit(Notes.HitThreshold + 1);
            AddAssert("Colors cursor miss", () => Cursor.ActiveCursor.Colour == Notes.MissColor);
        }

        [Test]
        public void ProcessHit_BeforeMissHit_DoesNotColorCursor() {
            ProcessHit(-Notes.MissThreshold - 1);
            AddAssert("Does not color cursor", () => Cursor.ActiveCursor.Colour == Notes.PerfectColor);
        }

        [Test]
        public void ProcessHit_AfterMissHit_ColorsCursorMiss() {
            ProcessHit(Notes.MissThreshold + 1);
            AddAssert("Colors cursor miss", () => Cursor.ActiveCursor.Colour == Notes.MissColor);
        }
    }
}
