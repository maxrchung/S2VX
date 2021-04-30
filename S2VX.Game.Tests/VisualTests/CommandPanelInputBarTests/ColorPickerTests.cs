﻿using NUnit.Framework;
using osu.Framework.Testing;
using S2VX.Game.Editor.Containers;
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
        public void ShowColorPicker_NonColorCommand_DoesNotShowColorPicker() {
            AddStep("Load non-color command", () => InputBar.CommandToValues(new ApproachesDistanceCommand()));
            AddAssert("Does not show color picker", () => InputBar.StartColorPicker.Alpha == 0);
        }

        [Test]
        public void ShowColorPicker_ColorCommandClick_ShowsColorPicker() {
        }

        [Test]
        public void SelectColor_RedColor_SavesRedColor() {
        }


        [Test]
        public void ValuesToString_RedStartColor_ContainsRedStartColor() {
        }

        [Test]
        public void ValuesToString_GreenEndColor_ContainsGreenEndColor() {
        }
    }
}
