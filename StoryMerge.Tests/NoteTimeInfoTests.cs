using NUnit.Framework;
using S2VX.Game.Story.Note;
using System.Collections.Generic;

namespace StoryMerge.Tests {
    public class NoteTimeInfoTests {
        [Test]
        public void ToString_Note_PrintsNoteTimeInfo() {
            var info = new NoteTimeInfo(new EditorNote {
                HitTime = 1000
            });
            Assert.AreEqual("Note at 1000", info.ToString());
        }

        [Test]
        public void ToString_HoldNote_PrintsNoteTimeInfo() {
            var info = new NoteTimeInfo(new EditorHoldNote {
                HitTime = 1000,
                EndTime = 2000
            });
            Assert.AreEqual("HoldNote from 1000 to 2000", info.ToString());
        }

        public class Sort_UnsortedCommandTimeInfo {
            private List<NoteTimeInfo> Infos;

            [SetUp]
            public void SetUp() {
                Infos = new List<NoteTimeInfo>() {
                    new NoteTimeInfo(new EditorHoldNote { HitTime = 1000, EndTime = 1000 }),
                    new NoteTimeInfo(new EditorHoldNote { HitTime = 0, EndTime = 1000 }),
                    new NoteTimeInfo(new EditorNote { HitTime = 0 }),
                };
                Infos.Sort();
            }

            [Test]
            public void SortsByStartTime() =>
                Assert.AreEqual("Note at 0", Infos[0].ToString());

            [Test]
            public void SortsByStartTimeThenEndTime() =>
                Assert.AreEqual("HoldNote from 0 to 1000", Infos[1].ToString());

            [Test]
            public void SortsLargerStartTimeLater() =>
                Assert.AreEqual("HoldNote from 1000 to 1000", Infos[2].ToString());
        }
    }
}
