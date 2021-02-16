using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using osu.Framework.Timing;
using S2VX.Game.Editor;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Tests.VisualTests {
    public class EditorHoldNoteTests : S2VXTestScene {
        [Cached]
        private S2VXStory Story { get; set; } = new S2VXStory();
        [Cached]
        private ScreenStack Screens { get; set; } = new ScreenStack();
        [Cached]
        private EditorScreen Editor { get; set; } = new EditorScreen(null, null, null, null);

        private EditorHoldNote NoteToTest { get; set; }
        private StopwatchClock StoryClock { get; set; }
        private FramedClock FramedClock { get; set; }
        private readonly float NoteAppearTime = 1000.0f;
        private readonly float ShowDuration = 1000.0f;

        [BackgroundDependencyLoader]
        private void Load() {
            Add(Story);
            Add(Screens);
            Add(Editor);
            StoryClock = new StopwatchClock();
            FramedClock = new FramedClock(StoryClock);
            Story.Clock = FramedClock;
        }

        // All tests will have a hold note that starts to appear in 1 second and lasts for 1 second
        [SetUp]
        public void Setup() {
            Schedule(() => Story.RemoveNotesUpTo(Story.Notes.ShowTime + Story.Notes.FadeInTime + NoteAppearTime));
            Schedule(() => Story.AddNote(NoteToTest = new EditorHoldNote {
                HitTime = Story.Notes.ShowTime + Story.Notes.FadeInTime + NoteAppearTime,
                EndTime = Story.Notes.ShowTime + Story.Notes.FadeInTime + ShowDuration
            }));
        }

        [Test]
        public void EditorHoldNoteAlpha_BeforeFadeInTime_IsZero() {
            AddStep("Seek before FadeInTime", () => StoryClock.Seek(NoteAppearTime));
            AddAssert("Note is not visible", () => NoteToTest.Alpha == 0);
        }

        [Test]
        public void EditorHoldNoteAlpha_AfterFadeInBeforeShowTime_IsBetweenZeroAndOne() {
            AddStep("Seek between FadeInTime and ShowTime", () => StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime / 2));
            AddAssert("Note is partially visible", () => NoteToTest.Alpha > 0 && NoteToTest.Alpha < 1);
        }

        [Test]
        public void EditorHoldNoteAlpha_AfterShowTimeBeforeHitTime_IsOne() {
            AddStep("Seek between ShowTime and HitTime", () => StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime / 2));
            AddAssert("Note is fully visible", () => NoteToTest.Alpha == 1);
        }

        [Test]
        public void EditorHoldNoteAlpha_AfterHitTimeBeforeEndTime_IsOne() {
            AddStep("Seek between HitTime and EndTime", () =>
                StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + ShowDuration / 2));
            AddAssert("Note is fully visible", () => NoteToTest.Alpha == 1);
        }

        [Test]
        public void EditorHoldNoteAlpha_AfterEndTimeBeforeFadeOutTime_IsBetweenZeroAndOne() {
            AddStep("Seek between EndTime and FadeOutTime", () =>
                StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + ShowDuration + Story.Notes.FadeOutTime / 2));
            AddAssert("Note is partially visible", () => NoteToTest.Alpha > 0 && NoteToTest.Alpha < 1);
        }

        [Test]
        public void EditorHoldNoteAlpha_AfterFadeOutTime_IsZero() {
            AddStep("Seek after FadeOutTime", () =>
                StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + ShowDuration + Story.Notes.FadeOutTime));
            AddAssert("Note is not visible", () => NoteToTest.Alpha == 0);
        }

        [Test]
        public void EditorHoldNoteApproachAlpha_BeforeFadeInTime_IsZero() {
            AddStep("Seek before FadeInTime", () => StoryClock.Seek(NoteAppearTime));
            AddAssert("Note approach is not visible", () => NoteToTest.Approach.Alpha == 0);
        }

        [Test]
        public void EditorHoldNoteApproachAlpha_AfterFadeInBeforeShowTime_IsBetweenZeroAndOne() {
            AddStep("Seek between FadeInTime and ShowTime", () => StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime / 2));
            AddAssert("Note approach is partially visible", () => NoteToTest.Approach.Alpha > 0 && NoteToTest.Approach.Alpha < 1);
        }

        [Test]
        public void EditorHoldNoteApproachAlpha_AfterShowTimeBeforeHitTime_IsOne() {
            AddStep("Seek between ShowTime and HitTime", () => StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime / 2));
            AddAssert("Note approach is fully visible", () => NoteToTest.Approach.Alpha == 1);
        }

        [Test]
        public void EditorHoldNoteApproachAlpha_AfterHitTimeBeforeEndTime_IsOne() {
            AddStep("Seek between HitTime and EndTime", () =>
                StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + ShowDuration / 2));
            AddAssert("Note approach is fully visible", () => NoteToTest.Approach.Alpha == 1);
        }

        [Test]
        public void EditorHoldNoteApproachAlpha_AfterEndTimeBeforeFadeOutTime_IsBetweenZeroAndOne() {
            AddStep("Seek between EndTime and FadeOutTime", () =>
                StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + ShowDuration + Story.Notes.FadeOutTime / 2));
            AddAssert("Note approach is partially visible", () => NoteToTest.Approach.Alpha > 0 && NoteToTest.Approach.Alpha < 1);
        }

        [Test]
        public void EditorHoldNoteApproachAlpha_AfterFadeOutTime_IsZero() {
            AddStep("Seek after FadeOutTime", () =>
                StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + ShowDuration + Story.Notes.FadeOutTime));
            AddAssert("Note approach is not visible", () => NoteToTest.Approach.Alpha == 0);
        }

    }
}
