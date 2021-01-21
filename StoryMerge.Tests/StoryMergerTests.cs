using NUnit.Framework;
using S2VX.Game.Story;
using System;

namespace StoryMerge.Tests {
    [TestFixture]
    public class StoryMergerTests {
        [Test]
        public void ValidateParameters_WithValidParameters_ItReturnsSuccess() {
            var merger = new StoryMerger(new[] { "CommandFrom0To0.s2ry", "CommandFrom0To1000.s2ry" }, "output.s2ry");
            var result = merger.ValidateParameters();
            Assert.AreEqual(true, result.IsSuccessful);
        }

        [Test]
        public void ValidateParameters_WithNoInputs_ItReturnsError() {
            var merger = new StoryMerger(null, "output.s2ry");
            var result = merger.ValidateParameters();
            Assert.AreEqual(false, result.IsSuccessful);
            Assert.AreEqual("2 or more inputs must be provided", result.Message);
        }

        [Test]
        public void ValidateParameters_WithOneInput_ItReturnsError() {
            var merger = new StoryMerger(new[] { "input1.s2ry" }, "output.s2ry");
            var result = merger.ValidateParameters();
            Assert.AreEqual(false, result.IsSuccessful);
            Assert.AreEqual("2 or more inputs must be provided", result.Message);
        }

        [Test]
        public void ValidateParameters_WithNoOutput_ItReturnsError() {
            var merger = new StoryMerger(new[] { "input1.s2ry", "input2.s2ry" }, null);
            var result = merger.ValidateParameters();
            Assert.AreEqual(false, result.IsSuccessful);
            Assert.AreEqual("1 output must be provided", result.Message);
        }

        [Test]
        public void ValidateParameters_WithNonexistentInput_ItReturnsError() {
            var merger = new StoryMerger(new[] { "CommandFrom0To0.s2ry", "NonexistentFile.s2ry" }, "output.s2ry");
            var result = merger.ValidateParameters();
            Assert.AreEqual(false, result.IsSuccessful);
            Assert.AreEqual("Input file does not exist: NonexistentFile.s2ry", result.Message);
        }

        [Test]
        public void ValidateInputs_WithValidStories_ItReturnsSuccess() {
            var merger = new StoryMerger(new[] {
                "CommandFrom0To0.s2ry",
                "CommandFrom0To1000.s2ry",
                "CommandFrom1000To1000.s2ry",
                "CommandFrom500To1500.s2ry",
                "CommandFrom500To500.s2ry",
                "HoldNoteFrom0To1000.s2ry",
                "NoteAt0.s2ry",
            }, "output.s2ry");
            var result = merger.ValidateInputs();
            Assert.AreEqual(true, result.IsSuccessful);
        }

        [Test]
        public void ValidateInputs_WithInvalidStory_ItReturnsError() {
            var merger = new StoryMerger(new[] { "CommandFrom0To0.s2ry", "InvalidStory.s2ry" }, "output.s2ry");
            var result = merger.ValidateInputs();
            Assert.AreEqual(false, result.IsSuccessful);
            Assert.AreEqual(true, result.Message.Contains("Failed to load: InvalidStory.s2ry", StringComparison.Ordinal));
        }

        [Test]
        public void MergeNotes_WithMultipleNotes_ItAddsAllIntoStory() {
            var merger = new StoryMerger(new[] { "NoteAt0.s2ry", "HoldNoteFrom0To1000.s2ry" }, "output.s2ry");
            var story = new S2VXStory();
            var result = merger.MergeNotes(story);
            Assert.AreEqual(true, result.IsSuccessful);
            Assert.AreEqual(2, story.Notes.Children.Count);
            Assert.AreEqual(1, story.Notes.GetNonHoldNotes().Count);
        }

        [Test]
        public void MergeNotes_WithNotesAtSameTime_ItAddsAllIntoStory() {
            var merger = new StoryMerger(new[] {
                "NoteAt0.s2ry",
                "NoteAt0.s2ry",
                "HoldNoteFrom0To1000.s2ry",
                "HoldNoteFrom0To1000.s2ry",
            }, "output.s2ry");
            var story = new S2VXStory();
            var result = merger.MergeNotes(story);
            Assert.AreEqual(true, result.IsSuccessful);
            Assert.AreEqual(4, story.Notes.Children.Count);
        }
    }
}
