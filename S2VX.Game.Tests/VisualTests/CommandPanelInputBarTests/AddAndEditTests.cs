using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK.Input;
using S2VX.Game.Editor.UserInterface;
using S2VX.Game.Story.Command;

namespace S2VX.Game.Tests.VisualTests.CommandPanelInputBarTests {
    public class AddAndEditTests : S2VXTestScene {

        private CommandPanelInputBar InputBar { get; set; }

        [SetUpSteps]
        public void SetUpSteps() => AddStep("Clear drawables", () => Clear());

        private void CreateAddInputBar() =>
            AddStep("Create add input bar", () => Add(InputBar = CommandPanelInputBar.CreateAddInputBar(null, null, () => 0)));

        private void CreateEditInputBar() =>
            AddStep("Create edit input bar", () => Add(InputBar = CommandPanelInputBar.CreateEditInputBar(null, () => 0)));

        [Test]
        public void Tab_FocusOnFirstInput_ShiftsFocusToNextInput() {
            CreateAddInputBar();
            AddStep("Focus start time input", () => InputManager.ChangeFocus(InputBar.StartTime.TxtValue));
            AddStep("Press tab", () => InputManager.PressKey(Key.Tab));
            AddStep("Release tab", () => InputManager.ReleaseKey(Key.Tab));
            AddAssert("Shifts focus to next input", () => InputBar.StartValue.TxtValue.HasFocus);
        }

        [Test]
        public void ShiftTab_FocusOnFirstInput_ShiftsFocusToLastInput() {
            CreateAddInputBar();
            AddStep("Focus start time input", () => InputManager.ChangeFocus(InputBar.StartTime.TxtValue));
            AddStep("Press shift tab", () => {
                InputManager.PressKey(Key.LShift);
                InputManager.PressKey(Key.Tab);
            });
            AddStep("Release shift tab", () => {
                InputManager.ReleaseKey(Key.LShift);
                InputManager.ReleaseKey(Key.Tab);
            });
            AddAssert("Shifts focus to last input", () => InputBar.EndValue.TxtValue.HasFocus);
        }

        [Test]
        public void CreateAddInputBar_AddInputBar_HasPlusButton() {
            CreateAddInputBar();
            AddAssert("Has plus button", () => (InputBar.BtnSave as IconButton).Icon.Equals(FontAwesome.Solid.Plus));
        }

        [Test]
        public void CreateEditInputBar_EditInputBar_HasSaveButton() {
            CreateEditInputBar();
            AddAssert("Has save button", () => (InputBar.BtnSave as IconButton).Icon.Equals(FontAwesome.Solid.Save));
        }

        [Test]
        public void ValuesToString_UpdatedValues_CreatesCorrectString() {
            CreateAddInputBar();
            AddStep("Update command type", () => InputBar.DropType.Current.Value = new GridAlphaCommand().GetCommandName());
            AddStep("Update start time", () => InputBar.StartTime.TxtValue.Current.Value = "123");
            AddStep("Update end time", () => InputBar.EndTime.TxtValue.Current.Value = "456789");
            AddStep("Update start value", () => InputBar.StartValue.TxtValue.Current.Value = "0.1");
            AddStep("Update end value", () => InputBar.EndValue.TxtValue.Current.Value = "1");
            AddStep("Update easing", () => InputBar.DropEasing.Current.Value = Easing.OutQuint.ToString());
            AddAssert("Creates correct string", () => InputBar.ValuesToString() == "GridAlpha|123|0.1|456789|1|OutQuint");
        }

        private void SetUpCommandToValues() {
            CreateAddInputBar();
            var command = new HoldNotesAlphaCommand() {
                StartTime = 999,
                EndTime = 999,
                StartValue = 0.999f,
                EndValue = 0.001f,
                Easing = Easing.InOutQuad
            };
            AddStep("Update input bar", () => InputBar.CommandToValues(command));
        }

        [Test]
        public void CommandToValues_GivenCommand_HasCorrectCommandType() {
            SetUpCommandToValues();
            AddAssert("Has correct command type", () => InputBar.DropType.Current.Value == "HoldNotesAlpha");
        }

        [Test]
        public void CommandToValues_GivenCommand_HasCorrectStartTime() {
            SetUpCommandToValues();
            AddAssert("Has correct start time", () => InputBar.StartTime.TxtValue.Current.Value == "999");
        }

        [Test]
        public void CommandToValues_GivenCommand_HasCorrectEndTime() {
            SetUpCommandToValues();
            AddAssert("Has correct end time", () => InputBar.EndTime.TxtValue.Current.Value == "999");
        }

        [Test]
        public void CommandToValues_GivenCommand_HasCorrectStartValue() {
            SetUpCommandToValues();
            AddAssert("Has correct start value", () => InputBar.StartValue.TxtValue.Current.Value == "0.999");
        }

        [Test]
        public void CommandToValues_GivenCommand_HasCorrectEndValue() {
            SetUpCommandToValues();
            AddAssert("Has correct end value", () => InputBar.EndValue.TxtValue.Current.Value == "0.001");
        }

        [Test]
        public void CommandToValues_GivenCommand_HasCorrectEasing() {
            SetUpCommandToValues();
            AddAssert("Has correct easing", () => InputBar.DropEasing.Current.Value == Easing.InOutQuad.ToString());
        }

        [Test]
        public void CommandToValues_GivenCommand_HasCorrectString() {
            SetUpCommandToValues();
            AddAssert("Has correct string", () => InputBar.ValuesToString() == "HoldNotesAlpha|999|0.999|999|0.001|InOutQuad");
        }
    }
}
