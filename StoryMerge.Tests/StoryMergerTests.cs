using NUnit.Framework;
using System;

namespace StoryMerge.Tests {
    [TestFixture]
    public class StoryMergerTests {
        [Test]
        public void ValidateParameters_WithValidParameters_ReturnsSuccess() {
            var merger = new StoryMerger(new[] { "CommandFrom0To0.s2ry", "CommandFrom0To1000.s2ry" }, "output.s2ry");
            var result = merger.ValidateParameters();
            Assert.AreEqual(true, result.IsSuccessful);
        }


        [Test]
        public void ValidateParameters_WithNoInputs_ReturnsError() {
            var merger = new StoryMerger(null, "output.s2ry");
            var result = merger.ValidateParameters();
            Assert.AreEqual(false, result.IsSuccessful);
            Assert.AreEqual("2 or more inputs must be provided", result.Message);
        }

        [Test]
        public void ValidateParameters_WithOneInput_ReturnsError() {
            var merger = new StoryMerger(new[] { "input1.s2ry" }, "output.s2ry");
            var result = merger.ValidateParameters();
            Assert.AreEqual(false, result.IsSuccessful);
            Assert.AreEqual("2 or more inputs must be provided", result.Message);
        }

        [Test]
        public void ValidateParameters_WithNoOutput_ReturnsError() {
            var merger = new StoryMerger(new[] { "input1.s2ry", "input2.s2ry" }, null);
            var result = merger.ValidateParameters();
            Assert.AreEqual(false, result.IsSuccessful);
            Assert.AreEqual("1 output must be provided", result.Message);
        }

        [Test]
        public void ValidateParameters_WithNonexistentInput_ReturnsError() {
            var merger = new StoryMerger(new[] { "CommandFrom0To0.s2ry", "NonexistentFile.s2ry" }, "output.s2ry");
            var result = merger.ValidateParameters();
            Assert.AreEqual(false, result.IsSuccessful);
            Assert.AreEqual("Input file does not exist: NonexistentFile.s2ry", result.Message);
        }

        [Test]
        public void ValidateInputs_WithValidStories_ReturnsSuccess() {
            var merger = new StoryMerger(new[] {
                "CommandFrom0To0.s2ry",
                "CommandFrom0To1000.s2ry",
                "CommandFrom1000To1000.s2ry",
                "CommandFrom500To1500.s2ry",
                "CommandFrom500To500.s2ry",
                "HoldNoteFrom1000To2000.s2ry",
                "HoldNoteFrom0To1000.s2ry",
                "NoteAt0.s2ry",
                "NoteAt500.s2ry"
            }, "output.s2ry");
            var result = merger.ValidateInputs();
            Assert.AreEqual(true, result.IsSuccessful);
        }

        [Test]
        public void ValidateInputs_WithInvalidStory_ReturnsError() {
            var merger = new StoryMerger(new[] { "CommandFrom0To0.s2ry", "InvalidStory.s2ry" }, "output.s2ry");
            var result = merger.ValidateInputs();
            Assert.AreEqual(false, result.IsSuccessful);
            Assert.AreEqual(true, result.Message.Contains("Failed to load: InvalidStory.s2ry", StringComparison.Ordinal));
        }


    }
}
