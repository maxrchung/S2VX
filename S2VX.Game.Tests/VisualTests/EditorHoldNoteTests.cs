using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using S2VX.Game.Editor;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;
using System.IO;

namespace S2VX.Game.Tests.VisualTests {
    public class EditorHoldNoteTests : S2VXTestScene {
        [Cached]
        private S2VXStory Story { get; set; } = new S2VXStory();
        private EditorScreen Editor { get; set; }

        [Resolved]
        private AudioManager Audio { get; set; }

        private static float NoteAppearTime { get; } = 1000.0f;
        private static float HoldDuration { get; } = 1000.0f;
        private EditorHoldNote NoteToTest { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            Add(new ScreenStack(Editor = new EditorScreen(Story, S2VXTrack.Open(audioPath, Audio))));
        }

        // All tests will have a hold note that starts to appear in 1 second and lasts for 1 second
        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Pause editor", () => Editor.Play(false));
            AddStep("Restart editor", () => Editor.Restart());
            AddStep("Reset story", () => Story.Reset());
            AddStep("Add note", () => Story.AddNote(NoteToTest = new EditorHoldNote {
                HitTime = Story.Notes.ShowTime + Story.Notes.FadeInTime + NoteAppearTime,
                EndTime = Story.Notes.ShowTime + Story.Notes.FadeInTime + NoteAppearTime + HoldDuration
            }));
        }

        [Test]
        public void Hit_BeforeHitTime_DoesNotPlay() =>
            AddAssert("Does not play", () => NoteToTest.Hit.PlayCount == 0);

        [Test]
        public void Hit_BetweenHitAndEndTime_PlaysOnce() {
            AddStep("Start play", () => Editor.Play(true));
            AddUntilStep("Play until between hit and end time", () =>
                Story.Clock.CurrentTime > NoteToTest.HitTime && Story.Clock.CurrentTime < NoteToTest.EndTime
            );
            AddAssert("Plays once", () => NoteToTest.Hit.PlayCount == 1);
        }

        [Test]
        public void Hit_AfterEndTime_PlaysTwice() {
            AddStep("Start play", () => Editor.Play(true));
            AddUntilStep("Play until after end time", () => Story.Clock.CurrentTime > NoteToTest.EndTime);
            AddAssert("Plays twice", () => NoteToTest.Hit.PlayCount == 2);
        }
    }
}
