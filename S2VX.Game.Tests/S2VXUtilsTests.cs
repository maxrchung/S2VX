using NUnit.Framework;
using osuTK;

namespace S2VX.Game.Tests {
    [TestFixture]
    public class S2VXUtilsTests {
        [Test]
        public void FloatToStringZeroPrecision() {
            var testInput = 3.141593f;
            var expectedString = "3.141593";
            var resultString = S2VXUtils.FloatToString(testInput);
            Assert.AreEqual(expectedString, resultString);
        }

        [Test]
        public void FloatToStringOnePrecision() {
            var testInput = 3.141593f;
            var expectedString = "3.1";
            var resultString = S2VXUtils.FloatToString(testInput, 1);
            Assert.AreEqual(expectedString, resultString);
        }

        [Test]
        public void FloatToStringFivePrecision() {
            var testInput = 3.141593f;
            var expectedString = "3.14159";
            var resultString = S2VXUtils.FloatToString(testInput, 5);
            Assert.AreEqual(expectedString, resultString);
        }

        [Test]
        public void Vector2ToStringZeroPrecision() {
            var testInput = new Vector2(1.234567f, -7.890123f);
            var expectedString = "(1.234567,-7.890123)";
            var resultString = S2VXUtils.Vector2ToString(testInput);
            Assert.AreEqual(expectedString, resultString);
        }

        [Test]
        public void Vector2ToStringOnePrecision() {
            var testInput = new Vector2(1.234567f, -7.890123f);
            var expectedString = "(1.2,-7.9)";
            var resultString = S2VXUtils.Vector2ToString(testInput, 1);
            Assert.AreEqual(expectedString, resultString);
        }

        [Test]
        public void Vector2ToStringFivePrecision() {
            var testInput = new Vector2(1.234567f, -7.890123f);
            var expectedString = "(1.23457,-7.89012)";
            var resultString = S2VXUtils.Vector2ToString(testInput, 5);
            Assert.AreEqual(expectedString, resultString);
        }
    }
}
