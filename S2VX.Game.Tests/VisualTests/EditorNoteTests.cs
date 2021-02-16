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
        private readonly float NoteAppearTime = 1000.0f;

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
            Schedule(() => Story.RemoveNotesUpTo(Story.Notes.ShowTime + Story.Notes.FadeInTime + NoteAppearTime));
            Schedule(() => Story.AddNote(NoteToTest = new EditorNote {
                HitTime = Story.Notes.ShowTime + Story.Notes.FadeInTime + NoteAppearTime
            }));
        }

        [Test]
        public void EditorNoteAlpha_BeforeFadeInTime_IsZero() {
            AddStep("Seek before FadeInTime", () => StoryClock.Seek(NoteAppearTime));
            AddAssert("Note is not visible", () => NoteToTest.Alpha == 0);
        }

        [Test]
        public void EditorNoteAlpha_AfterFadeInBeforeShowTime_IsBetweenZeroAndOne() {
            AddStep("Seek between FadeInTime and ShowTime", () => StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime / 2));
            AddAssert("Note is partially visible", () => NoteToTest.Alpha > 0 && NoteToTest.Alpha < 1);
        }

        [Test]
        public void EditorNoteAlpha_AfterShowTimeBeforeHitTime_IsOne() {
            AddStep("Seek between ShowTime and HitTime", () => StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime / 2));
            AddAssert("Note is fully visible", () => NoteToTest.Alpha == 1);
        }

        [Test]
        public void EditorNoteAlpha_AfterHitTimeBeforeFadeOutTime_IsBetweenZeroAndOne() {
            AddStep("Seek between HitTime and FadeOutTime", () =>
                StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + Story.Notes.FadeOutTime / 2));
            AddAssert("Note is partially visible", () => NoteToTest.Alpha > 0 && NoteToTest.Alpha < 1);
        }

        [Test]
        public void EditorNoteAlpha_AfterFadeOutTime_IsZero() {
            AddStep("Seek after FadeOutTime", () =>
                StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + Story.Notes.FadeOutTime));
            AddAssert("Note is not visible", () => NoteToTest.Alpha == 0);
        }

        [Test]
        public void EditorNoteApproachAlpha_BeforeFadeInTime_IsZero() {
            AddStep("Seek before FadeInTime", () => StoryClock.Seek(NoteAppearTime));
            AddAssert("Note approach is not visible", () => NoteToTest.Approach.Alpha == 0);
        }

        [Test]
        public void EditorNoteApproachAlpha_AfterFadeInBeforeShowTime_IsBetweenZeroAndOne() {
            AddStep("Seek between FadeInTime and ShowTime", () => StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime / 2));
            AddAssert("Note approach is partially visible", () => NoteToTest.Approach.Alpha > 0 && NoteToTest.Approach.Alpha < 1);
        }

        [Test]
        public void EditorNoteApproachAlpha_AfterShowTimeBeforeHitTime_IsOne() {
            AddStep("Seek between ShowTime and HitTime", () => StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime / 2));
            AddAssert("Note approach is fully visible", () => NoteToTest.Approach.Alpha == 1);
        }

        [Test]
        public void EditorNoteApproachAlpha_AfterHitTimeBeforeFadeOutTime_IsBetweenZeroAndOne() {
            AddStep("Seek between HitTime and FadeOutTime", () =>
                StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + Story.Notes.FadeOutTime / 2));
            AddAssert("Note approach is partially visible", () => NoteToTest.Approach.Alpha > 0 && NoteToTest.Approach.Alpha < 1);
        }

        [Test]
        public void EditorNoteApproachAlpha_AfterFadeOutTime_IsZero() {
            AddStep("Seek after FadeOutTime", () =>
                StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + Story.Notes.FadeOutTime));
            AddAssert("Note approach is not visible", () => NoteToTest.Approach.Alpha == 0);
        }

    }
}
