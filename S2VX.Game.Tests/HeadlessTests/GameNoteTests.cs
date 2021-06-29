using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osu.Framework.Timing;
using osuTK;
using osuTK.Input;
using S2VX.Game.Play;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;
using System.IO;
using System.Linq;

namespace S2VX.Game.Tests.HeadlessTests {
    [HeadlessTest]
    public class GameNoteTests : S2VXTestScene {
        [Resolved]
        private AudioManager Audio { get; set; }

        private S2VXStory Story { get; set; } = new S2VXStory();
        private StopwatchClock Stopwatch { get; set; }
        private PlayScreen PlayScreen { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            Add(new ScreenStack(PlayScreen = new PlayScreen(false, Story, S2VXTrack.Open(audioPath, Audio))));
        }

        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Clear score", () => PlayScreen.ScoreProcessor.Reset());
            AddStep("Reset story", () => Story.Reset());
            AddStep("Reset clock", () => Story.Clock = new FramedClock(Stopwatch = new StopwatchClock()));
        }

        [Test]
        public void OnPress_StackedNotes_HitsTopNote() {
            AddStep("Add notes", () => {
                Story.AddNote(new GameNote { HitTime = 0 });
                Story.AddNote(new GameNote { HitTime = 10 });
                Story.AddNote(new GameNote { HitTime = 20 });
            });

            AddStep("Seek clock", () => Stopwatch.Seek(25));
            AddStep("Move mouse to centre", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Hold key", () => InputManager.PressKey(Key.Z));
            AddStep("Release key", () => InputManager.ReleaseKey(Key.Z));
            AddStep("Seek clock", () => Stopwatch.Seek(Story.Notes.MissThreshold + 20));
            AddAssert("Hit only top note", () => PlayScreen.ScoreProcessor.ScoreStatistics.Score == Story.Notes.MissThreshold * 2 + 25);
        }

        [Test]
        public void OnPress_NoteThenNote_HitsLaterNote() {
            AddStep("Add notes", () => {
                Story.AddNote(new GameNote { HitTime = 0 });
                Story.AddNote(new GameNote { HitTime = 10, Coordinates = new Vector2(0, 1) });
            });

            AddStep("Seek clock", () => Stopwatch.Seek(10));
            AddStep("Move mouse to second note", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Hold key", () => InputManager.PressKey(Key.Z));
            AddStep("Release key", () => InputManager.ReleaseKey(Key.Z));
            AddStep("Seek clock", () => Stopwatch.Seek(Story.Notes.MissThreshold + 10));
            AddAssert("Hit only top note", () => PlayScreen.ScoreProcessor.ScoreStatistics.Score == Story.Notes.MissThreshold);
        }

        [Test]
        public void OnPress_OutsideMissThreshold_DoesNothing() {
            AddStep("Add note", () => Story.AddNote(new GameNote { HitTime = Story.Notes.MissThreshold + 50 }));
            AddStep("Move mouse to note", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Hold key", () => InputManager.PressKey(Key.Z));
            AddStep("Release key", () => InputManager.ReleaseKey(Key.Z));
            AddStep("Seek after post-threshold", () => Stopwatch.Seek(Story.Notes.MissThreshold * 2 + 60));
            AddAssert("Note was missed", () => PlayScreen.ScoreProcessor.ScoreStatistics.Score == Story.Notes.MissThreshold);
        }

        [Test]
        public void OnPress_WithinMissThreshold_RegistersHit() {
            GameNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameNote { HitTime = Story.Notes.MissThreshold + 50 }));
            AddStep("Seek between pre-threshold and HitTime", () => Stopwatch.Seek(note.HitTime - Story.Notes.MissThreshold / 2));
            AddStep("Move mouse to note", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Hold key", () => InputManager.PressKey(Key.Z));
            AddStep("Release key", () => InputManager.ReleaseKey(Key.Z));
            AddStep("Seek after post-threshold", () => Stopwatch.Seek(Story.Notes.MissThreshold * 2 + 60));
            AddAssert("Note was hit", () => PlayScreen.ScoreProcessor.ScoreStatistics.Score == Story.Notes.MissThreshold / 2);
        }
    }
}
