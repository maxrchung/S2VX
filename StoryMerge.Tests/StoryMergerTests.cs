using NUnit.Framework;
using osuTK;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;
using System;

namespace StoryMerge.Tests {
    public static class StoryMergerTests {
        public class ValidateParameters_WithValidParameters {
            private Result Result;

            [SetUp]
            public void SetUp() {
                var merger = new StoryMerger(new[] {
                    "NotesAlphaFrom0To0.s2ry",
                    "NotesAlphaFrom0To1000.s2ry"
                }, "output.s2ry");
                Result = merger.ValidateParameters();
            }

            [Test]
            public void ItIsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void ItHasEmptyMessage() =>
                Assert.IsEmpty(Result.Message);
        }

        public class ValidateParameters_WithNoInputs {
            private Result Result;

            [SetUp]
            public void SetUp() {
                var merger = new StoryMerger(null, "output.s2ry");
                Result = merger.ValidateParameters();
            }

            [Test]
            public void ItIsNotSuccessful() =>
                Assert.IsFalse(Result.IsSuccessful);

            [Test]
            public void ItHasErrorMessage() =>
                Assert.AreEqual("2 or more inputs must be provided", Result.Message);
        }

        public class ValidateParameters_WithOneInput {
            private Result Result;

            [SetUp]
            public void SetUp() {
                var merger = new StoryMerger(new[] { "input1.s2ry" }, "output.s2ry");
                Result = merger.ValidateParameters();
            }

            [Test]
            public void ItIsNotSuccessful() =>
                Assert.IsFalse(Result.IsSuccessful);

            [Test]
            public void ItHasErrorMessage() =>
                Assert.AreEqual("2 or more inputs must be provided", Result.Message);
        }

        public class ValidateParameters_WithNoOutput {
            private Result Result;

            [SetUp]
            public void SetUp() {
                var merger = new StoryMerger(new[] {
                    "input1.s2ry",
                    "input2.s2ry"
                }, null);
                Result = merger.ValidateParameters();
            }

            [Test]
            public void ItIsNotSuccessful() =>
                Assert.IsFalse(Result.IsSuccessful);

            [Test]
            public void ItHasErrorMessage() =>
                Assert.AreEqual("1 output must be provided", Result.Message);
        }

        public class ValidateParameters_WithNonexistentInput {
            private Result Result;

            [SetUp]
            public void SetUp() {
                var merger = new StoryMerger(new[] {
                    "NotesAlphaFrom0To0.s2ry",
                    "NonexistentFile.s2ry"
                }, "output.s2ry");
                Result = merger.ValidateParameters();
            }

            [Test]
            public void ItIsNotSuccessful() =>
                Assert.IsFalse(Result.IsSuccessful);

            [Test]
            public void ItHasErrorMessage() =>
                Assert.AreEqual("Input file does not exist: NonexistentFile.s2ry", Result.Message);
        }

        public class LoadInputs_WithValidStories {
            private Result Result;

            [SetUp]
            public void SetUp() {
                var merger = new StoryMerger(new[] {
                    "GridAlphaFrom0To0.s2ry",
                    "HoldNoteFrom0To1000.s2ry",
                    "HoldNoteFrom500To1500.s2ry",
                    "NoteAt0.s2ry",
                    "NotesAlphaFrom0To0.s2ry",
                    "NotesAlphaFrom0To1000.s2ry",
                    "NotesAlphaFrom1000To1000.s2ry",
                    "NotesAlphaFrom500To1500.s2ry",
                }, "output.s2ry");
                Result = merger.LoadInputs();
            }

            [Test]
            public void ItIsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void ItHasEmptyMessage() =>
                Assert.IsEmpty(Result.Message);
        }

        public class LoadInputs_WithInvalidStory {
            private Result Result;

            [SetUp]
            public void SetUp() {
                var merger = new StoryMerger(new[] {
                    "NotesAlphaFrom0To0.s2ry",
                    "InvalidStory.s2ry"
                }, "output.s2ry");
                Result = merger.LoadInputs();
            }

