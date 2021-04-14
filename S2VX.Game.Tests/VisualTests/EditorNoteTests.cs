using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using osu.Framework.Timing;
using osuTK.Graphics;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;
using S2VX.Game.Story.Note;

namespace S2VX.Game.Tests.VisualTests {
    public class EditorNoteTests : S2VXTestScene {
        [Cached]
        private S2VXStory Story { get; set; } = new S2VXStory();

        private EditorNote NoteToTest { get; set; }
        private StopwatchClock StoryClock { get; set; }
        private readonly float NoteAppearTime = 1000.0f;

        [BackgroundDependencyLoader]
        private void Load() {
            StoryClock = new StopwatchClock();
            Story.Clock = new FramedClock(StoryClock);
            Add(Story);
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
    }
}
