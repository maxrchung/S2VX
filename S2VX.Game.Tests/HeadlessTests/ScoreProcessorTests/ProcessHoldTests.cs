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
    public class ProcessHoldTests : S2VXTestScene {
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

        private void ProcessHold(double scoreTime, bool isPress) =>
            AddStep("Process note", () => GetProcessor().ProcessHold(scoreTime, isPress, 0, 1000));

        [Test]
        public void Process_PressDuring_ColorCursorLate() {
            ProcessHold(500, true);
            AddAssert("Colors cursor late", () => Cursor.ActiveCursor.Colour == Notes.LateColor);
        }

        [Test]
        public void ProcessHold_ReleaseDuring_ColorCursorMiss() {
            ProcessHold(500, false);
            AddAssert("Colors cursor miss", () => Cursor.ActiveCursor.Colour == Notes.MissColor);
        }

        [Test]
        public void ProcessHold_EndWithHold_DoesNotColorCursor() {
            ProcessHold(1001, true);
            AddAssert("Does not color cursor", () => Cursor.ActiveCursor.Colour == Notes.PerfectColor);
        }

        [Test]
        public void ProcessHold_EndWithoutHold_ColorCursorMiss() {
            ProcessHold(1001, false);
            AddAssert("Color cursor miss", () => Cursor.ActiveCursor.Colour == Notes.MissColor);
        }

        [Test]
        public void ProcessHold_PressDuring_PlaysNoSound() {
            ProcessHold(500, true);
            AddAssert("Plays no sound", () => GetProcessor().Hit.PlayCount == 0);
        }

        [Test]
        public void ProcessHold_ReleaseDuring_PlaysMissSound() {
            ProcessHold(500, true);
            AddAssert("Plays miss sound", () => GetProcessor().Miss.PlayCount == 1);
        }

        [Test]
        public void ProcessHold_EndWithHold_PlaysHitSound() {
            ProcessHold(1001, true);
            AddAssert("Plays hit sound", () => GetProcessor().Hit.PlayCount == 1);
        }

        [Test]
        public void ProcessHold_EndWithoutHold_PlaysMissSound() {
            ProcessHold(1001, false);
            AddAssert("Plays miss sound", () => GetProcessor().Miss.PlayCount == 1);
        }
    }
}
