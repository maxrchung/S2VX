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
        public void Sort_WithNoteInfo_SortsByStartTimeThenEndTime() {
            var infos = new List<NoteInfo>() {
                new NoteInfo(new EditorHoldNote { HitTime = 1000, EndTime = 1000 }),
                new NoteInfo(new EditorHoldNote { HitTime = 0, EndTime = 1000 }),
                new NoteInfo(new EditorNote { HitTime = 0 }),
            };
            infos.Sort();
            Assert.AreEqual("Note at 0", infos[0].ToString());
            Assert.AreEqual("HoldNote from 0 to 1000", infos[1].ToString());
            Assert.AreEqual("HoldNote from 1000 to 1000", infos[2].ToString());
        }
    }
}
