using NUnit.Framework;
using osuTK;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;
using System;

namespace StoryMerge.Tests {
    [TestFixture]
    public class StoryMergerTests {
        [Test]
        public void ValidateParameters_WithValidParameters_ItReturnsSuccess() {
            var merger = new StoryMerger(new[] {
                "NotesAlphaFrom0To0.s2ry",
                "NotesAlphaFrom0To1000.s2ry"
            }, "output.s2ry");
            var result = merger.ValidateParameters();
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsEmpty(result.Message);
        }

        [Test]
        public void ValidateParameters_WithNoInputs_ItReturnsError() {
            var merger = new StoryMerger(null, "output.s2ry");
            var result = merger.ValidateParameters();
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("2 or more inputs must be provided", result.Message);
        }

        [Test]
        public void ValidateParameters_WithOneInput_ItReturnsError() {
            var merger = new StoryMerger(new[] { "input1.s2ry" }, "output.s2ry");
            var result = merger.ValidateParameters();
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("2 or more inputs must be provided", result.Message);
        }

        [Test]
        public void ValidateParameters_WithNoOutput_ItReturnsError() {
            var merger = new StoryMerger(new[] {
                "input1.s2ry",
                "input2.s2ry"
            }, null);
            var result = merger.ValidateParameters();
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("1 output must be provided", result.Message);
        }

        [Test]
        public void ValidateParameters_WithNonexistentInput_ItReturnsError() {
            var merger = new StoryMerger(new[] {
                "NotesAlphaFrom0To0.s2ry",
                "NonexistentFile.s2ry"
            }, "output.s2ry");
            var result = merger.ValidateParameters();
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("Input file does not exist: NonexistentFile.s2ry", result.Message);
        }

        [Test]
        public void LoadInputs_WithValidStories_ItReturnsSuccess() {
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
            var result = merger.LoadInputs();
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsEmpty(result.Message);
        }

        [Test]
        public void LoadInputs_WithInvalidStory_ItReturnsError() {
            var merger = new StoryMerger(new[] {
                "NotesAlphaFrom0To0.s2ry",
                "InvalidStory.s2ry"
            }, "output.s2ry");
            var result = merger.LoadInputs();
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.Message.Contains("Failed to load: InvalidStory.s2ry", StringComparison.Ordinal));
        }

        [Test]
        public void CopyNote_WithNote_ItReturnsNewNote() {
            var note = new EditorNote {
                HitTime = 1000,
                Coordinates = new Vector2(2, 2)
            };
            var copied = StoryMerger.CopyNote(note);
            Assert.AreEqual(note.HitTime, copied.HitTime);
            Assert.AreEqual(note.Coordinates, copied.Coordinates);
        }

        [Test]
        public void CopyHoldNote_WithHoldNote_ItReturnsNewHoldNote() {
            var holdNote = new EditorHoldNote {
                HitTime = 1000,
                EndTime = 2000,
                Coordinates = new Vector2(2, 2),
                EndCoordinates = new Vector2(4, 4)
            };
            var copied = StoryMerger.CopyHoldNote(holdNote);
            Assert.AreEqual(holdNote.HitTime, copied.HitTime);
            Assert.AreEqual(holdNote.EndTime, copied.EndTime);
            Assert.AreEqual(holdNote.Coordinates, copied.Coordinates);
            Assert.AreEqual(holdNote.EndCoordinates, copied.EndCoordinates);
        }

        [Test]
        public void MergeNotes_WithMultipleNotes_ItAddsAllIntoStory() {
            var merger = new StoryMerger(new[] {
                "NoteAt0.s2ry",
                "HoldNoteFrom0To1000.s2ry"
            }, "output.s2ry");
            merger.LoadInputs();
            var story = new S2VXStory();
            var result = merger.MergeNotes(story);
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(2, story.Notes.Children.Count);
            Assert.AreEqual(1, story.Notes.GetHoldNotes().Count);
            Assert.AreEqual(1, story.Notes.GetNonHoldNotes().Count);
            Assert.IsEmpty(result.Message);
        }

