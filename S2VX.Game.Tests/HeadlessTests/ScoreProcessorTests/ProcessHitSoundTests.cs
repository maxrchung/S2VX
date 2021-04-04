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
    public class ProcessHitSoundTests : S2VXTestScene {
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
        public void ProcessHit_PerfectHit_PlaysHitSound() {
            ProcessHit(0);
            AddAssert("Plays hit sound", () => GetProcessor().Hit.PlayCount == 1);
        }

        [Test]
        public void ProcessHit_EarlyHit_PlaysHitSound() {
            ProcessHit(-Notes.PerfectThreshold - 1);
            AddAssert("Plays hit sound", () => GetProcessor().Hit.PlayCount == 1);
        }

        [Test]
        public void ProcessHit_LateHit_PlaysHitSound() {
            ProcessHit(Notes.PerfectThreshold + 1);
            AddAssert("Plays hit sound", () => GetProcessor().Hit.PlayCount == 1);
        }

        [Test]
        public void ProcessHit_EarlyMissHit_PlaysMissSound() {
            ProcessHit(-Notes.HitThreshold - 1);
            AddAssert("Plays miss sound", () => GetProcessor().Miss.PlayCount == 1);
        }

        [Test]
        public void ProcessHit_LateMissHit_PlaysMissSound() {
            ProcessHit(Notes.HitThreshold + 1);
            AddAssert("Plays miss sound", () => GetProcessor().Miss.PlayCount == 1);
        }

        [Test]
        public void ProcessHit_BeforeMissHit_PlaysNoSound() {
            ProcessHit(-Notes.MissThreshold - 1);
            AddAssert("Plays no sound", () => GetProcessor().Miss.PlayCount == 0);
        }

        [Test]
        public void ProcessHit_AfterMissHit_PlaysMissSound() {
            ProcessHit(Notes.MissThreshold + 1);
            AddAssert("Plays miss sound", () => GetProcessor().Miss.PlayCount == 1);
        }
    }
}
