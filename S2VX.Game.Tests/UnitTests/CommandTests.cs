using NUnit.Framework;
using osu.Framework.Graphics;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Story.Command;
using System;

namespace S2VX.Game.Tests.UnitTests {
    [TestFixture]
    public class CommandTests {

        private const double FloatingPointTolerance = 0.001;

        [Test]
        public void FromString_DoubleCommand() {
            var input = "ApproachesDistance|0.1|0.1|0.2|1.0|None";
            var expected = new ApproachesDistanceCommand() {
                StartTime = 0.1f,
                EndTime = 0.2f,
                Easing = Enum.Parse<Easing>("None"),
                StartValue = 0.1f,
                EndValue = 1.0f,
            };
            var actual = (ApproachesDistanceCommand)S2VXCommand.FromString(input);
            Assert.AreEqual(expected.StartTime, actual.StartTime, FloatingPointTolerance);
            Assert.AreEqual(expected.EndTime, actual.EndTime, FloatingPointTolerance);
            Assert.AreEqual(expected.Easing, actual.Easing);
            Assert.AreEqual(expected.StartValue, actual.StartValue, FloatingPointTolerance);
            Assert.AreEqual(expected.EndValue, actual.EndValue, FloatingPointTolerance);
        }

        [Test]
        public void FromString_ColorCommand() {
            var input = "BackgroundColor|0.1|(0,0,0)|0.2|(1,1,1)|None";
            var expected = new BackgroundColorCommand() {
                StartTime = 0.1f,
                EndTime = 0.2f,
                Easing = Enum.Parse<Easing>("None"),
                StartValue = Color4.Black,
                EndValue = Color4.White,
            };
            var actual = (BackgroundColorCommand)S2VXCommand.FromString(input);
            Assert.AreEqual(expected.StartTime, actual.StartTime, FloatingPointTolerance);
            Assert.AreEqual(expected.EndTime, actual.EndTime, FloatingPointTolerance);
            Assert.AreEqual(expected.Easing, actual.Easing);
            Assert.AreEqual(expected.StartValue, actual.StartValue);
            Assert.AreEqual(expected.EndValue, actual.EndValue);
        }

        [Test]
        public void FromString_Vector2Command() {
            var input = "CameraMove|0.1|(0,0)|0.2|(1,1)|None";
            var expected = new CameraMoveCommand() {
                StartTime = 0.1f,
                EndTime = 0.2f,
                Easing = Enum.Parse<Easing>("None"),
                StartValue = Vector2.Zero,
                EndValue = Vector2.One,
            };
            var actual = (CameraMoveCommand)S2VXCommand.FromString(input);
            Assert.AreEqual(expected.StartTime, actual.StartTime, FloatingPointTolerance);
            Assert.AreEqual(expected.EndTime, actual.EndTime, FloatingPointTolerance);
            Assert.AreEqual(expected.Easing, actual.Easing);
            Assert.AreEqual(expected.StartValue, actual.StartValue);
            Assert.AreEqual(expected.EndValue, actual.EndValue);
        }

    }
}
