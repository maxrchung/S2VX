using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using S2VX.Game.Play.UserInterface;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Tests.VisualTests {
    public class ScoreProcessorTests : S2VXTestScene {
        [Resolved]
        private S2VXCursor Cursor { get; set; }

        [Cached]
        private ScoreProcessor ScoreProcessor { get; } = new();

        [BackgroundDependencyLoader]
        private void Load() => Add(ScoreProcessor);

        [SetUpSteps]
        public void SetUpSteps() => AddStep("Reset score processor", () => ScoreProcessor.Reset());

        [Test]
        public void Process_PerfectHit_ColorsCursorPerfect() {
            AddStep("Process note", () => ScoreProcessor.Process(
                1000,
                new GameNote { HitTime = 1000 }
            ));
            AddAssert("Colors cursor perfect", () => Cursor.ActiveCursor.Colour = ScoreProcessor.PerfectColor);
        }

        [Test]
        public void Process_EarlyHit_ColorsCursorEarly() {
            AddStep("Process note", () => ScoreProcessor.Process(
                1000 - ScoreProcessor.PerfectThreshold - 1,
                new GameNote { HitTime = 1000 }
            ));
            AddAssert("Colors cursor early", () => Cursor.ActiveCursor.Colour = ScoreProcessor.EarlyColor);
        }

        [Test]
        public void Process_LateHit_ColorsCursorLate() {
            AddStep("Process note", () => ScoreProcessor.Process(
                1000 + ScoreProcessor.PerfectThreshold + 1,
                new GameNote { HitTime = 1000 }
            ));
            AddAssert("Colors cursor late", () => Cursor.ActiveCursor.Colour = ScoreProcessor.LateColor);
        }

        [Test]
        public void Process_MissHit_ColorsCursorMiss() {
            AddStep("Process note", () => ScoreProcessor.Process(
                1000 + ScoreProcessor.MissThreshold + 1,
                new GameNote { HitTime = 1000 }
            ));
            AddAssert("Colors cursor miss", () => Cursor.ActiveCursor.Colour = ScoreProcessor.MissColor);
        }

        [Test]
        public void Process_PerfectHit_PlaysHitSound() {
            AddStep("Process note", () => ScoreProcessor.Process(
                1000,
                new GameNote { HitTime = 1000 }
            ));
            AddAssert("Plays hit sound", () => ScoreProcessor.Hit.PlayCount = 1);
        }

        [Test]
        public void Process_MissHit_PlaysMissSound() {
            AddStep("Process note", () => ScoreProcessor.Process(
                1000 - ScoreProcessor.MissThreshold - 1,
                new GameNote { HitTime = 1000 }
            ));
            AddAssert("Plays miss sound", () => ScoreProcessor.Miss.PlayCount = 1);
        }
    }
}
