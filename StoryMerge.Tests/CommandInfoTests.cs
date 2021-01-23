using NUnit.Framework;
using S2VX.Game.Story.Command;
using System.Collections.Generic;

namespace StoryMerge.Tests {
    public class CommandInfoTests {
        [Test]
        public void ToString_WithHoldNote_PrintsNoteInfo() {
            var info = new CommandInfo(new NotesAlphaCommand {
                StartTime = 1000,
                EndTime = 2000
            });
            Assert.AreEqual("NotesAlpha from 1000 to 2000", info.ToString());
        }

        public class Sort_WithUnsortedCommandInfo {
            private List<CommandInfo> Infos;

            [SetUp]
            public void SetUp() {
                Infos = new List<CommandInfo>() {
                    new CommandInfo(new NotesAlphaCommand { StartTime = 1000, EndTime = 1000 }),
                    new CommandInfo(new NotesAlphaCommand { StartTime = 0, EndTime = 1000 }),
                    new CommandInfo(new NotesAlphaCommand { StartTime = 0, EndTime = 0 }),
                };
                Infos.Sort();
            }

            [Test]
            public void ItSortsByStartTime() =>
                Assert.AreEqual("NotesAlpha from 0 to 0", Infos[0].ToString());

            [Test]
            public void ItSortsByStartTimeThenEndTime() =>
                Assert.AreEqual("NotesAlpha from 0 to 1000", Infos[1].ToString());

            [Test]
            public void ItSortsLargerStartTimeLater() =>
                Assert.AreEqual("NotesAlpha from 1000 to 1000", Infos[2].ToString());
        }
    }
}
