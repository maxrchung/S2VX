using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using S2VX.Game.Editor;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Tests.VisualTests {
    public class EditorHoldNoteTests : S2VXTestScene {
        [Cached]
        private S2VXStory Story { get; set; } = new S2VXStory();
        private EditorScreen Editor { get; set; }

        [Resolved]
        private AudioManager Audio { get; set; }

        private static string AudioPath { get; } = "VisualTests/TestTracks/10-seconds-of-silence.mp3";
        private static float NoteAppearTime { get; } = 1000.0f;
        private static float HoldDuration { get; } = 1000.0f;
        private EditorHoldNote NoteToTest { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            var screenStack = new ScreenStack();
            var drawableTrack = S2VXTrack.Open(AudioPath, Audio);
            Editor = new EditorScreen(Story, drawableTrack);
            screenStack.Push(Editor);
            Add(screenStack);
        }

        // All tests will have a hold note that starts to appear in 1 second and lasts for 1 second
        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Remove notes", () => Story.RemoveNotesUpTo(Story.Notes.ShowTime + Story.Notes.FadeInTime + NoteAppearTime));
            AddStep("Add note", () => Story.AddNote(NoteToTest = new EditorHoldNote {
                HitTime = Story.Notes.ShowTime + Story.Notes.FadeInTime + NoteAppearTime,
                EndTime = Story.Notes.ShowTime + Story.Notes.FadeInTime + NoteAppearTime + HoldDuration
            }));
        }

        [Test]
        public void EditorHoldNoteAlpha_BeforeFadeInTime_IsZero() {
            AddStep("Seek before FadeInTime", () => Editor.Seek(NoteAppearTime));
            AddAssert("Note is not visible", () => NoteToTest.Alpha == 0);
        }

        [Test]
        public void EditorHoldNoteAlpha_AfterFadeInBeforeShowTime_IsBetweenZeroAndOne() {
            AddStep("Seek between FadeInTime and ShowTime", () => Editor.Seek(NoteAppearTime + Story.Notes.FadeInTime / 2));
            AddAssert("Note is partially visible", () => NoteToTest.Alpha is > 0 and < 1);
        }

        [Test]
        public void EditorHoldNoteAlpha_AfterShowTimeBeforeHitTime_IsOne() {
            AddStep("Seek between ShowTime and HitTime", () => Editor.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime / 2));
            AddAssert("Note is fully visible", () => NoteToTest.Alpha == 1);
        }

        [Test]
        public void EditorHoldNoteAlpha_AfterHitTimeBeforeEndTime_IsOne() {
            AddStep("Seek between HitTime and EndTime", () =>
                Editor.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + HoldDuration / 2));
            AddAssert("Note is fully visible", () => NoteToTest.Alpha == 1);
        }

        [Test]
        public void EditorHoldNoteAlpha_AfterEndTimeBeforeFadeOutTime_IsBetweenZeroAndOne() {
            AddStep("Seek between EndTime and FadeOutTime", () =>
                Editor.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + HoldDuration + Story.Notes.FadeOutTime / 2));
            AddAssert("Note is partially visible", () => NoteToTest.Alpha is > 0 and < 1);
        }

        [Test]
        public void EditorHoldNoteAlpha_AfterFadeOutTime_IsZero() {
            AddStep("Seek after FadeOutTime", () =>
                Editor.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + HoldDuration + Story.Notes.FadeOutTime));
            AddAssert("Note is not visible", () => NoteToTest.Alpha == 0);
        }

        [Test]
        public void EditorHoldNoteApproachAlpha_BeforeFadeInTime_IsZero() {
            AddStep("Seek before FadeInTime", () => Editor.Seek(NoteAppearTime));
            AddAssert("Note approach is not visible", () => NoteToTest.Approach.Alpha == 0);
        }

        [Test]
        public void EditorHoldNoteApproachAlpha_AfterFadeInBeforeShowTime_IsBetweenZeroAndOne() {
            AddStep("Seek between FadeInTime and ShowTime", () => Editor.Seek(NoteAppearTime + Story.Notes.FadeInTime / 2));
            AddAssert("Note approach is partially visible", () => NoteToTest.Approach.Alpha is > 0 and < 1);
        }

        [Test]
        public void EditorHoldNoteApproachAlpha_AfterShowTimeBeforeHitTime_IsOne() {
            AddStep("Seek between ShowTime and HitTime", () => Editor.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime / 2));
            AddAssert("Note approach is fully visible", () => NoteToTest.Approach.Alpha == 1);
        }

        [Test]
        public void EditorHoldNoteApproachAlpha_AfterHitTimeBeforeEndTime_IsOne() {
            AddStep("Seek between HitTime and EndTime", () =>
                Editor.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + HoldDuration / 2));
            AddAssert("Note approach is fully visible", () => NoteToTest.Approach.Alpha == 1);
        }

        [Test]
        public void EditorHoldNoteApproachAlpha_AfterEndTimeBeforeFadeOutTime_IsBetweenZeroAndOne() {
            AddStep("Seek between EndTime and FadeOutTime", () =>
                Editor.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + HoldDuration + Story.Notes.FadeOutTime / 2));
            AddAssert("Note approach is partially visible", () => NoteToTest.Approach.Alpha is > 0 and < 1);
        }

        [Test]
        public void EditorHoldNoteApproachAlpha_AfterFadeOutTime_IsZero() {
            AddStep("Seek after FadeOutTime", () =>
                Editor.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + HoldDuration + Story.Notes.FadeOutTime));
            AddAssert("Note approach is not visible", () => NoteToTest.Approach.Alpha == 0);
        }

    }
}
