using NUnit.Framework;
using osu.Framework.Bindables;
using osuTK.Input;
using S2VX.Game.Editor.Containers;

namespace S2VX.Game.Tests.VisualTests {
    public class CommandPanelInputBarTests : S2VXTestScene {

        private void HandleTypeSelect(ValueChangedEvent<string> _) { }

        private void HandleAddClick() { }

        private CommandPanelInputBar InputBar { get; set; }

        [SetUp]
        public new void SetUp() => Schedule(() => Add(InputBar = new CommandPanelInputBar(HandleTypeSelect, HandleAddClick)));

        [Test]
        public void Tab_FocusOnFirstInput_ShiftsFocusToNextInput() {
            AddStep("focus start time input", () => InputManager.ChangeFocus(InputBar.TxtStartTime));
            AddStep("press tab", () => InputManager.PressKey(Key.Tab));
            AddAssert("shifts focus to next input", () => InputBar.TxtEndTime.HasFocus);
        }

        [Test]
        public void ShiftTab_FocusOnFirstInput_ShiftsFocusToLastInput() {
            AddStep("focus start time input", () => InputManager.ChangeFocus(InputBar.TxtStartTime));
            AddStep("press shift tab", () => {
                InputManager.PressKey(Key.LShift);
                InputManager.PressKey(Key.Tab);
            });
            AddAssert("shifts focus to last input", () => InputBar.TxtEndValue.HasFocus);
        }
    }
}
