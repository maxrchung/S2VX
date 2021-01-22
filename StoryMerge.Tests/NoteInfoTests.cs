using NUnit.Framework;
using S2VX.Game.Story.Note;
using System.Collections.Generic;

namespace StoryMerge.Tests {
    public class NoteInfoTests {
        [Test]
        public void ToString_WithNote_PrintsNoteInfo() {
            var info = new NoteInfo(new EditorNote {
                HitTime = 1000
            });
            Assert.AreEqual("Note at 1000", info.ToString());
        }

        [Test]
        public void ToString_WithHoldNote_PrintsNoteInfo() {
            var info = new NoteInfo(new EditorHoldNote {
                HitTime = 1000,
                EndTime = 2000
            });
            Assert.AreEqual("HoldNote from 1000 to 2000", info.ToString());
        }

        public class Sort_WithUnsortedCommandInfo {
            private List<NoteInfo> Infos;

            [SetUp]
            public void SetUp() {
                Infos = new List<NoteInfo>() {
                    new NoteInfo(new EditorHoldNote { HitTime = 1000, EndTime = 1000 }),
                    new NoteInfo(new EditorHoldNote { HitTime = 0, EndTime = 1000 }),
                    new NoteInfo(new EditorNote { HitTime = 0 }),
                };
                Infos.Sort();
            }

            [Test]
            public void ItSortsByStartTime() =>
                Assert.AreEqual("Note at 0", Infos[0].ToString());

            [Test]
            public void ItSortsByStartTimeThenEndTime() =>
                Assert.AreEqual("HoldNote from 0 to 1000", Infos[1].ToString());

            [Test]
            public void ItSortsLargerStartTimeLater() =>
                Assert.AreEqual("HoldNote from 1000 to 1000", Infos[2].ToString());
        }
    }
}
