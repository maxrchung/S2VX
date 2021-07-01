using NUnit.Framework;
using osu.Framework.Testing;
using S2VX.Game.EndGame.UserInterface;

namespace S2VX.Game.Tests.VisualTests {
    public class ScoreGradeTests : S2VXTestScene {
        private ScoreGrade ScoreGrade { get; set; }

        [SetUpSteps]
        private void SetUpSteps() => AddStep("Clear drawables", () => Clear());

        [Test]
        public void Constructor_Accuracy100IsFullCombo_IsS2() {
            AddStep("Add score grade", () => Add(ScoreGrade = new ScoreGrade(1, true)));
            AddAssert("Is S2", () => ScoreGrade.Grade == "S" && ScoreGrade.IsSquared);
        }

        [Test]
        public void Constructor_Accuracy100IsNotFullCombo_IsA() {
            AddStep("Add score grade", () => Add(ScoreGrade = new ScoreGrade(1, false)));
            AddAssert("Is A", () => ScoreGrade.Grade == "A" && !ScoreGrade.IsSquared);
        }

        [Test]
        public void Constructor_Accuracy95IsFullCombo_IsS() {
            AddStep("Add score grade", () => Add(ScoreGrade = new ScoreGrade(0.95, true)));
            AddAssert("Is S", () => ScoreGrade.Grade == "S" && !ScoreGrade.IsSquared);
        }

        [Test]
        public void Constructor_Accuracy95IsNotFullCombo_IsA() {
            AddStep("Add score grade", () => Add(ScoreGrade = new ScoreGrade(0.95, false)));
            AddAssert("Is S", () => ScoreGrade.Grade == "A" && !ScoreGrade.IsSquared);
        }

        [Test]
        public void Constructor_Accuracy90_IsA() {
            AddStep("Add score grade", () => Add(ScoreGrade = new ScoreGrade(0.9, false)));
            AddAssert("Is A", () => ScoreGrade.Grade == "A");
        }

        [Test]
        public void Constructor_Accuracy80_IsB() {
            AddStep("Add score grade", () => Add(ScoreGrade = new ScoreGrade(0.8, false)));
            AddAssert("Is B", () => ScoreGrade.Grade == "B");
        }

        [Test]
        public void Constructor_Accuracy70_IsC() {
            AddStep("Add score grade", () => Add(ScoreGrade = new ScoreGrade(0.7, false)));
            AddAssert("Is C", () => ScoreGrade.Grade == "C");
        }

        [Test]
        public void Constructor_Accuracy60_IsD() {
            AddStep("Add score grade", () => Add(ScoreGrade = new ScoreGrade(0.6, false)));
            AddAssert("Is D", () => ScoreGrade.Grade == "D");
        }

        [Test]
        public void Constructor_Accuracy50_IsE() {
            AddStep("Add score grade", () => Add(ScoreGrade = new ScoreGrade(0.5, false)));
            AddAssert("Is E", () => ScoreGrade.Grade == "E");
        }

        [Test]
        public void Constructor_Accuracy49_IsF() {
            AddStep("Add score grade", () => Add(ScoreGrade = new ScoreGrade(0.49, false)));
            AddAssert("Is F", () => ScoreGrade.Grade == "F");
        }
    }
}
