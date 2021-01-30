using NUnit.Framework;
using S2VX.Game.Story.Command;
using System.Collections.Generic;

namespace StoryMerge.Tests {
    public class CommandTimeInfoTests {
        [Test]
        public void ToString_Command_PrintsNoteTimeInfo() {
            var info = new CommandTimeInfo(new NotesAlphaCommand {
                StartTime = 1000,
                EndTime = 2000
            });
            Assert.AreEqual("NotesAlpha from 1000 to 2000", info.ToString());
        }

        public class Sort_UnsortedCommandTimeInfo {
            private List<CommandTimeInfo> Infos;

            [SetUp]
            public void SetUp() {
                Infos = new List<CommandTimeInfo>() {
                    new CommandTimeInfo(new NotesAlphaCommand { StartTime = 1000, EndTime = 1000 }),
                    new CommandTimeInfo(new NotesAlphaCommand { StartTime = 0, EndTime = 1000 }),
                    new CommandTimeInfo(new NotesAlphaCommand { StartTime = 0, EndTime = 0 }),
                };
                Infos.Sort();
            }

            [Test]
            public void SortsByStartTime() =>
                Assert.AreEqual("NotesAlpha from 0 to 0", Infos[0].ToString());

            [Test]
            public void SortsByStartTimeThenEndTime() =>
                Assert.AreEqual("NotesAlpha from 0 to 1000", Infos[1].ToString());

            [Test]
            public void SortsLargerStartTimeLater() =>
                Assert.AreEqual("NotesAlpha from 1000 to 1000", Infos[2].ToString());
        }
    }
}
