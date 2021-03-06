using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osu.Framework.Timing;
using S2VX.Game.Editor;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;
using S2VX.Game.Story.Note;
using System.IO;

namespace S2VX.Game.Tests.HeadlessTests {
    [HeadlessTest]
    public class EditorNoteTests : S2VXTestScene {
        [Cached]
        private S2VXStory Story { get; set; } = new();

        [Resolved]
        private AudioManager Audio { get; set; }

        private EditorNote NoteToTest { get; set; }
        private StopwatchClock StoryClock { get; set; }
        private readonly float NoteAppearTime = 1000.0f;

        [BackgroundDependencyLoader]
        private void Load() {
            StoryClock = new();
            Story.Clock = new FramedClock(StoryClock);
            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            Add(new ScreenStack(new EditorScreen(Story, S2VXTrack.Open(audioPath, Audio))));
        }

        // All tests will have a note that starts to appear in 1 second
        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Reset story", () => Story.Reset());
            AddStep("Reset clock", () => Story.Clock = new FramedClock(StoryClock = new()));
            AddStep("Add note", () => Story.AddNote(NoteToTest = new() {
                HitTime = Story.Notes.ShowTime + Story.Notes.FadeInTime + NoteAppearTime
            }));
            AddStep("Set max note alpha to 1", () => Story.AddCommand(new NotesAlphaCommand {
                StartValue = 1.0f,
                EndValue = 1.0f
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
            AddAssert("Note is partially visible", () => NoteToTest.Alpha is > 0 and < 1);
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
            AddAssert("Note is partially visible", () => NoteToTest.Alpha is > 0 and < 1);
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
            AddAssert("Note approach is partially visible", () => NoteToTest.Approach.Alpha is > 0 and < 1);
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
            AddAssert("Note approach is partially visible", () => NoteToTest.Approach.Alpha is > 0 and < 1);
        }

        [Test]
        public void EditorNoteApproachAlpha_AfterFadeOutTime_IsZero() {
            AddStep("Seek after FadeOutTime", () =>
                StoryClock.Seek(NoteAppearTime + Story.Notes.FadeInTime + Story.Notes.ShowTime + Story.Notes.FadeOutTime));
            AddAssert("Note approach is not visible", () => NoteToTest.Approach.Alpha == 0);
        }
    }
}