        [Test]
        public void MergeNotes_WithNotesAtSameTime_ItShowsConflicts() {
            var merger = new StoryMerger(new[] {
                "NoteAt0.s2ry",
                "NoteAt0.s2ry",
                "HoldNoteFrom0To1000.s2ry",
                "HoldNoteFrom0To1000.s2ry",
            }, "output.s2ry");
            merger.LoadInputs();
            var story = new S2VXStory();
            var result = merger.MergeNotes(story);
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(4, story.Notes.Children.Count);
            Assert.AreEqual(2, story.Notes.GetHoldNotes().Count);
            Assert.AreEqual(2, story.Notes.GetNonHoldNotes().Count);
            Assert.IsTrue(result.Message.Contains("Note conflict:\nNote at 0\nNote at 0", StringComparison.Ordinal));
            Assert.IsTrue(result.Message.Contains("Note conflict:\nHoldNote from 0 to 1000\nHoldNote from 0 to 1000", StringComparison.Ordinal));
        }

        [Test]
        public void MergeNotes_WithOverlappingHoldNotes_ItShowsConflicts() {
            var merger = new StoryMerger(new[] {
                "HoldNoteFrom0To1000.s2ry",
                "HoldNoteFrom500To1500.s2ry",
            }, "output.s2ry");
            merger.LoadInputs();
            var story = new S2VXStory();
            var result = merger.MergeNotes(story);
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(2, story.Notes.Children.Count);
            Assert.AreEqual(2, story.Notes.GetHoldNotes().Count);
            Assert.IsTrue(result.Message.Contains("Note conflict:\nHoldNote from 0 to 1000\nHoldNote from 500 to 1500", StringComparison.Ordinal));
        }

        [Test]
        public void MergeCommands_WithMultipleCommands_ItAddsAllIntoStory() {
            var merger = new StoryMerger(new[] {
                "NotesAlphaFrom0To1000.s2ry",
                "NotesAlphaFrom0To0.s2ry",
                "NotesAlphaFrom1000To1000.s2ry",
            }, "output.s2ry");
            merger.LoadInputs();
            var story = new S2VXStory();
            var result = merger.MergeCommands(story);
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(3, story.Commands.Count);
            Assert.IsEmpty(result.Message);
        }

        [Test]
        public void MergeCommands_WithCommandsAtSameTime_ItShowsConflicts() {
            var merger = new StoryMerger(new[] {
                "NotesAlphaFrom0To0.s2ry",
                "NotesAlphaFrom0To0.s2ry",
                "NotesAlphaFrom0To1000.s2ry",
                "NotesAlphaFrom0To1000.s2ry",
            }, "output.s2ry");
            merger.LoadInputs();
            var story = new S2VXStory();
            var result = merger.MergeCommands(story);
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(4, story.Commands.Count);
            Assert.IsTrue(result.Message.Contains("Command conflict:\nNotesAlpha from 0 to 0\nNotesAlpha from 0 to 0", StringComparison.Ordinal));
            Assert.IsTrue(result.Message.Contains("Command conflict:\nNotesAlpha from 0 to 1000\nNotesAlpha from 0 to 1000", StringComparison.Ordinal));
        }

        [Test]
        public void MergeCommands_WithOverlappingCommands_ItAddsAllIntoStory() {
            var merger = new StoryMerger(new[] {
                "NotesAlphaFrom500To1500.s2ry",
                "NotesAlphaFrom0To1000.s2ry",
            }, "output.s2ry");
            merger.LoadInputs();
            var story = new S2VXStory();
            var result = merger.MergeCommands(story);
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(2, story.Commands.Count);
            Assert.IsTrue(result.Message.Contains("Command conflict:\nNotesAlpha from 0 to 1000\nNotesAlpha from 500 to 1500", StringComparison.Ordinal));
        }

        [Test]
        public void MergeCommands_WithDifferentCommandsAtSameTime_ItShowsNoConflicts() {
            var merger = new StoryMerger(new[] {
                "GridAlphaFrom0To0.s2ry",
                "NotesAlphaFrom0To0.s2ry",
            }, "output.s2ry");
            merger.LoadInputs();
            var story = new S2VXStory();
            var result = merger.MergeCommands(story);
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(2, story.Commands.Count);
            Assert.IsEmpty(result.Message);
        }
    }
}
