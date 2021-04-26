using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using S2VX.Game.Editor;
using S2VX.Game.Story;
using System.IO;

namespace S2VX.Game.Tests.VisualTests {
    public class EditorApproachRateDisplayTests : S2VXTestScene {
        private EditorScreen Editor { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            var track = S2VXTrack.Open(audioPath, audio);

            Add(new ScreenStack(Editor = new EditorScreen(new S2VXStory(), track)));
        }

        [SetUpSteps]

        public void SetUpSteps() {
            AddStep("Pause editor", () => Editor.Play(false));
            AddStep("Restart editor", () => Editor.Restart());
            AddStep("Reload editor settings", () => Editor.LoadEditorSettings());
        }

        [Test]
        public void EditorApproachRateDisplay_ScrollWheelDown_ApproachRateIsOne() {
            AddStep("Move mouse over approach rate display", () => InputManager.MoveMouseTo(Editor.EditorInfoBar.ApproachRateDisplay));
            AddStep("Scroll wheel down", () => InputManager.ScrollVerticalBy(-1));
            AddAssert("Editor approach rate is 1", () => Editor.EditorApproachRate == 1);
        }

        [Test]
        public void EditorApproachRateDisplay_ScrollWheelUp_ApproachRateIsFour() {
            AddStep("Move mouse over approach rate display", () => InputManager.MoveMouseTo(Editor.EditorInfoBar.ApproachRateDisplay));
            AddStep("Scroll wheel up", () => InputManager.ScrollVerticalBy(1));
            AddAssert("Editor approach rate is 4", () => Editor.EditorApproachRate == 4);
        }

        [Test]
        public void EditorApproachRateDisplay_ScrollWheelUpTwice_ApproachRateIsEight() {
            AddStep("Move mouse over approach rate display", () => InputManager.MoveMouseTo(Editor.EditorInfoBar.ApproachRateDisplay));
            AddStep("Scroll wheel up", () => InputManager.ScrollVerticalBy(1));
            AddStep("Scroll wheel up", () => InputManager.ScrollVerticalBy(1));
            AddAssert("Editor approach rate is 8", () => Editor.EditorApproachRate == 8);
        }

        [Test]
        public void EditorApproachRateDisplay_ScrollWheelUpThreeTimes_ApproachRateIsStillEight() {
            AddStep("Move mouse over approach rate display", () => InputManager.MoveMouseTo(Editor.EditorInfoBar.ApproachRateDisplay));
            AddStep("Scroll wheel up", () => InputManager.ScrollVerticalBy(1));
            AddStep("Scroll wheel up", () => InputManager.ScrollVerticalBy(1));
            AddStep("Scroll wheel up", () => InputManager.ScrollVerticalBy(1));
            AddAssert("Editor approach rate is 8", () => Editor.EditorApproachRate == 8);
        }

        [Test]
        public void EditorApproachRateDisplay_ScrollWheelDownTwoTimes_ApproachRateIsStillOne() {
            AddStep("Move mouse over approach rate display", () => InputManager.MoveMouseTo(Editor.EditorInfoBar.ApproachRateDisplay));
            AddStep("Scroll wheel down", () => InputManager.ScrollVerticalBy(-1));
            AddStep("Scroll wheel down", () => InputManager.ScrollVerticalBy(-1));
            AddAssert("Editor approach rate is 1", () => Editor.EditorApproachRate == 1);
        }
    }
}
