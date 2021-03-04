using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osu.Framework.Timing;
using osu.Framework.Utils;
using osuTK.Input;
using S2VX.Game.Play;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;
using System.IO;
using System.Linq;

namespace S2VX.Game.Tests.VisualTests {
    public class GameHoldNoteTests : S2VXTestScene {
        [Resolved]
        private AudioManager Audio { get; set; }

        private S2VXStory Story { get; set; } = new S2VXStory();
        private StopwatchClock Stopwatch { get; set; }
        private PlayScreen PlayScreen { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            var audioPath = Path.Combine("VisualTests", "TestTracks", "10-seconds-of-silence.mp3");
            Add(new ScreenStack(PlayScreen = new PlayScreen(false, Story, S2VXUtils.LoadDrawableTrack(audioPath, Audio))));
        }

        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Clear score", () => PlayScreen.ScoreInfo.ClearScore());
            AddStep("Reset story", () =>
            Story.Reset());
            AddStep("Reset clock", () => Story.Clock = new FramedClock(Stopwatch = new StopwatchClock()));
        }

        // Assuming defaults are as such:
        //     FadeInTime = 100
        //     ShowTime = 1000
        //     MissThreshold = 200
        //     FadeOutTime = 100
        // Then the times for each state should be, for a hold duration of 100:
        //     NotVisible: 0-50
        //     VisibleBefore: 50-950
        //     HitWindow: 950-1150
        //     During: 1150-1250
        //     VisibleAfter: 1250-1350
        [Test]
        public void GetState_NotVisible() {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Seek before note is visible", () => Stopwatch.Seek(25));
            AddAssert("Note is in NotVisible state", () => note.State == GameHoldNote.HoldNoteState.NotVisible);
        }

        [Test]
        public void GetState_VisibleBefore() {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Seek before MissThreshold", () => Stopwatch.Seek(note.HitTime - GameHoldNote.MissThreshold - 50));
            AddAssert("Note is in VisibleBefore state", () => note.State == GameHoldNote.HoldNoteState.VisibleBefore);
        }

        [Test]
        public void GetState_HitWindow() {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Seek within MissThreshold", () => Stopwatch.Seek(note.HitTime - GameHoldNote.MissThreshold / 2));
            AddAssert("Note is in HitWindow state", () => note.State == GameHoldNote.HoldNoteState.HitWindow);
        }

        [Test]
        public void GetState_During() {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Seek between HitTime and EndTime", () => Stopwatch.Seek((note.HitTime + note.EndTime) / 2));
            AddAssert("Note is in During state", () => note.State == GameHoldNote.HoldNoteState.During);
        }

        [Test]
        public void GetState_VisibleAfter() {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Seek after EndTime", () => Stopwatch.Seek(note.EndTime + Story.Notes.FadeOutTime / 2));
            AddAssert("Note is in VisibleAfter state", () => note.State == GameHoldNote.HoldNoteState.VisibleAfter);
        }

        [TestCase(0, 50)]  // Press and Release entirely within NotVisible state
        [TestCase(0, 1350)] // Press in NotVisible, Release when note disappears
        [TestCase(100, 800)]  // Press and Release entirely within VisibleBefore state
        [TestCase(100, 1250)] // Press in VisibleBefore, Release when note disappears
        [TestCase(1260, 40)] // Press and Release entirely within VisibleAfter state
        public void Score_PressIgnored_IsFullDuration(int timeToPress, int holdDuration) {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Move mouse to note", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep($"Seek to {timeToPress}", () => Stopwatch.Seek(timeToPress));
            AddStep($"Hold key and release after {holdDuration}", () => {
                InputManager.PressKey(Key.Z);
                Scheduler.AddDelayed(() => InputManager.ReleaseKey(Key.Z), holdDuration);
            });
            AddStep("Start clock", () => Stopwatch.Start());
            AddUntilStep("Wait until note is deleted", () => Story.Notes.Children.Count == 0);
            AddAssert("Score is note duration (full miss)", () => PlayScreen.ScoreInfo.Score == 100.0);
        }

        [TestCase(1100, 250)] // Press in HitWindow, Release when note disappears
        [TestCase(1000, 100)] // Press and Release entirely within HitWindow
        public void Score_PressInHitWindowOnly_IncludesOffsetFromHitTime(int timeToPress, int holdDuration) {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Move mouse to note", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep($"Seek to {timeToPress}", () => Stopwatch.Seek(timeToPress));
            AddStep($"Hold key and release after {holdDuration}", () => {
                InputManager.PressKey(Key.Z);
                Scheduler.AddDelayed(() => InputManager.ReleaseKey(Key.Z), holdDuration);
            });
            AddStep("Start clock", () => Stopwatch.Start());
            AddUntilStep("Wait until note is deleted", () => Story.Notes.Children.Count == 0);
            AddAssert("Score is offset from HitTime", () => PlayScreen.ScoreInfo.Score ==
                Story.Notes.FadeInTime + Story.Notes.ShowTime + 50 // HitTime
                + 100.0 // Hold duration
                - timeToPress);
        }

        [Test]
        public void Score_MultiplePressesInHitWindowOnly_IncludesFirstOffsetFromHitTime() {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Move mouse to note", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Seek to start of HitWindow", () => Stopwatch.Seek(950));
            AddStep("Repeatedly press and release", () => {
                InputManager.PressKey(Key.Z);
                Scheduler.AddDelayed(() => InputManager.ReleaseKey(Key.Z), 50);
                Scheduler.AddDelayed(() => InputManager.PressKey(Key.Z), 100);
                Scheduler.AddDelayed(() => InputManager.ReleaseKey(Key.Z), 150);
                Scheduler.AddDelayed(() => InputManager.PressKey(Key.Z), 200);
                Scheduler.AddDelayed(() => InputManager.ReleaseKey(Key.Z), 250);
            });
            AddStep("Start clock", () => Stopwatch.Start());
            AddUntilStep("Wait until note is deleted", () => Story.Notes.Children.Count == 0);
            AddAssert("Score is first Press's offset from HitTime", () => PlayScreen.ScoreInfo.Score ==
                Story.Notes.FadeInTime + Story.Notes.ShowTime + 50 // HitTime
                + 100.0 // Hold duration
                - 950);
        }

        //[Test]
        //public void Score_OverlappingPresses_HaveNoEffect() {

        //}

        [Test]
        public void OnPress_KeyHeldDown_DoesNotTriggerMultipleNotes() {
            var originalNoteCount = 0;
            AddStep("Add notes", () => {
                for (var i = 0; i < 1000; i += 100 + 50) {
                    Story.AddNote(new GameHoldNote {
                        HitTime = i,
                        EndTime = i + 100
                    });
                }
                originalNoteCount = Story.Notes.Children.Count;
            });
            AddStep("Move mouse", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Hold key", () => InputManager.PressKey(Key.Z));
            AddStep("Start clock", () => Stopwatch.Start());
            AddUntilStep("Wait until all notes are removed", () => Story.Notes.Children.Count == 0);
            AddStep("Release key", () => InputManager.ReleaseKey(Key.Z));
            AddAssert("Does not trigger multiple notes", () =>
                Precision.AlmostEquals(originalNoteCount * 100, PlayScreen.ScoreInfo.Score, 20)
            );
        }

    }
}
