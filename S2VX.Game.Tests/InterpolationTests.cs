using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game.Tests {
    [TestFixture]
    public class InterpolationTests {

        private const double FloatingPointTolerance = 0.001;

        [Test]
        public void ValueAt_Double_0StartTime0DurationUsesStartValue() {
            var inputCurrentTime = 0.0f;
            var inputStartValue = 0.1f;
            var inputEndValue = 1.0f;
            var inputStartTime = 0.0f;
            var inputEndTime = 0.0f;
            var inputEasing = Easing.None;
            var expected = inputStartValue;
            var actual = Interpolation.ValueAt(inputCurrentTime, inputStartValue, inputEndValue, inputStartTime, inputEndTime, inputEasing);
            Assert.AreEqual(expected, actual, FloatingPointTolerance);
        }

        [Test]
        public void ValueAt_Double_0DurationUsesStartValue() {
            var inputCurrentTime = 1.0f;
            var inputStartValue = 0.1f;
            var inputEndValue = 1.0f;
            var inputStartTime = 1.0f;
            var inputEndTime = 1.0f;
            var inputEasing = Easing.None;
            var expected = inputStartValue;
            var actual = Interpolation.ValueAt(inputCurrentTime, inputStartValue, inputEndValue, inputStartTime, inputEndTime, inputEasing);
            Assert.AreEqual(expected, actual, FloatingPointTolerance);
        }

        [Test]
        public void ValueAt_Color_0StartTime0DurationUsesStartValue() {
            var inputCurrentTime = 1.0f;
            var inputStartValue = Color4.White;
            var inputEndValue = Color4.Black;
            var inputStartTime = 0.0f;
            var inputEndTime = 0.0f;
            var inputEasing = Easing.None;
            var expected = inputStartValue;
            var actual = Interpolation.ValueAt(inputCurrentTime, inputStartValue, inputEndValue, inputStartTime, inputEndTime, inputEasing);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValueAt_Color_0DurationUsesStartValue() {
            var inputCurrentTime = 1.0f;
            var inputStartValue = Color4.White;
            var inputEndValue = Color4.Black;
            var inputStartTime = 1.0f;
            var inputEndTime = 1.0f;
            var inputEasing = Easing.None;
            var expected = inputStartValue;
            var actual = Interpolation.ValueAt(inputCurrentTime, inputStartValue, inputEndValue, inputStartTime, inputEndTime, inputEasing);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValueAt_Vector2_0StartTime0DurationUsesStartValue() {
            var inputCurrentTime = 1.0f;
            var inputStartValue = Vector2.Zero;
            var inputEndValue = Vector2.One;
            var inputStartTime = 0.0f;
            var inputEndTime = 0.0f;
            var inputEasing = Easing.None;
            var expected = inputStartValue;
            var actual = Interpolation.ValueAt(inputCurrentTime, inputStartValue, inputEndValue, inputStartTime, inputEndTime, inputEasing);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValueAt_Vector2_0DurationUsesStartValue() {
            var inputCurrentTime = 1.0f;
            var inputStartValue = Vector2.Zero;
            var inputEndValue = Vector2.One;
            var inputStartTime = 1.0f;
            var inputEndTime = 1.0f;
            var inputEasing = Easing.None;
            var expected = inputStartValue;
            var actual = Interpolation.ValueAt(inputCurrentTime, inputStartValue, inputEndValue, inputStartTime, inputEndTime, inputEasing);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValueAt_Double_Non0Duration() {
            // Startpoint
            var inputCurrentTime = 0.0f;
            var inputStartValue = 0.0f;
            var inputEndValue = 1.0f;
            var inputStartTime = 0.0f;
            var inputEndTime = 1.0f;
            var inputEasing = Easing.None;
            var expected = inputStartValue;
            var actual = Interpolation.ValueAt(inputCurrentTime, inputStartValue, inputEndValue, inputStartTime, inputEndTime, inputEasing);
            Assert.AreEqual(expected, actual, FloatingPointTolerance);
            // Midpoint
            inputCurrentTime = 0.5f;
            expected = 0.5f;
            actual = Interpolation.ValueAt(inputCurrentTime, inputStartValue, inputEndValue, inputStartTime, inputEndTime, inputEasing);
            Assert.AreEqual(expected, actual, FloatingPointTolerance);
            // Endpoint
            inputCurrentTime = 1.0f;
            expected = inputEndValue;
            actual = Interpolation.ValueAt(inputCurrentTime, inputStartValue, inputEndValue, inputStartTime, inputEndTime, inputEasing);
            Assert.AreEqual(expected, actual, FloatingPointTolerance);
        }

        [Test]
        public void ValueAt_Color_Non0Duration() {
            // Startpoint
            var inputCurrentTime = 0.0f;
            var inputStartValue = Color4.White;
            var inputEndValue = Color4.Black;
            var inputStartTime = 0.0f;
            var inputEndTime = 1.0f;
            var inputEasing = Easing.None;
            var expected = inputStartValue;
            var actual = Interpolation.ValueAt(inputCurrentTime, inputStartValue, inputEndValue, inputStartTime, inputEndTime, inputEasing);
            Assert.AreEqual(expected, actual);
            // Midpoint
            inputCurrentTime = 0.5f;
            expected = new Color4(0.5f, 0.5f, 0.5f, 1);
            actual = Interpolation.ValueAt(inputCurrentTime, inputStartValue, inputEndValue, inputStartTime, inputEndTime, inputEasing);
            Assert.AreEqual(expected, actual);
            // Endpoint
            inputCurrentTime = 1.0f;
            expected = inputEndValue;
            actual = Interpolation.ValueAt(inputCurrentTime, inputStartValue, inputEndValue, inputStartTime, inputEndTime, inputEasing);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValueAt_Vector2_Non0Duration() {
            // Startpoint
            var inputCurrentTime = 0.0f;
            var inputStartValue = Vector2.Zero;
            var inputEndValue = Vector2.One;
            var inputStartTime = 0.0f;
            var inputEndTime = 1.0f;
            var inputEasing = Easing.None;
            var expected = inputStartValue;
            var actual = Interpolation.ValueAt(inputCurrentTime, inputStartValue, inputEndValue, inputStartTime, inputEndTime, inputEasing);
            Assert.AreEqual(expected, actual);
            // Midpoint
            inputCurrentTime = 0.5f;
            expected = Vector2.Divide(inputEndValue, 2);
            actual = Interpolation.ValueAt(inputCurrentTime, inputStartValue, inputEndValue, inputStartTime, inputEndTime, inputEasing);
            Assert.AreEqual(expected, actual);
            // Endpoint
            inputCurrentTime = 1.0f;
            expected = inputEndValue;
            actual = Interpolation.ValueAt(inputCurrentTime, inputStartValue, inputEndValue, inputStartTime, inputEndTime, inputEasing);
            Assert.AreEqual(expected, actual);
        }

    }
}
