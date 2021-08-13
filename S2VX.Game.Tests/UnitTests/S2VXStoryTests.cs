using NUnit.Framework;
using osuTK;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;
using System.Collections.Generic;
using System.Linq;

namespace S2VX.Game.Tests.UnitTests {
    public static class S2VXStoryTests {
        public class Open_HoldNote {
            private HoldNote LoadedHoldNote { get; set; }

            [SetUp]
            public void SetUp() {
                var story = new S2VXStory();
                var holdNote = new EditorHoldNote {
                    HitTime = 100,
                    EndTime = 400,
                    Coordinates = new(1, 1),
                    EndCoordinates = new(4, 4),
                };
                holdNote.MidCoordinates.Add(new(2, 2));
                holdNote.MidCoordinates.Add(new(3, 3));
                story.AddNote(holdNote);

                var filePath = "HoldNoteLoadingTests_Open_HoldNote_SetUp.s2ry";
                story.Save(filePath);

                var newStory = new S2VXStory();
                newStory.Open(filePath, true);
                LoadedHoldNote = newStory.Notes.GetHoldNotes().First();
            }

            [Test]
            public void HasExpectedHitTime() => Assert.AreEqual(100, LoadedHoldNote.HitTime);

            [Test]
            public void HasExpectedEndTime() => Assert.AreEqual(400, LoadedHoldNote.EndTime);

            [Test]
            public void HasExpectedCoordinates() => Assert.AreEqual(new Vector2(1, 1), LoadedHoldNote.Coordinates);

            [Test]
            public void HasExpectedEndCoordinates() => Assert.AreEqual(new Vector2(4, 4), LoadedHoldNote.EndCoordinates);

            [Test]
            public void HasExpectedMidCoordinates() => Assert.AreEqual(
                new List<Vector2> { new(2, 2), new(3, 3) },
                LoadedHoldNote.MidCoordinates
            );
        }
    }
}
