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

        [Test]
        public void OnPress_OutsideHitWindowAndDuring_ScoreIsFullDuration() {
            GameHoldNote note = null;
            var notes = Story.Notes;
            // Assuming defaults are as such:
            // FadeInTime = 100
            // ShowTime = 1000
            // MissThreshold = 200
            // FadeOutTime = 100
            // Then the times for each state should be, for a hold duration of 100:
            // NotVisible: 0-50
            // VisibleBefore: 50-950
            // HitWindow: 950-1150
            // During: 1150-1250
            // VisibleAfter: 1250-1350
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = notes.FadeInTime + notes.ShowTime + 50,
                EndTime = notes.FadeInTime + notes.ShowTime + 150
            }));
            AddStep("Move mouse to note", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Seek to the instant the note starts becoming visible", () => Stopwatch.Seek(note.HitTime - Story.Notes.FadeInTime));
            AddStep("Hold key", () => InputManager.PressKey(Key.Z));
            AddStep("Seek after post-threshold", () => Stopwatch.Seek(Story.Notes.FadeInTime + GameHoldNote.MissThreshold * 2 + 100));
            AddStep("Release key", () => InputManager.ReleaseKey(Key.Z));
            AddAssert("Score is at most twice MissThreshold", () => PlayScreen.ScoreInfo.Score <= GameHoldNote.MissThreshold * 2);
        }

        [Test]
        public void OnPress_KeyMaxHold_ScoreIsAtMostTwiceMissThreshold() {
            GameHoldNote note = null;
            AddStep("Add note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.FadeInTime + GameHoldNote.MissThreshold + 50,
                EndTime = Story.Notes.FadeInTime + GameHoldNote.MissThreshold + 100
            }));
            AddStep("Move mouse to note", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Seek to the instant the note starts becoming visible", () => Stopwatch.Seek(note.HitTime - Story.Notes.FadeInTime));
            AddStep("Hold key", () => InputManager.PressKey(Key.Z));
            AddStep("Seek after post-threshold", () => Stopwatch.Seek(Story.Notes.FadeInTime + GameHoldNote.MissThreshold * 2 + 100));
            AddStep("Release key", () => InputManager.ReleaseKey(Key.Z));
            AddAssert("Score is at most twice MissThreshold", () => PlayScreen.ScoreInfo.Score <= GameHoldNote.MissThreshold * 2);
        }
    }
}
