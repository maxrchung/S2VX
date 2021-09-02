using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osu.Framework.Utils;
using osuTK.Input;
using S2VX.Game.Editor;
using S2VX.Game.Editor.UserInterface;
using S2VX.Game.Story.Command;
using System.Globalization;
using System.IO;

namespace S2VX.Game.Tests.VisualTests.CommandPanelInputBarTests {
    public class ApplyCurrentTimeTests : S2VXTestScene {
        private CommandPanelInputBar InputBar { get; set; }
        private EditorScreen Editor { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            var audioPath = Path.Combine("TestTracks", "10-seconds-of-silence.mp3");
            var track = S2VXTrack.Open(audioPath, audio);

            Add(new ScreenStack(Editor = new(new(), track)));
            Editor.CommandPanel.ToggleVisibility();
        }

        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Restart editor", () => Editor.Restart());
            AddStep("Load a command", () => {
                InputBar = Editor.CommandPanel.AddInputBar;
                InputBar.CommandToValues(new ApproachesDistanceCommand());
            });
        }

        [Test]
        public void AnyCommand_HasBtnApplyCurrentTime() =>
            AddAssert("Time field has an Apply Current Time button", () => InputBar.StartTime.BtnApplyCurrentTime.Alpha == 1);

        [Test]
        public void ClickBtn_AppliesCurrentTime() {
            AddStep("Seek to 100", () => Editor.Seek(100));
            AddStep("Move mouse to button", () => MoveMouseTo(InputBar.StartTime.BtnApplyCurrentTime));
            AddStep("Click button", () => InputManager.PressButton(MouseButton.Left));
            AddStep("Release button", () => InputManager.ReleaseButton(MouseButton.Left));
            AddAssert("Time field is 100", () => Precision.AlmostEquals(
                float.Parse(InputBar.StartTime.TxtValue.Current.Value, CultureInfo.InvariantCulture),
                100));
        }
    }
}
