using NUnit.Framework;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game.Tests {
    [TestFixture]
    public class S2VXUtilsTests {

        private const double FloatingPointTolerance = 0.001;

        [Test]
        public void Rotate_0() {
            var testInput = new Vector2(12.345f, -67.89f);
            var expected = testInput;
            var result = S2VXUtils.Rotate(testInput, 0);
            Assert.AreEqual(expected.X, result.X, FloatingPointTolerance);
            Assert.AreEqual(expected.Y, result.Y, FloatingPointTolerance);
        }

        [Test]
        public void Rotate_360() {
            var testInput = new Vector2(12.345f, -67.89f);
            var expected = testInput;
            var result = S2VXUtils.Rotate(testInput, 360);
            Assert.AreEqual(expected.X, result.X, FloatingPointTolerance);
            Assert.AreEqual(expected.Y, result.Y, FloatingPointTolerance);
        }

        [Test]
        public void Rotate_90() {
            var testInput = new Vector2(12.345f, -67.89f);
            var expected = new Vector2(67.89f, 12.345f);
            var result = S2VXUtils.Rotate(testInput, 90);
            Assert.AreEqual(expected.X, result.X, FloatingPointTolerance);
            Assert.AreEqual(expected.Y, result.Y, FloatingPointTolerance);
        }

        [Test]
        public void FloatToString_0Precision_IsExact() {
            var testInput = 3.141593f;
            var expected = "3.141593";
            var result = S2VXUtils.FloatToString(testInput);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void FloatToString_1Precision_IsRoundedTo1Place() {
            var testInput = 3.141593f;
            var expected = "3.1";
            var result = S2VXUtils.FloatToString(testInput, 1);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void FloatToString_5Precision_IsRoundedTo5Places() {
            var testInput = 3.141593f;
            var expected = "3.14159";
            var result = S2VXUtils.FloatToString(testInput, 5);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Vector2ToString_0Precision_IsExact() {
            var testInput = new Vector2(1.234567f, -7.890123f);
            var expected = "(1.234567,-7.890123)";
            var result = S2VXUtils.Vector2ToString(testInput);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Vector2ToString_1Precision_IsRoundedTo1Place() {
            var testInput = new Vector2(1.234567f, -7.890123f);
            var expected = "(1.2,-7.9)";
            var result = S2VXUtils.Vector2ToString(testInput, 1);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Vector2ToString_5Precision_IsRoundedTo5Places() {
            var testInput = new Vector2(1.234567f, -7.890123f);
            var expected = "(1.23457,-7.89012)";
            var result = S2VXUtils.Vector2ToString(testInput, 5);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Color4ToString_Black_Is0() {
            var testInput = Color4.Black;
            var expected = "(0,0,0)";
            var result = S2VXUtils.Color4ToString(testInput);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Color4ToString_White_Is1() {
            var testInput = Color4.White;
            var expected = "(1,1,1)";
            var result = S2VXUtils.Color4ToString(testInput);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Vector2FromString_RandomVector() {
            var testInput = "(1.234567,-7.890123)";
            var expected = new Vector2(1.234567f, -7.890123f);
            var result = S2VXUtils.Vector2FromString(testInput);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Color4FromString_0_IsBlack() {
            var testInput = "(0,0,0)";
            var expected = Color4.Black;
            var result = S2VXUtils.Color4FromString(testInput);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Color4FromString_1_IsWhite() {
            var testInput = "(1,1,1)";
            var expected = Color4.White;
            var result = S2VXUtils.Color4FromString(testInput);
            Assert.AreEqual(expected, result);
        }
    }
}
