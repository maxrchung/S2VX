using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Tests.VisualTests {
    public class ScoreProcessorTests : S2VXTestScene {
        [Resolved]
        private S2VXCursor Cursor { get; set; }

        private ScoreProcessor Score { get; } = new();

        [BackgroundDependencyLoader]
        private void Load() => Add(Score);

        [SetUpSteps]
        public void SetUpSteps() => AddStep("Reset score", () => Score.Reset());

        [Test]
        public void Process_PerfectHit_ColorsCursorPerfect() {
            AddStep("Process note", () => Score.Process(1000, new GameNote {
                HitTime = 1000
            }));
            AddAssert("Colors cursor perfect", () => Cursor.ActiveCursor.Colour = Score.PerfectColor);
        }

        [Test]
        public void Process_EarlyHit_ColorsCursorEarly() {
            AddStep("Process note", () => Score.Process(1000, new GameNote {
                HitTime = 1000 - Score.PerfectThreshold - 1
            }));
            AddAssert("Colors cursor early", () => Cursor.ActiveCursor.Colour = Score.EarlyColor);
        }

        [Test]
        public void Process_LateHit_ColorsCursorLate() {
            AddStep("Process note", () => Score.Process(1000, new GameNote {
                HitTime = 1000 + Score.PerfectThreshold + 1
            }));
            AddAssert("Colors cursor late", () => Cursor.ActiveCursor.Colour = Score.LateColor);
        }

        [Test]
        public void Process_MissHit_ColorsCursorMiss() {
            AddStep("Process note", () => Score.Process(1000, new GameNote {
                HitTime = 1000 + Score.MissThreshold + 1
            }));
            AddAssert("Colors cursor miss", () => Cursor.ActiveCursor.Colour = Score.MissColor);
        }

        [Test]
        public void Process_PerfectHit_PlaysHitSound() {
            AddStep("Process note", () => Score.Process(1000, new GameNote {
                HitTime = 1000
            }));
            AddAssert("Plays hit sound", () => Score.Hit.PlayCount = 1);
        }

        [Test]
        public void Process_MissHit_PlaysMissSound() {
            AddStep("Process note", () => Score.Process(1000, new GameNote {
                HitTime = 1000 - Score.MissThreshold - 1
            }));
            AddAssert("Plays miss sound", () => Score.Miss.PlayCount = 1);
        }
    }
}
