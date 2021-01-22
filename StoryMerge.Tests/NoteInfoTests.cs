using NUnit.Framework;
using S2VX.Game.Story.Note;
using System.Collections.Generic;

namespace StoryMerge.Tests {
    [TestFixture]
    public class NoteInfoTests {
        [Test]
        public void Constructor_WithNote_LoadsNoteInfo() {
            var info = new NoteInfo(new EditorNote {
                HitTime = 1000
            });
            Assert.AreEqual("Note", info.Type);
            Assert.AreEqual(1000, info.StartTime);
            Assert.AreEqual(1000, info.EndTime);
        }

        [Test]
        public void Constructor_WithHoldNote_LoadsNoteInfo() {
            var info = new NoteInfo(new EditorHoldNote {
                HitTime = 1000,
                EndTime = 2000
            });
            Assert.AreEqual("HoldNote", info.Type);
            Assert.AreEqual(1000, info.StartTime);
            Assert.AreEqual(2000, info.EndTime);
        }

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

        [Test]
        public void Sort_WithNoteInfo_SortsByStartTime() {
            var infos = new List<NoteInfo>() {
                new NoteInfo(new EditorHoldNote { HitTime = 1000, EndTime = 2000 }),
                new NoteInfo(new EditorNote { HitTime = 1200 }),
                new NoteInfo(new EditorHoldNote { HitTime = 500, EndTime = 2500 }),
                new NoteInfo(new EditorNote { HitTime = 400 }),
            };
            infos.Sort();
            Assert.AreEqual("Note at 400", infos[0].ToString());
            Assert.AreEqual("HoldNote from 500 to 2500", infos[1].ToString());
            Assert.AreEqual("HoldNote from 1000 to 2000", infos[2].ToString());
            Assert.AreEqual("Note at 1200", infos[3].ToString());
        }
    }
}
