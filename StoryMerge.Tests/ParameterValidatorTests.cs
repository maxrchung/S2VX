using NUnit.Framework;

namespace StoryMerge.Tests {
    public static class ParameterValidatorTests {
        public class Validate_ValidParameters {
            private Result Result;

            [SetUp]
            public void SetUp() {
                var validator = new ParameterValidator(new[] {
                    "Samples/NotesAlphaFrom0To0.s2ry",
                    "Samples/NotesAlphaFrom0To1000.s2ry"
                }, "output.s2ry");
                Result = validator.Validate();
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void HasEmptyMessage() =>
                Assert.IsEmpty(Result.Message);
        }

        public class Validate_NoInputs {
            private Result Result;

            [SetUp]
            public void SetUp() {
                var validator = new ParameterValidator(null, "output.s2ry");
                Result = validator.Validate();
            }

            [Test]
            public void IsNotSuccessful() =>
                Assert.IsFalse(Result.IsSuccessful);

            [Test]
            public void HasErrorMessage() =>
                Assert.AreEqual("2 or more inputs must be provided", Result.Message);
        }

        public class Validate_OneInput {
            private Result Result;

            [SetUp]
            public void SetUp() {
                var validator = new ParameterValidator(new[] {
                    "Samples/input1.s2ry"
                }, "output.s2ry");
                Result = validator.Validate();
            }

            [Test]
            public void IsNotSuccessful() =>
                Assert.IsFalse(Result.IsSuccessful);

            [Test]
            public void HasErrorMessage() =>
                Assert.AreEqual("2 or more inputs must be provided", Result.Message);
        }

        public class Validate_NoOutput {
            private Result Result;

            [SetUp]
            public void SetUp() {
                var validator = new ParameterValidator(new[] {
                    "Samples/input1.s2ry",
                    "Samples/input2.s2ry"
                }, null);
                Result = validator.Validate();
            }

            [Test]
            public void IsNotSuccessful() =>
                Assert.IsFalse(Result.IsSuccessful);

            [Test]
            public void HasErrorMessage() =>
                Assert.AreEqual("1 output must be provided", Result.Message);
        }

        public class Validate_NonexistentInput {
            private Result Result;

            [SetUp]
            public void SetUp() {
                var validator = new ParameterValidator(new[] {
                    "Samples/NotesAlphaFrom0To0.s2ry",
                    "Samples/NonexistentFile.s2ry"
                }, "output.s2ry");
                Result = validator.Validate();
            }

            [Test]
            public void IsNotSuccessful() =>
                Assert.IsFalse(Result.IsSuccessful);

            [Test]
            public void HasErrorMessage() =>
                Assert.AreEqual("Input file does not exist: \"Samples/NonexistentFile.s2ry\"", Result.Message);
        }
    }
}
