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

namespace S2VX.Game.Tests.VisualTests {
    public class GameNoteTests : S2VXTestScene {
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
            AddStep("Reset story", () => Story.Reset());
            AddStep("Reset clock", () => Story.Clock = new FramedClock(Stopwatch = new StopwatchClock()));
        }

        [Test]
        public void OnPress_KeyHeldDown_DoesNotTriggerMultipleNotes() {
            var originalNoteCount = 0;
            AddStep("Add notes", () => {
                for (var i = 0; i < 1000; i += 50) {
                    Story.AddNote(new GameNote { HitTime = i });
                }
                originalNoteCount = Story.Notes.Children.Count;
            });
            AddStep("Move mouse", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Hold key", () => InputManager.PressKey(Key.Z));
            AddStep("Start clock", () => Stopwatch.Start());
            AddUntilStep("Wait until all notes are removed", () => Story.Notes.Children.Count == 0);
            AddStep("Release key", () => InputManager.ReleaseKey(Key.Z));
            AddAssert("Does not trigger multiple notes", () =>
                (originalNoteCount - 1) * GameNote.MissThreshold == PlayScreen.ScoreInfo.Score
            );
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
            AddStep("Seek clock", () => Stopwatch.Seek(GameNote.MissThreshold + 20));
            AddAssert("Hit only top note", () => PlayScreen.ScoreInfo.Score == GameNote.MissThreshold * 2 + 25);
        }

        [Test]
        public void OnPress_LaterNote_HitsLaterNote() {
            AddStep("Add notes", () => {
                Story.AddNote(new GameNote { HitTime = 0 });
                Story.AddNote(new GameNote { HitTime = 10, Coordinates = new Vector2(0, 1) });
            });

            AddStep("Seek clock", () => Stopwatch.Seek(10));
            AddStep("Move mouse to second note", () => InputManager.MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Hold key", () => InputManager.PressKey(Key.Z));
            AddStep("Release key", () => InputManager.ReleaseKey(Key.Z));
            AddStep("Seek clock", () => Stopwatch.Seek(GameNote.MissThreshold + 10));
            AddAssert("Hit only top note", () => PlayScreen.ScoreInfo.Score == GameNote.MissThreshold);
        }
    }
}
