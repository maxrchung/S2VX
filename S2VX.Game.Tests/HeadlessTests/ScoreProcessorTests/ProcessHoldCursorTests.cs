using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using S2VX.Game.Play.Score;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Tests.HeadlessTests.ScoreProcessorTests {
    [HeadlessTest]
    public class ProcessHoldCursorTests : S2VXTestScene {
        [Resolved]
        private S2VXCursor Cursor { get; set; }

        [Cached]
        private S2VXStory Story { get; } = new();

        private ScoreProcessor Processor { get; } = new();

        [BackgroundDependencyLoader]
        private void Load() => Add(Processor);

        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Reset cursor", () => Cursor.Reset());
            AddStep("Reset score processor", () => Processor.Reset());
        }

        private void ProcessHold(double scoreTime, bool isPress) =>
            AddStep("Process note", () => Processor.ProcessHold(scoreTime, 0, isPress, 0, 1000));

        [Test]
        public void ProcessHold_PressBeforeDuring_DoesNotColorCursor() {
            ProcessHold(-1, true);
            AddAssert("Does not color cursor", () => Cursor.ActiveCursor.Colour == Notes.PerfectColor);
        }

        [Test]
        public void ProcessHold_ReleaseBeforeDuring_DoesNotColorCursor() {
            ProcessHold(-1, false);
            AddAssert("Does not color cursor", () => Cursor.ActiveCursor.Colour == Notes.PerfectColor);
        }

        [Test]
        public void ProcessHold_PressDuring_ColorCursorLate() {
            ProcessHold(500, true);
            AddAssert("Colors cursor late", () => Cursor.ActiveCursor.Colour == Notes.LateColor);
        }

        [Test]
        public void ProcessHold_ReleaseDuring_ColorCursorMiss() {
            ProcessHold(500, false);
            AddAssert("Colors cursor miss", () => Cursor.ActiveCursor.Colour == Notes.MissColor);
        }

        [Test]
        public void ProcessHold_EndWithPress_DoesNotColorCursor() {
            ProcessHold(1001, true);
            AddAssert("Does not color cursor", () => Cursor.ActiveCursor.Colour == Notes.PerfectColor);
        }

        [Test]
        public void ProcessHold_EndWithRelease_ColorCursorMiss() {
            ProcessHold(1001, false);
            AddAssert("Color cursor miss", () => Cursor.ActiveCursor.Colour == Notes.MissColor);
        }
    }
}
