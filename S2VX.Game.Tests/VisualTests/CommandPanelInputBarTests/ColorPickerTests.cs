using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK.Graphics;
using S2VX.Game.Editor.CommandPanel;
using S2VX.Game.Story.Command;

namespace S2VX.Game.Tests.VisualTests.CommandPanelInputBarTests {
    public class ColorPickerTests : S2VXTestScene {
        private CommandPanelInputBar InputBar { get; set; }

        [SetUpSteps]
        public void SetUpSteps() {
            AddStep("Clear drawables", () => Clear());
            AddStep("Add input bar", () => Add(InputBar = CommandPanelInputBar.CreateAddInputBar(_ => { }, () => { })));
        }

        [Test]
        public void UseColorPicker_NonColorCommand_DoesNotUseColorPicker() {
            AddStep("Load non-color command", () => InputBar.CommandToValues(new ApproachesDistanceCommand()));
            AddAssert("Does not use color picker", () => InputBar.StartValue.BtnToggle.Alpha == 0);
        }

        [Test]
        public void UseColorPicker_ColorCommandClick_UsesColorPicker() {
            AddStep("Load color command", () => InputBar.CommandToValues(new ApproachesColorCommand()));
            AddAssert("Uses color picker", () => InputBar.StartValue.BtnToggle.Alpha == 1);
        }

        [Test]
        public void BindTxtValueChange_LimeColor_SetsColorPickerToLime() {
            AddStep("Load color command", () => InputBar.CommandToValues(new NotesColorCommand()));
            AddStep("Set text value to lime", () => InputBar.StartValue.TxtValue.Text = "(0,1,0)");
            AddAssert("Sets color picker to lime", () => InputBar.StartValue.ColorPicker.Current.Value == Color4.Lime);
        }

        [Test]
        public void BindColorPickerChange_RedColor_SetsTxtValueToRed() {
            AddStep("Load color command", () => InputBar.CommandToValues(new BackgroundColorCommand()));
            AddStep("Set color picker to red", () => InputBar.StartValue.ColorPicker.Current.Value = Color4.Red);
            AddAssert("Sets text value to red", () => InputBar.StartValue.TxtValue.Text == "(1,0,0)");
        }

        [Test]
        public void ValuesToString_BlueStartAndEndColor_CreatesCorrectString() {
            AddStep("Update command type", () => InputBar.DropType.Current.Value = new GridColorCommand().GetCommandName());
            AddStep("Update start time", () => InputBar.StartTime.TxtValue.Current.Value = "12");
            AddStep("Update end time", () => InputBar.EndTime.TxtValue.Current.Value = "345");
            AddStep("Update start value", () => InputBar.StartValue.ColorPicker.Current.Value = Color4.Blue);
            AddStep("Update end value", () => InputBar.EndValue.ColorPicker.Current.Value = Color4.Blue);
            AddStep("Update easing", () => InputBar.DropEasing.Current.Value = Easing.OutBounce.ToString());
            AddAssert("Creates correct string", () => InputBar.ValuesToString() == "GridColor|12|345|OutBounce|(0,0,1)|(0,0,1)");
        }
    }
}
