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
    public class GameHoldNoteTests : S2VXTestScene {
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
            AddAssert("Does not trigger multiple notes", () => PlayScreen.ScoreProcessor.Score == (originalNoteCount - 1) * 100);
        }

        [Test]
        public void UpdateColor_HoldNotesAlphaCommand_IsHalf() {
            GameHoldNote note = null;
            AddStep("Add a note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.ShowTime - 100,
                EndTime = Story.Notes.ShowTime + 100
            }));
            AddStep("Apply half HoldNotesAlphaCommand", () => Story.AddCommand(new HoldNotesAlphaCommand {
                StartValue = 0.5f,
                EndValue = 0.5f
            }));
            AddAssert("Hold note is half alpha", () => Precision.AlmostEquals(note.Alpha, 0.5f));
        }

        [Test]
        public void UpdateColor_HoldNotesColorCommand_IsGreen() {
            GameHoldNote note = null;
            AddStep("Add a note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.ShowTime - 100,
                EndTime = Story.Notes.ShowTime + 100
            }));
            AddStep("Apply green HoldNotesColorCommand", () => Story.AddCommand(new HoldNotesColorCommand {
                StartValue = Color4.Green,
                EndValue = Color4.Green
            }));
            AddAssert("Hold note is green", () => note.InnerColor == Color4.Green);
        }

        [Test]
        public void UpdateColor_HoldNotesOutlineColorCommand_IsGreen() {
            GameHoldNote note = null;
            AddStep("Add a note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.ShowTime - 100,
                EndTime = Story.Notes.ShowTime + 100
            }));
            AddStep("Apply green HoldNotesOutlineColorCommand", () => Story.AddCommand(new HoldNotesOutlineColorCommand {
                StartValue = Color4.Green,
                EndValue = Color4.Green
            }));
            AddAssert("Hold note outline is green", () => note.OutlineColor == Color4.Green);
        }

        [Test]
        public void UpdateSliderPath_HoldNotesOutlineColorCommand_IsGreen() {
            GameHoldNote note = null;
            AddStep("Add a note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.ShowTime - 100,
                EndTime = Story.Notes.ShowTime + 100
            }));
            AddStep("Apply green HoldNotesOutlineColorCommand", () => Story.AddCommand(new HoldNotesOutlineColorCommand {
                StartValue = Color4.Green,
                EndValue = Color4.Green
            }));
            AddAssert("Hold note path is green", () => note.SliderPath.Colour == Color4.Green);
        }

        [Test]
        public void UpdateColor_HoldNotesOutlineThicknessCommand_IsTwoHundredths() {
            GameHoldNote note = null;
            AddStep("Add a note", () => Story.AddNote(note = new GameHoldNote {
                HitTime = Story.Notes.ShowTime - 100,
                EndTime = Story.Notes.ShowTime + 100
            }));
            AddStep("Apply 0.02 HoldNotesOutlineThicknessCommand", () => Story.AddCommand(new HoldNotesOutlineThicknessCommand {
                StartValue = 0.02f,
                EndValue = 0.02f
            }));
            AddAssert("Hold note outline is 0.02 thick", () => Precision.AlmostEquals(note.OutlineThickness, 0.02f));
        }
    }
}
