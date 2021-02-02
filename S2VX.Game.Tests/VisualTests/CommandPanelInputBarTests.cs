using NUnit.Framework;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osuTK.Input;
using S2VX.Game.Editor.Containers;

namespace S2VX.Game.Tests.VisualTests {
    public class CommandPanelInputBarTests : S2VXTestScene {

        private static void ValueChangedHandler(ValueChangedEvent<string> _) { }

        private static void EmptyHandler() { }

        private CommandPanelInputBar InputBar { get; set; }

        private void CreateAddInputBar() =>
            AddStep("create add input bar", () => Add(InputBar = CommandPanelInputBar.CreateAddInputBar(ValueChangedHandler, EmptyHandler)));

        private void CreateEditInputBar() =>
            AddStep("create edit input bar", () => Add(InputBar = CommandPanelInputBar.CreateEditInputBar(EmptyHandler)));

        [Test]
        public void Tab_FocusOnFirstInput_ShiftsFocusToNextInput() {
            CreateAddInputBar();
            AddStep("focus start time input", () => InputManager.ChangeFocus(InputBar.TxtStartTime));
            AddStep("press tab", () => InputManager.PressKey(Key.Tab));
            AddAssert("shifts focus to next input", () => InputBar.TxtEndTime.HasFocus);
        }

        [Test]
        public void ShiftTab_FocusOnFirstInput_ShiftsFocusToLastInput() {
            CreateAddInputBar();
            AddStep("focus start time input", () => InputManager.ChangeFocus(InputBar.TxtStartTime));
            AddStep("press shift tab", () => {
                InputManager.PressKey(Key.LShift);
                InputManager.PressKey(Key.Tab);
            });
            AddAssert("shifts focus to last input", () => InputBar.TxtEndValue.HasFocus);
        }

        [Test]
        public void SaveIcon_AddInputBar_IsPlus() {
            CreateAddInputBar();
            IconButton iconButton = null;
            AddStep("set icon button", () => iconButton = InputBar.BtnSave as IconButton);
            AddAssert("is plus", () => iconButton.Icon.Equals(FontAwesome.Solid.Plus));
        }

        [Test]
        public void SaveIcon_EditInputBar_IsSave() {
            CreateEditInputBar();
            IconButton iconButton = null;
            AddStep("set icon button", () => iconButton = InputBar.BtnSave as IconButton);
            AddAssert("is save", () => iconButton.Icon.Equals(FontAwesome.Solid.Save));
        }
    }
}
