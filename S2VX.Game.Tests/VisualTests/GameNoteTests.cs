using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osu.Framework.Timing;
using osu.Framework.Utils;
using osuTK.Graphics;
using osuTK.Input;
using S2VX.Game.Play;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;
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
        public void OnPress_KeyHeldDown_DoesNotTriggerMultipleNotes() {
            var originalNoteCount = 0;
            AddStep("Add notes", () => {
                for (var i = 0; i < 1000; i += 50) {
                    Story.AddNote(new GameNote { HitTime = i });
                }
                originalNoteCount = Story.Notes.Children.Count;
            });
            AddStep("Move mouse", () => MoveMouseTo(Story.Notes.Children.First()));
            AddStep("Hold key", () => InputManager.PressKey(Key.Z));
            AddStep("Start clock", () => Stopwatch.Start());
            AddUntilStep("Wait until all notes are removed", () => Story.Notes.Children.Count == 0);
            AddStep("Release key", () => InputManager.ReleaseKey(Key.Z));
            AddAssert("Does not trigger multiple notes", () =>
                (originalNoteCount - 1) * Notes.MissThreshold == PlayScreen.ScoreProcessor.ScoreStatistics.Score
            );
        }

        [Test]
        public void UpdateColor_ApproachesColorCommand_IsGreen() {
            GameNote note = null;
            AddStep("Add a note", () => Story.AddNote(note = new GameNote { HitTime = 1000 }));
            AddStep("Apply Green ApproachesColorCommand", () => Story.AddCommand(new ApproachesColorCommand {
                StartValue = Color4.Green,
                EndValue = Color4.Green
            }));
            AddAssert("Approach is green", () => note.Approach.Colour == Color4.Green);
        }

        [Test]
        public void UpdateColor_NotesAlphaCommand_IsHalf() {
            GameNote note = null;
            AddStep("Add a note", () => Story.AddNote(note = new GameNote {
                HitTime = Story.Notes.ShowTime - 100
            }));
            AddStep("Apply half NotesAlphaCommand", () => Story.AddCommand(new NotesAlphaCommand {
                StartValue = 0.5f,
                EndValue = 0.5f
            }));
            AddAssert("Note is half alpha", () => Precision.AlmostEquals(note.Alpha, 0.5f));
        }

        [Test]
        public void UpdateColor_NotesColorCommand_IsGreen() {
            GameNote note = null;
            AddStep("Add a note", () => Story.AddNote(note = new GameNote {
                HitTime = Story.Notes.ShowTime - 100
            }));
            AddStep("Apply green NotesColorCommand", () => Story.AddCommand(new NotesColorCommand {
                StartValue = Color4.Green,
                EndValue = Color4.Green
            }));
            AddAssert("Note is green", () => note.InnerColor == Color4.Green);
        }

        [Test]
        public void UpdateColor_NotesOutlineColorCommand_IsGreen() {
            GameNote note = null;
            AddStep("Add a note", () => Story.AddNote(note = new GameNote {
                HitTime = Story.Notes.ShowTime - 100
            }));
            AddStep("Apply green NotesOutlineColorCommand", () => Story.AddCommand(new NotesOutlineColorCommand {
                StartValue = Color4.Green,
                EndValue = Color4.Green
            }));
            AddAssert("Note outline is green", () => note.OutlineColor == Color4.Green);
        }

        [Test]
        public void UpdateColor_NotesOutlineThicknessCommand_IsTwoHundredths() {
            GameNote note = null;
            AddStep("Add a note", () => Story.AddNote(note = new GameNote {
                HitTime = Story.Notes.ShowTime - 100
            }));
            AddStep("Apply 0.02 NotesOutlineThicknessCommand", () => Story.AddCommand(new NotesOutlineThicknessCommand {
                StartValue = 0.02f,
                EndValue = 0.02f
            }));
            AddAssert("Note outline is 0.02 thick", () => Precision.AlmostEquals(note.OutlineThickness, 0.02f));
        }
    }
}
