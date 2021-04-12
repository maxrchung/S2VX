using NUnit.Framework;
using System;
using System.IO;

[assembly: CLSCompliant(false)]
namespace StoryMerge.Tests {
    public static class StoryMergerTests {
        public class Merge_MultipleNotesAndCommands {
            private Result Result;
            private string ExpectedOutput;
            private string ActualOutput;

            [SetUp]
            public void SetUp() {
                Result = StoryMerger.Merge(new[] {
                    "Samples/NotesAlphaFrom1000To1000.s2ry",
                    "Samples/NotesAlphaFrom0To1000.s2ry",
                    "Samples/NotesAlphaFrom0To0.s2ry",
                    "Samples/GridAlphaFrom0To0.s2ry",
                    "Samples/HoldNoteFrom500To1500.s2ry",
                    "Samples/NoteAt0.s2ry",
                }, "output.s2ry");
                ExpectedOutput = File.ReadAllText("Samples/ExpectedOutput.s2ry");
                ActualOutput = File.ReadAllText("output.s2ry");
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void EqualsExpectedOutput() =>
                Assert.AreEqual(ExpectedOutput, ActualOutput);

            [Test]
            public void Merged6Inputs() =>
                Assert.IsTrue(Result.Message.Contains("Merged 6 stories into \"output.s2ry\"", StringComparison.Ordinal));

            [Test]
            public void HasNoNoteConflicts() =>
                Assert.IsTrue(Result.Message.Contains("No note conflicts found", StringComparison.Ordinal));

            [Test]
            public void HasNoCommandConflicts() =>
                Assert.IsTrue(Result.Message.Contains("No command conflicts found", StringComparison.Ordinal));
        }
    }
}
