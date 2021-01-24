using NUnit.Framework;
using S2VX.Game.Story;
using System;

namespace StoryMerge.Tests {
    public static class NotesMergerTests {
        public class MergeNotes_MultipleNotes {
            private S2VXStory OutputStory;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var loader = new InputsLoader(new[] {
                    "Samples/NoteAt0.s2ry",
                    "Samples/HoldNoteFrom500To1500.s2ry"
                });
                loader.Load();
                OutputStory = new S2VXStory();
                var merger = new NotesMerger(loader.LoadedStories, OutputStory);
                Result = merger.Merge();
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void Has2Notes() =>
                Assert.AreEqual(2, OutputStory.Notes.Children.Count);

            [Test]
            public void Has1RegularNote() =>
                Assert.AreEqual(1, OutputStory.Notes.GetNonHoldNotes().Count);

            [Test]
            public void Has1HoldNote() =>
                Assert.AreEqual(1, OutputStory.Notes.GetHoldNotes().Count);

            [Test]
            public void HasNoErrorMessage() =>
                Assert.AreEqual("No note conflicts found", Result.Message);
        }

        public class MergeNotes_NotesAtSameTime {
            private S2VXStory OutputStory;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var loader = new InputsLoader(new[] {
                    "Samples/NoteAt0.s2ry",
                    "Samples/NoteAt0.s2ry",
                    "Samples/HoldNoteFrom0To1000.s2ry",
                    "Samples/HoldNoteFrom0To1000.s2ry",
                });
                loader.Load();
                OutputStory = new S2VXStory();
                var merger = new NotesMerger(loader.LoadedStories, OutputStory);
                Result = merger.Merge();
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void Has4Notes() =>
                Assert.AreEqual(4, OutputStory.Notes.Children.Count);

            [Test]
            public void Has2RegularNote() =>
                Assert.AreEqual(2, OutputStory.Notes.GetNonHoldNotes().Count);

            [Test]
            public void Has2HoldNote() =>
                Assert.AreEqual(2, OutputStory.Notes.GetHoldNotes().Count);

            [Test]
            public void HasConflictAt0() =>
                Assert.IsTrue(Result.Message.Contains("Note conflict:\nNote at 0\nNote at 0", StringComparison.Ordinal));

            [Test]
            public void HasConflictFrom0To1000() =>
                Assert.IsTrue(Result.Message.Contains("Note conflict:\nHoldNote from 0 to 1000\nHoldNote from 0 to 1000", StringComparison.Ordinal));
        }

        public class MergeNotes_NotesThatShareTime {
            private S2VXStory OutputStory;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var loader = new InputsLoader(new[] {
                    "Samples/NoteAt0.s2ry",
                    "Samples/HoldNoteFrom0To1000.s2ry",
                });
                loader.Load();
                OutputStory = new S2VXStory();
                var merger = new NotesMerger(loader.LoadedStories, OutputStory);
                Result = merger.Merge();
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void Has4Notes() =>
                Assert.AreEqual(2, OutputStory.Notes.Children.Count);

            [Test]
            public void Has1RegularNote() =>
                Assert.AreEqual(1, OutputStory.Notes.GetNonHoldNotes().Count);

            [Test]
            public void Has1HoldNote() =>
                Assert.AreEqual(1, OutputStory.Notes.GetHoldNotes().Count);

            [Test]
            public void HasConflictAt0() =>
                Assert.IsTrue(Result.Message.Contains("Note conflict:\nNote at 0\nHoldNote from 0 to 1000", StringComparison.Ordinal));
        }

        public class MergeNotes_OverlappingHoldNotes {
            private S2VXStory OutputStory;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var loader = new InputsLoader(new[] {
                    "Samples/HoldNoteFrom0To1000.s2ry",
                    "Samples/HoldNoteFrom500To1500.s2ry",
                });
                loader.Load();
                OutputStory = new S2VXStory();
                var merger = new NotesMerger(loader.LoadedStories, OutputStory);
                Result = merger.Merge();
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void Has2Notes() =>
                Assert.AreEqual(2, OutputStory.Notes.Children.Count);

            [Test]
            public void Has2HoldNote() =>
                Assert.AreEqual(2, OutputStory.Notes.GetHoldNotes().Count);

            [Test]
            public void HasConflictFrom500To1000() =>
                Assert.IsTrue(Result.Message.Contains("Note conflict:\nHoldNote from 0 to 1000\nHoldNote from 500 to 1500", StringComparison.Ordinal));
        }
    }
}
