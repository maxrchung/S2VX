using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osu.Framework.Timing;
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
            Add(new ScreenStack(PlayScreen = new PlayScreen(false, Story, S2VXTrack.Open(audioPath, Audio))));
        }

        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Clear score", () => PlayScreen.ScoreInfo.ClearScore());
            AddStep("Reset story", () => Story.Reset());
            AddStep("Reset clock", () => Story.Clock = new FramedClock(Stopwatch = new StopwatchClock()));
        }

        public void PressAndRelease(double timeToPress, double holdDuration) {
            AddStep($"Seek to {timeToPress}", () => Stopwatch.Seek(timeToPress));
            AddStep("Press note", () => InputManager.PressKey(Key.Z));
            AddStep($"Seek to {timeToPress + holdDuration}", () => Stopwatch.Seek(timeToPress + holdDuration));
            AddStep("Release note", () => InputManager.ReleaseKey(Key.Z));
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
            AddAssert("Note is in NotVisible state", () => note.State == HoldNoteState.NotVisible);
        }

        [Test]
        public void GetState_VisibleBefore() {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Seek before MissThreshold", () => Stopwatch.Seek(note.HitTime - GameHoldNote.MissThreshold - 50));
            AddAssert("Note is in VisibleBefore state", () => note.State == HoldNoteState.VisibleBefore);
        }

        [Test]
        public void GetState_HitWindow() {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Seek within MissThreshold", () => Stopwatch.Seek(note.HitTime - GameHoldNote.MissThreshold / 2));
            AddAssert("Note is in HitWindow state", () => note.State == HoldNoteState.HitWindow);
        }

        [Test]
        public void GetState_During() {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Seek between HitTime and EndTime", () => Stopwatch.Seek((note.HitTime + note.EndTime) / 2));
            AddAssert("Note is in During state", () => note.State == HoldNoteState.During);
        }

        [Test]
        public void GetState_VisibleAfter() {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Seek after EndTime", () => Stopwatch.Seek(note.EndTime + Story.Notes.FadeOutTime / 2));
            AddAssert("Note is in VisibleAfter state", () => note.State == HoldNoteState.VisibleAfter);
        }

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
            AddAssert("Does not trigger multiple notes", () => PlayScreen.ScoreInfo.Score == (originalNoteCount - 1) * 100);
        }

        [TestCase(20, 20)]  // Press and Release entirely within NotVisible state
        [TestCase(20, 1330)] // Press in NotVisible, Release when note disappears
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
            PressAndRelease(timeToPress, holdDuration);
            AddStep("Seek after note is deleted", () => Stopwatch.Seek(note.EndTime + Story.Notes.FadeOutTime));
            AddAssert("Score is note duration (full miss)", () => PlayScreen.ScoreInfo.Score == 100.0);
        }

        [Test]
        public void Score_PressAndReleaseInHitWindow_IsOffsetFromHitTime() {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Move mouse to note", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            PressAndRelease(1000, 100);
            AddStep("Seek after note is deleted", () => Stopwatch.Seek(note.EndTime + Story.Notes.FadeOutTime));
            AddAssert("Score is offset from HitTime", () => PlayScreen.ScoreInfo.Score ==
                Story.Notes.FadeInTime + Story.Notes.ShowTime + 50 // Expected HitTime
                - 1000 // Actual HitTime
                + 100.0); // HoldNote Duration
        }

        [TestCase(1100, 250)]  // Press 50ms early, Release when note disappears
        [TestCase(1150, 200)]  // Press exactly at HitTime, Release when note disappears
        public void Score_PressInHitWindowReleaseInVisibleAfter_IsOffsetFromHitTime(int timeToPress, int holdDuration) {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Move mouse to note", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            PressAndRelease(timeToPress, holdDuration);
            AddStep("Seek after note is deleted", () => Stopwatch.Seek(note.EndTime + Story.Notes.FadeOutTime));
            AddAssert("Score is offset from HitTime", () => PlayScreen.ScoreInfo.Score ==
                Story.Notes.FadeInTime + Story.Notes.ShowTime + 50 // Expected HitTime
                - timeToPress); // Actual HitTime
        }

        [Test]
        public void Score_MultiplePressesInHitWindowOnly_IncludesFirstOffsetFromHitTime() {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Move mouse to note", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            PressAndRelease(950, 50);
            PressAndRelease(1000, 50);
            PressAndRelease(1050, 50);
            PressAndRelease(1100, 50);
            AddStep("Seek after note is deleted", () => Stopwatch.Seek(note.EndTime + Story.Notes.FadeOutTime));
            AddAssert("Score is first Press's offset from HitTime", () => PlayScreen.ScoreInfo.Score ==
                Story.Notes.FadeInTime + Story.Notes.ShowTime + 50 // HitTime
                - 950 // First HitTime
                + 100.0); // Hold note duration
        }

        [TestCase(1200, 50)] // Press 50ms late, Release on time (expect 50)
        [TestCase(1150, 50)] // Press on time, Release 50ms early (expect 50)
        [TestCase(1175, 50)] // Press 25ms late, Release 25ms early (expect 50)
        [TestCase(1150, 100)] // Press on time, Release on time (expect 0)
        public void Score_PressInDuring_OffsetsFromHitOrEndTime(int timeToPress, int holdDuration) {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Move mouse to note", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            PressAndRelease(timeToPress, holdDuration);
            AddStep("Seek after note is deleted", () => Stopwatch.Seek(note.EndTime + Story.Notes.FadeOutTime));
            AddAssert("Score is offset from HitTime and/or EndTime", () => PlayScreen.ScoreInfo.Score ==
                timeToPress // Actual HitTime
                - (Story.Notes.FadeInTime + Story.Notes.ShowTime + 50) // Expected HitTime
                + Story.Notes.FadeInTime + Story.Notes.ShowTime + 150 // Expected EndTime
                - (timeToPress + holdDuration));  // Actual EndTime
        }

        [Test]
        public void Score_OverlappingPresses_HaveNoEffect() {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Move mouse to note", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Seek to HitTime", () => Stopwatch.Seek(Story.Notes.FadeInTime + Story.Notes.ShowTime + 50));
            AddStep("Press Z", () => InputManager.PressKey(Key.Z));
            AddStep("Seek to HitTime + 5", () => Stopwatch.Seek(note.HitTime + 5));
            AddStep("Press X", () => InputManager.PressKey(Key.X));
            AddStep("Seek to HitTime + 10", () => Stopwatch.Seek(note.HitTime + 10));
            AddStep("Release X", () => InputManager.ReleaseKey(Key.X));
            AddStep("Seek to EndTime", () => Stopwatch.Seek(note.EndTime));
            AddStep("Release Z", () => InputManager.ReleaseKey(Key.Z));
            AddStep("Seek after note is deleted", () => Stopwatch.Seek(note.EndTime + Story.Notes.FadeOutTime));
            AddAssert("Score is 0", () => PlayScreen.ScoreInfo.Score == 0);
        }

        [Test]
        public void Score_SwitchingKeys_HasNoEffect() {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 50,
                EndTime = Story.Notes.FadeInTime + Story.Notes.ShowTime + 150
            }));
            AddStep("Move mouse to note", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Seek to HitTime", () => Stopwatch.Seek(Story.Notes.FadeInTime + Story.Notes.ShowTime + 50));
            AddStep("Press Z", () => InputManager.PressKey(Key.Z));
            AddStep("Seek to HitTime + 5", () => Stopwatch.Seek(note.HitTime + 5));
            AddStep("Press X", () => InputManager.PressKey(Key.X));
            AddStep("Seek to HitTime + 10", () => Stopwatch.Seek(note.HitTime + 10));
            AddStep("Release Z", () => InputManager.ReleaseKey(Key.Z));
            AddStep("Seek to EndTime", () => Stopwatch.Seek(note.EndTime));
            AddStep("Release X", () => InputManager.ReleaseKey(Key.X));
            AddStep("Seek after note is deleted", () => Stopwatch.Seek(note.EndTime + Story.Notes.FadeOutTime));
            AddAssert("Score is 0", () => PlayScreen.ScoreInfo.Score == 0);
        }

    }
}
