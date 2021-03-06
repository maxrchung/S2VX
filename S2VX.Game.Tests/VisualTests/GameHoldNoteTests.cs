using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osu.Framework.Timing;
using osu.Framework.Utils;
using osuTK;
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

        [Test]
        public void OnPress_KeyHeldDown_DoesNotTriggerMultipleNotes() {
            var originalNoteCount = 0;
            AddStep("Add notes", () => {
                for (var i = 0; i < 1000; i += 100 + 50) {
                    Story.AddHoldNote(new GameHoldNote {
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
        public void OnPress_HoldNoteOnTopOfNote_HitsHoldNote() {
            AddStep("Add notes", () => {
                Story.AddHoldNote(new GameHoldNote { HitTime = 0, EndTime = 10 });
                Story.AddNote(new GameNote { HitTime = 100 });
            });

            AddStep("Move mouse to centre", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Hold key", () => InputManager.PressKey(Key.Z));
            AddStep("Seek clock", () => Stopwatch.Seek(10));
            AddStep("Release key", () => InputManager.ReleaseKey(Key.Z));
            AddStep("Seek clock", () => Stopwatch.Seek(GameNote.MissThreshold + 100));
            AddAssert("Hits only hold note", () => PlayScreen.ScoreInfo.Score == GameNote.MissThreshold + 10);
        }

        [Test]
        public void OnPress_NoteOnTopOfHoldNote_HitsNote() {
            AddStep("Add notes", () => {
                Story.AddNote(new GameNote { HitTime = 0 });
                Story.AddHoldNote(new GameHoldNote { HitTime = 1, EndTime = 100 });
            });

            AddStep("Seek clock", () => Stopwatch.Seek(100));
            AddStep("Move mouse to centre", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Hold key", () => InputManager.PressKey(Key.Z));
            AddStep("Release key", () => InputManager.ReleaseKey(Key.Z));
            AddStep("Seek clock", () => Stopwatch.Seek(GameNote.MissThreshold + 100));
            AddAssert("Hit only top note", () => PlayScreen.ScoreInfo.Score == GameNote.MissThreshold + 100);
        }

        [Test]
        public void OnPress_NoteThenHoldNote_HitsHoldNote() {
            AddStep("Add notes", () => {
                Story.AddNote(new GameNote { HitTime = 0 });
                Story.AddHoldNote(new GameHoldNote { HitTime = 10, EndTime = 100, Coordinates = new Vector2(0, 1) });
            });

            AddStep("Seek clock", () => Stopwatch.Seek(10));
            AddStep("Move mouse to HoldNote", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Hold key", () => InputManager.PressKey(Key.Z));
            AddStep("Seek clock", () => Stopwatch.Seek(100));
            AddStep("Release key", () => InputManager.ReleaseKey(Key.Z));
            AddStep("Seek clock", () => Stopwatch.Seek(GameNote.MissThreshold + 100));
            AddAssert("Hit only top note", () => PlayScreen.ScoreInfo.Score == GameNote.MissThreshold);
        }
    }
}
