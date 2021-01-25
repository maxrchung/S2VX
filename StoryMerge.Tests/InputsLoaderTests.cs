using NUnit.Framework;
using System;

namespace StoryMerge.Tests {
    public static class InputsLoaderTests {
        public class Load_ValidStories {
            private Result Result;

            [SetUp]
            public void SetUp() {
                var (result, _) = InputsLoader.Load(new[] {
                    "Samples/GridAlphaFrom0To0.s2ry",
                    "Samples/HoldNoteFrom0To1000.s2ry",
                    "Samples/HoldNoteFrom500To1500.s2ry",
                    "Samples/NoteAt0.s2ry",
                    "Samples/NotesAlphaFrom0To0.s2ry",
                    "Samples/NotesAlphaFrom0To1000.s2ry",
                    "Samples/NotesAlphaFrom1000To1000.s2ry",
                    "Samples/NotesAlphaFrom500To1500.s2ry",
                });
                Result = result;
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void HasEmptyMessage() =>
                Assert.IsEmpty(Result.Message);
        }

        public class Load_InvalidStory {
            private Result Result;

            [SetUp]
            public void SetUp() {
                var (result, _) = InputsLoader.Load(new[] {
                    "Samples/NotesAlphaFrom0To0.s2ry",
                    "Samples/InvalidStory.s2ry"
                });
                Result = result;
            }

            [Test]
            public void IsNotSuccessful() =>
                Assert.IsFalse(Result.IsSuccessful);

            [Test]
            public void HasErrorMessage() =>
                Assert.True(Result.Message.Contains("Input file failed to load: \"Samples/InvalidStory.s2ry\"", StringComparison.Ordinal));
        }
    }
}
