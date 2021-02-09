using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Timing;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Tests.VisualTests {
    public class EditorNoteTests : S2VXTestScene {
        [Cached]
        private S2VXStory Story { get; set; } = new S2VXStory();

        private EditorNote NoteToTest { get; set; }
        private StopwatchClock StoryClock { get; set; }
        private FramedClock FramedClock { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            Add(Story);
            StoryClock = new StopwatchClock();
            FramedClock = new FramedClock(StoryClock);
            Story.Clock = FramedClock;
        }

        // All tests will have a note that starts to appear in 1 second
        [SetUp]
        public void Setup() {
            Story.RemoveNotesUpTo(Story.Notes.ShowTime + Story.Notes.FadeInTime + 1000);
            Story.AddNote(NoteToTest = new EditorNote {
                HitTime = Story.Notes.ShowTime + Story.Notes.FadeInTime + 1000
            });
        }

        [Test]
        public void EditorNoteAlpha_BeforeFadeInTime_IsZero() {
            AddStep("Seek before FadeInTime", () => StoryClock.Seek(1000));
            AddAssert("Note is not visible", () => NoteToTest.Alpha == 0);
        }

        [Test]
        public void EditorNoteAlpha_AfterFadeInBeforeShowTime_IsBetweenZeroAndOne() {
            AddStep("Seek between FadeInTime and ShowTime", () => StoryClock.Seek(1000 + Story.Notes.FadeInTime / 2));
            AddAssert("Note is partially visible", () => NoteToTest.Alpha > 0 && NoteToTest.Alpha < 1);
        }

        [Test]
        public void EditorNoteAlpha_AfterShowTimeBeforeHitTime_IsOne() {
            AddStep("Seek between ShowTime and HitTime", () => StoryClock.Seek(1000 + Story.Notes.FadeInTime + Story.Notes.ShowTime / 2));
            AddAssert("Note is fully visible", () => NoteToTest.Alpha == 1);
        }

        [Test]
        public void EditorNoteAlpha_AfterHitTimeBeforeFadeOutTime_IsBetweenZeroAndOne() {
            AddStep("Seek between HitTime and FadeOutTime", () =>
                StoryClock.Seek(1000 + Story.Notes.FadeInTime + Story.Notes.ShowTime + Story.Notes.FadeOutTime / 2));
            AddAssert("Note is partially visible", () => NoteToTest.Alpha > 0 && NoteToTest.Alpha < 1);
        }

        [Test]
        public void EditorNoteAlpha_AfterFadeOutTime_IsZero() {
            AddStep("Seek after FadeOutTime", () =>
                StoryClock.Seek(1000 + Story.Notes.FadeInTime + Story.Notes.ShowTime + Story.Notes.FadeOutTime));
            AddAssert("Note is not visible", () => NoteToTest.Alpha == 0);
        }
    }
}
