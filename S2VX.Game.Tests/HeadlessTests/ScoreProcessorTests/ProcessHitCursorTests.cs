using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using S2VX.Game.Play;
using S2VX.Game.Play.UserInterface;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;
using System.IO;

namespace S2VX.Game.Tests.HeadlessTests.ScoreProcessorTests {
    [HeadlessTest]
    public class ProcessHitCursorTests : S2VXTestScene {
        [Resolved]
        private S2VXCursor Cursor { get; set; }

        private PlayScreen PlayScreen { get; set; }
        private Notes Notes { get; set; }
        private ScoreProcessor GetProcessor() => PlayScreen.ScoreProcessor;

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            var story = new S2VXStory();
            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            Add(new ScreenStack(PlayScreen = new PlayScreen(false, story, S2VXTrack.Open(audioPath, audio))));
            Notes = story.Notes;
        }

        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Reset cursor", () => Cursor.Reset());
            AddStep("Reset score processor", () => GetProcessor().Reset());
        }

        private void ProcessHit(double scoreTime) =>
            AddStep("Process note", () => GetProcessor().ProcessHit(scoreTime, 0));

        [Test]
        public void ProcessHit_PerfectHit_ColorsCursorPerfect() {
            ProcessHit(0);
            AddAssert("Colors cursor perfect", () => Cursor.ActiveCursor.Colour == Notes.PerfectColor);
        }

        [Test]
        public void ProcessHit_EarlyHit_ColorsCursorEarly() {
            ProcessHit(-Notes.PerfectThreshold - 1);
            AddAssert("Colors cursor early", () => Cursor.ActiveCursor.Colour == Notes.EarlyColor);
        }

        [Test]
        public void ProcessHit_LateHit_ColorsCursorLate() {
            ProcessHit(Notes.PerfectThreshold + 1);
            AddAssert("Colors cursor late", () => Cursor.ActiveCursor.Colour == Notes.LateColor);
        }

        [Test]
        public void ProcessHit_EarlyMissHit_ColorsCursorMiss() {
            ProcessHit(-Notes.HitThreshold - 1);
            AddAssert("Colors cursor miss", () => Cursor.ActiveCursor.Colour == Notes.MissColor);
        }

        [Test]
        public void ProcessHit_LateMissHit_ColorsCursorMiss() {
            ProcessHit(Notes.HitThreshold + 1);
            AddAssert("Colors cursor miss", () => Cursor.ActiveCursor.Colour == Notes.MissColor);
        }

        [Test]
        public void ProcessHit_BeforeMissHit_DoesNotColorCursor() {
            ProcessHit(-Notes.MissThreshold - 1);
            AddAssert("Does not color cursor", () => Cursor.ActiveCursor.Colour == Notes.PerfectColor);
        }

        [Test]
        public void ProcessHit_AfterMissHit_ColorsCursorMiss() {
            ProcessHit(Notes.MissThreshold + 1);
            AddAssert("Colors cursor miss", () => Cursor.ActiveCursor.Colour == Notes.MissColor);
        }
    }
}