            [Test]
            public void ItIsNotSuccessful() =>
                Assert.IsFalse(Result.IsSuccessful);

            [Test]
            public void ItHasErrorMessage() =>
                Assert.True(Result.Message.Contains("Failed to load: InvalidStory.s2ry", StringComparison.Ordinal));
        }

        public class CopyNote_WithNote {
            private S2VXNote Original;
            private S2VXNote Copied;

            [SetUp]
            public void SetUp() {
                Original = new EditorNote {
                    HitTime = 1000,
                    Coordinates = new Vector2(2, 2)
                };
                Copied = StoryMerger.CopyNote(Original);
            }

            [Test]
            public void ItHasTheSameHitTime() =>
                Assert.AreEqual(Original.HitTime, Copied.HitTime);

            [Test]
            public void ItHasTheSameCoordinates() =>
                Assert.AreEqual(Original.Coordinates, Copied.Coordinates);
        }

        public class CopyHoldNote_WithHoldNote {
            private HoldNote Original;
            private HoldNote Copied;

            [SetUp]
            public void SetUp() {
                Original = new EditorHoldNote {
                    HitTime = 1000,
                    EndTime = 2000,
                    Coordinates = new Vector2(2, 2),
                    EndCoordinates = new Vector2(4, 4)
                };
                Copied = StoryMerger.CopyHoldNote(Original);
            }

            [Test]
            public void ItHasTheSameHitTime() =>
                Assert.AreEqual(Original.HitTime, Copied.HitTime);

            [Test]
            public void ItHasTheSameEndTime() =>
                Assert.AreEqual(Original.EndTime, Copied.EndTime);

            [Test]
            public void ItHasTheSameCoordinates() =>
                Assert.AreEqual(Original.Coordinates, Copied.Coordinates);

            [Test]
            public void ItHasTheSameEndCoordinates() =>
                Assert.AreEqual(Original.EndCoordinates, Copied.EndCoordinates);
        }

        public class MergeNotes_WithMultipleNotes {
            private S2VXStory Story;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var merger = new StoryMerger(new[] {
                    "NoteAt0.s2ry",
                    "HoldNoteFrom0To1000.s2ry"
                }, "output.s2ry");
                merger.LoadInputs();
                Story = new S2VXStory();
                Result = merger.MergeNotes(Story);
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void ItHas2Notes() =>
                Assert.AreEqual(2, Story.Notes.Children.Count);

            [Test]
            public void ItHas1RegularNote() =>
                Assert.AreEqual(1, Story.Notes.GetNonHoldNotes().Count);

            [Test]
            public void ItHas1HoldNote() =>
                Assert.AreEqual(1, Story.Notes.GetHoldNotes().Count);

            [Test]
            public void ItHasEmptyMessage() =>
                Assert.IsEmpty(Result.Message);
        }

        public class MergeNotes_WithNotesAtSameTime {
            private S2VXStory Story;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var merger = new StoryMerger(new[] {
                    "NoteAt0.s2ry",
                    "NoteAt0.s2ry",
                    "HoldNoteFrom0To1000.s2ry",
                    "HoldNoteFrom0To1000.s2ry",
                }, "output.s2ry");
                merger.LoadInputs();
                Story = new S2VXStory();
                Result = merger.MergeNotes(Story);
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void ItHas4Notes() =>
                Assert.AreEqual(4, Story.Notes.Children.Count);

            [Test]
            public void ItHas2RegularNote() =>
                Assert.AreEqual(2, Story.Notes.GetNonHoldNotes().Count);

            [Test]
            public void ItHas2HoldNote() =>
                Assert.AreEqual(2, Story.Notes.GetHoldNotes().Count);

            [Test]
            public void ItHasConflictAt0() =>
                Assert.IsTrue(Result.Message.Contains("Note conflict:\nNote at 0\nNote at 0", StringComparison.Ordinal));

            [Test]
            public void ItHasConflictFrom0To1000() =>
                Assert.IsTrue(Result.Message.Contains("Note conflict:\nHoldNote from 0 to 1000\nHoldNote from 0 to 1000", StringComparison.Ordinal));
        }

