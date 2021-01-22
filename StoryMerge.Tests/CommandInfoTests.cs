using NUnit.Framework;
using S2VX.Game.Story.Command;
using System.Collections.Generic;

namespace StoryMerge.Tests {
    [TestFixture]
    public class CommandInfoTests {
        [Test]
        public void Constructor_WithCommand_LoadsCommandInfo() {
            var info = new CommandInfo(new NotesAlphaCommand {
                StartTime = 1000,
                EndTime = 2000
            });
            Assert.AreEqual("NotesAlpha", info.Type);
            Assert.AreEqual(1000, info.StartTime);
            Assert.AreEqual(2000, info.EndTime);
        }

        [Test]
        public void ToString_WithHoldNote_PrintsNoteInfo() {
            var info = new CommandInfo(new NotesAlphaCommand {
                StartTime = 1000,
                EndTime = 2000
            });
            Assert.AreEqual("NotesAlpha", info.Type);
            Assert.AreEqual(1000, info.StartTime);
            Assert.AreEqual(2000, info.EndTime);
            Assert.AreEqual("NotesAlpha from 1000 to 2000", info.ToString());
        }

        [Test]
        public void Sort_WithCommandInfo_SortsByStartTimeThenEndTime() {
            var infos = new List<CommandInfo>() {
                new CommandInfo(new NotesAlphaCommand { StartTime = 1000, EndTime = 1000 }),
                new CommandInfo(new NotesAlphaCommand { StartTime = 0, EndTime = 1000 }),
                new CommandInfo(new NotesAlphaCommand { StartTime = 0 }),
            };
            infos.Sort();
            Assert.AreEqual("NotesAlpha from 0 to 0", infos[0].ToString());
            Assert.AreEqual("NotesAlpha from 0 to 1000", infos[1].ToString());
            Assert.AreEqual("NotesAlpha from 1000 to 1000", infos[2].ToString());
        }
    }
}
