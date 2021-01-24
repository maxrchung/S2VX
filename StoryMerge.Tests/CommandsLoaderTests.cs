using NUnit.Framework;
using S2VX.Game.Story;
using System;


namespace StoryMerge.Tests {
    public static class CommandsLoaderTests {
        public class MergeCommands_MultipleCommands {
            private S2VXStory OutputStory;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var loader = new InputsLoader(new[] {
                    "Samples/NotesAlphaFrom0To1000.s2ry",
                    "Samples/NotesAlphaFrom0To0.s2ry",
                    "Samples/NotesAlphaFrom1000To1000.s2ry",
                });
                loader.Load();
                OutputStory = new S2VXStory();
                var merger = new CommandsMerger(loader.LoadedStories, OutputStory);
                Result = merger.Merge();
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void Has3Commands() =>
                Assert.AreEqual(3, OutputStory.Commands.Count);

            [Test]
            public void HasNoErrorMessage() =>
                Assert.AreEqual("No command conflicts found", Result.Message);
        }

        public class MergeCommands_CommandsAtSameTime {
            private S2VXStory OutputStory;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var loader = new InputsLoader(new[] {
                    "Samples/NotesAlphaFrom0To0.s2ry",
                    "Samples/NotesAlphaFrom0To0.s2ry",
                    "Samples/NotesAlphaFrom0To1000.s2ry",
                    "Samples/NotesAlphaFrom0To1000.s2ry",
                });
                loader.Load();
                OutputStory = new S2VXStory();
                var merger = new CommandsMerger(loader.LoadedStories, OutputStory);
                Result = merger.Merge();
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void Has4Commands() =>
                Assert.AreEqual(4, OutputStory.Commands.Count);

            [Test]
            public void HasConflictAt0() =>
                Assert.IsTrue(Result.Message.Contains("Command conflict:\nNotesAlpha from 0 to 0\nNotesAlpha from 0 to 0", StringComparison.Ordinal));

            [Test]
            public void HasConflictFrom0To1000() =>
                Assert.IsTrue(Result.Message.Contains("Command conflict:\nNotesAlpha from 0 to 1000\nNotesAlpha from 0 to 1000", StringComparison.Ordinal));
        }

        public class MergeCommands_CommandsThatShareTime {
            private S2VXStory OutputStory;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var loader = new InputsLoader(new[] {
                    "Samples/NotesAlphaFrom0To0.s2ry",
                    "Samples/NotesAlphaFrom0To1000.s2ry",
                });
                loader.Load();
                OutputStory = new S2VXStory();
                var merger = new CommandsMerger(loader.LoadedStories, OutputStory);
                Result = merger.Merge();
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void Has2Commands() =>
                Assert.AreEqual(2, OutputStory.Commands.Count);

            [Test]
            public void HasNoErrorMessage() =>
                Assert.AreEqual("No command conflicts found", Result.Message);
        }

        public class MergeCommands_OverlappingCommands {
            private S2VXStory OutputStory;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var loader = new InputsLoader(new[] {
                    "Samples/NotesAlphaFrom500To1500.s2ry",
                    "Samples/NotesAlphaFrom0To1000.s2ry",
                });
                loader.Load();
                OutputStory = new S2VXStory();
                var merger = new CommandsMerger(loader.LoadedStories, OutputStory);
                Result = merger.Merge();
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void Has2Commands() =>
                Assert.AreEqual(2, OutputStory.Commands.Count);

            [Test]
            public void HasConflictFrom500To1000() =>
                Assert.IsTrue(Result.Message.Contains("Command conflict:\nNotesAlpha from 0 to 1000\nNotesAlpha from 500 to 1500", StringComparison.Ordinal));
        }

        public class MergeCommands_DifferentCommandsAtSameTime {
            private S2VXStory OutputStory;
            private Result Result;

            [SetUp]
            public void SetUp() {
                var loader = new InputsLoader(new[] {
                    "Samples/GridAlphaFrom0To0.s2ry",
                    "Samples/NotesAlphaFrom0To0.s2ry",
                });
                loader.Load();
                OutputStory = new S2VXStory();
                var merger = new CommandsMerger(loader.LoadedStories, OutputStory);
                Result = merger.Merge();
            }

            [Test]
            public void IsSuccessful() =>
                Assert.IsTrue(Result.IsSuccessful);

            [Test]
            public void Has2Commands() =>
                Assert.AreEqual(2, OutputStory.Commands.Count);

            [Test]
            public void HasNoErrorMessage() =>
                Assert.AreEqual("No command conflicts found", Result.Message);
        }

    }
}