        public class MergeNotes_WithOverlappingHoldNotes {
            private S2VXStory Story;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var merger = new StoryMerger(new[] {
                    "HoldNoteFrom0To1000.s2ry",
                    "HoldNoteFrom500To1500.s2ry",
                }, "output.s2ry");
                merger.LoadInputs();
                Story = new S2VXStory();
                Result = merger.MergeNotes(Story);
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void ItHas2Notes() =>
                Assert.AreEqual(2, Story.Notes.Children.Count);

            [Test]
            public void ItHas2HoldNote() =>
                Assert.AreEqual(2, Story.Notes.GetHoldNotes().Count);

            [Test]
            public void ItHasConflictFrom500To1000() =>
                Assert.IsTrue(Result.Message.Contains("Note conflict:\nHoldNote from 0 to 1000\nHoldNote from 500 to 1500", StringComparison.Ordinal));
        }

        public class MergeCommands_WithMultipleCommands {
            private S2VXStory Story;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var merger = new StoryMerger(new[] {
                    "NotesAlphaFrom0To1000.s2ry",
                    "NotesAlphaFrom0To0.s2ry",
                    "NotesAlphaFrom1000To1000.s2ry",
                }, "output.s2ry");
                merger.LoadInputs();
                Story = new S2VXStory();
                Result = merger.MergeCommands(Story);
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void ItHas3Commands() =>
                Assert.AreEqual(3, Story.Commands.Count);

            [Test]
            public void HasEmptyMessage() =>
                Assert.IsEmpty(Result.Message);
        }

        public class MergeCommands_WithCommandsAtSameTime {
            private S2VXStory Story;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var merger = new StoryMerger(new[] {
                    "NotesAlphaFrom0To0.s2ry",
                    "NotesAlphaFrom0To0.s2ry",
                    "NotesAlphaFrom0To1000.s2ry",
                    "NotesAlphaFrom0To1000.s2ry",
                }, "output.s2ry");
                merger.LoadInputs();
                Story = new S2VXStory();
                Result = merger.MergeCommands(Story);
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void ItHas4Commands() =>
                Assert.AreEqual(4, Story.Commands.Count);

            [Test]
            public void ItHasConflictAt0() =>
                Assert.IsTrue(Result.Message.Contains("Command conflict:\nNotesAlpha from 0 to 0\nNotesAlpha from 0 to 0", StringComparison.Ordinal));

            [Test]
            public void ItHasConflictFrom0To1000() =>
                Assert.IsTrue(Result.Message.Contains("Command conflict:\nNotesAlpha from 0 to 1000\nNotesAlpha from 0 to 1000", StringComparison.Ordinal));
        }

        public class MergeCommands_WithOverlappingCommands {
            private S2VXStory Story;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var merger = new StoryMerger(new[] {
                    "NotesAlphaFrom500To1500.s2ry",
                    "NotesAlphaFrom0To1000.s2ry",
                }, "output.s2ry");
                merger.LoadInputs();
                Story = new S2VXStory();
                Result = merger.MergeCommands(Story);
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void ItHas2Commands() =>
                Assert.AreEqual(2, Story.Commands.Count);

            [Test]
            public void ItHasConflictFrom500To1000() =>
                Assert.IsTrue(Result.Message.Contains("Command conflict:\nNotesAlpha from 0 to 1000\nNotesAlpha from 500 to 1500", StringComparison.Ordinal));
        }

        public class MergeCommands_WithDifferentCommandsAtSameTime {
            private S2VXStory Story;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var merger = new StoryMerger(new[] {
                    "GridAlphaFrom0To0.s2ry",
                    "NotesAlphaFrom0To0.s2ry",
                }, "output.s2ry");
                merger.LoadInputs();
                Story = new S2VXStory();
                Result = merger.MergeCommands(Story);
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void ItHas2Commands() =>
                Assert.AreEqual(2, Story.Commands.Count);

            [Test]
            public void ItHasEmptyMessage() =>
                Assert.IsEmpty(Result.Message);
        }
    }
}
