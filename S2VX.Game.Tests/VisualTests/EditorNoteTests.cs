using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osu.Framework.Timing;
using osu.Framework.Utils;
using osuTK.Graphics;
using S2VX.Game.Editor;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;
using S2VX.Game.Story.Note;
using System.IO;

namespace S2VX.Game.Tests.VisualTests {
    public class EditorNoteTests : S2VXTestScene {
        [Cached]
        private S2VXStory Story { get; set; } = new S2VXStory();
#pragma warning disable IDE0052 // Remove unread private members
        private EditorScreen Editor { get; set; }
#pragma warning restore IDE0052 // Remove unread private members

        [Resolved]
        private AudioManager Audio { get; set; }

        private EditorNote NoteToTest { get; set; }
        private StopwatchClock StoryClock { get; set; }
        private readonly float NoteAppearTime = 1000.0f;

        [BackgroundDependencyLoader]
        private void Load() {
            StoryClock = new StopwatchClock();
            Story.Clock = new FramedClock(StoryClock);
            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            Add(new ScreenStack(Editor = new EditorScreen(Story, S2VXTrack.Open(audioPath, Audio))));
        }

        // All tests will have a note that starts to appear in 1 second
        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Reset story", () => Story.Reset());
            AddStep("Reset clock", () => Story.Clock = new FramedClock(StoryClock = new StopwatchClock()));
            AddStep("Add note", () => Story.AddNote(NoteToTest = new EditorNote {
                HitTime = Story.Notes.ShowTime + Story.Notes.FadeInTime + NoteAppearTime
            }));
        }

        [Test]
        public void Hit_BeforeHitTime_DoesNotPlay() =>
            AddAssert("Does not play", () => NoteToTest.Hit.PlayCount == 0);

        [Test]
        public void Hit_AfterHitTime_PlaysOnce() {
            AddStep("Start clock", () => StoryClock.Start());
            AddUntilStep("Play until after hit time", () => StoryClock.CurrentTime > NoteToTest.HitTime);
            AddAssert("Plays once", () => NoteToTest.Hit.PlayCount == 1);
        }

        [Test]
        public void UpdateColor_ApproachesColorCommand_IsGreen() {
            EditorNote note = null;
            AddStep("Add a note", () => Story.AddNote(note = new EditorNote { HitTime = 1000 }));
            AddStep("Apply Green ApproachesColorCommand", () => Story.AddCommand(new ApproachesColorCommand {
                StartValue = Color4.Green,
                EndValue = Color4.Green
            }));
            AddAssert("Approach is green", () => note.Approach.Colour == Color4.Green);
        }

        [Test]
        public void UpdateColor_NotesAlphaCommand_IsHalf() {
            EditorNote note = null;
            AddStep("Add a note", () => Story.AddNote(note = new EditorNote {
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
            EditorNote note = null;
            AddStep("Add a note", () => Story.AddNote(note = new EditorNote {
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
            EditorNote note = null;
            AddStep("Add a note", () => Story.AddNote(note = new EditorNote {
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
            EditorNote note = null;
            AddStep("Add a note", () => Story.AddNote(note = new EditorNote {
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
