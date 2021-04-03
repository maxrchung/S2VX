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
    public class GameNoteTests : S2VXTestScene {
        [Resolved]
        private S2VXCursor Cursor { get; set; }

        private S2VXStory Story { get; set; } = new S2VXStory();
        private PlayScreen PlayScreen { get; set; }
        private ScoreProcessor GetProcessor() => PlayScreen.ScoreProcessor;

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            Add(new ScreenStack(PlayScreen = new PlayScreen(false, Story, S2VXTrack.Open(audioPath, audio))));
        }

        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Reset cursor", () => Cursor.Reset());
            AddStep("Reset score processor", () => GetProcessor().Reset());
        }

        private void Process(double time) =>
            AddStep("Process note", () => GetProcessor().Process(time,new GameNote()));

        [Test]
        public void Process_PerfectHit_ColorsCursorPerfect() {
            Process(0);
            AddAssert("Colors cursor perfect", () => Cursor.ActiveCursor.Colour == Story.Notes.PerfectColor);
        }

        [Test]
        public void Process_EarlyHit_ColorsCursorEarly() {
            Process(-Story.Notes.PerfectThreshold - 1);
            AddAssert("Colors cursor early", () => Cursor.ActiveCursor.Colour == Story.Notes.EarlyColor);
        }

        [Test]
        public void Process_LateHit_ColorsCursorLate() {
            Process(Story.Notes.PerfectThreshold + 1);
            AddAssert("Colors cursor late", () => Cursor.ActiveCursor.Colour == Story.Notes.LateColor);
        }

        [Test]
        public void Process_EarlyMissHit_ColorsCursorMiss() {
            Process(-Story.Notes.HitThreshold - 1);
        }

        [Test]
        public void Process_LateMissHit_ColorsCursorMiss() {
            Process(Story.Notes.HitThreshold + 1);
        }

        [Test]
        public void Process_BeforeMissHit_DoesNotColorCursor() {
        }

        [Test]
        public void Process_AfterMissHit_ColorsCursorMiss() {
        }



        [Test]
        public void Process_PerfectHit_PlaysHitSound() {
            Process(0);
            AddAssert("Plays hit sound", () => GetProcessor().Hit.PlayCount == 1);
        }

        [Test]
        public void Process_EarlyHit_PlaysHitSound() {
        }

        [Test]
        public void Process_LateHit_PlaysHitSound() {
        }

        [Test]
        public void Process_EarlyMissHit_PlaysMissSound() {
        }

        [Test]
        public void Process_LateMissHit_PlaysMissSound() {
        }

        [Test]
        public void Process_BeforeMissHit_PlaysNoSound() {
        }

        [Test]
        public void Process_AfterMissHit_PlaysMissSound() {
        }
    }
}
