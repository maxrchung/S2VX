using NUnit.Framework;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game.Tests {
    [TestFixture]
    public class S2VXUtilsTests {
        [Test]
        public void RotateZero() {
            var testInput = new Vector2(12.345f, -67.89f);
            var expected = testInput;
            var tolerance = 0.001;
            var result = S2VXUtils.Rotate(testInput, 0);
            Assert.AreEqual(expected.X, result.X, tolerance);
            Assert.AreEqual(expected.Y, result.Y, tolerance);
        }

        [Test]
        public void Rotate360() {
            var testInput = new Vector2(12.345f, -67.89f);
            var expected = testInput;
            var tolerance = 0.001;
            var result = S2VXUtils.Rotate(testInput, 360);
            Assert.AreEqual(expected.X, result.X, tolerance);
            Assert.AreEqual(expected.Y, result.Y, tolerance);
        }

        [Test]
        public void Rotate90() {
            var testInput = new Vector2(12.345f, -67.89f);
            var expected = new Vector2(67.89f, 12.345f);
            var tolerance = 0.001;
            var result = S2VXUtils.Rotate(testInput, 90);
            Assert.AreEqual(expected.X, result.X, tolerance);
            Assert.AreEqual(expected.Y, result.Y, tolerance);
        }

        [Test]
        public void FloatToStringZeroPrecision() {
            var testInput = 3.141593f;
            var expected = "3.141593";
            var result = S2VXUtils.FloatToString(testInput);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void FloatToStringOnePrecision() {
            var testInput = 3.141593f;
            var expected = "3.1";
            var result = S2VXUtils.FloatToString(testInput, 1);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void FloatToStringFivePrecision() {
            var testInput = 3.141593f;
            var expected = "3.14159";
            var result = S2VXUtils.FloatToString(testInput, 5);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Vector2ToStringZeroPrecision() {
            var testInput = new Vector2(1.234567f, -7.890123f);
            var expected = "(1.234567,-7.890123)";
            var result = S2VXUtils.Vector2ToString(testInput);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Vector2ToStringOnePrecision() {
            var testInput = new Vector2(1.234567f, -7.890123f);
            var expected = "(1.2,-7.9)";
            var result = S2VXUtils.Vector2ToString(testInput, 1);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Vector2ToStringFivePrecision() {
            var testInput = new Vector2(1.234567f, -7.890123f);
            var expected = "(1.23457,-7.89012)";
            var result = S2VXUtils.Vector2ToString(testInput, 5);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Color4ToStringBlack() {
            var testInput = Color4.Black;
            var expected = "(0,0,0)";
            var result = S2VXUtils.Color4ToString(testInput);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Color4ToStringWhite() {
            var testInput = Color4.White;
            var expected = "(1,1,1)";
            var result = S2VXUtils.Color4ToString(testInput);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Vector2FromString() {
            var testInput = "(1.234567,-7.890123)";
            var expected = new Vector2(1.234567f, -7.890123f);
            var result = S2VXUtils.Vector2FromString(testInput);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Color4FromStringBlack() {
            var testInput = "(0,0,0)";
            var expected = Color4.Black;
            var result = S2VXUtils.Color4FromString(testInput);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Color4FromStringWhite() {
            var testInput = "(1,1,1)";
            var expected = Color4.White;
            var result = S2VXUtils.Color4FromString(testInput);
            Assert.AreEqual(expected, result);
        }
    }
}
