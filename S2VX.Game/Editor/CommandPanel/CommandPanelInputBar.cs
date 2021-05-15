using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using S2VX.Game.Story.Command;
using System;
using System.Collections.Generic;

namespace S2VX.Game.Editor.CommandPanel {
    public class CommandPanelInputBar : FillFlowContainer {
        public Dropdown<string> DropType { get; } = new BasicDropdown<string> { Width = 160 };
        public CommandPanelValueInput StartTime { get; }
        public CommandPanelValueInput EndTime { get; }
        public CommandPanelValueInput StartValue { get; } = new();
        public CommandPanelValueInput EndValue { get; } = new();
        public Dropdown<string> DropEasing { get; } = new BasicDropdown<string> { Width = S2VXCommandPanel.InputSize.X };
        public Button BtnSave { get; }

        public static CommandPanelInputBar CreateAddInputBar(Action<ValueChangedEvent<string>> handleTypeSelect, Action handleAddClick,
            Func<double> currentTimeDelegate) =>
            new(false, handleTypeSelect, handleAddClick, currentTimeDelegate);

        public static CommandPanelInputBar CreateEditInputBar(Action handleSaveClick, Func<double> currentTimeDelegate) =>
            new(true, _ => { }, handleSaveClick, currentTimeDelegate);

        private CommandPanelInputBar(bool isEditBar, Action<ValueChangedEvent<string>> handleTypeSelect, Action handleCommitClick,
            Func<double> currentTimeDelegate) {
            var saveIcon = isEditBar ? FontAwesome.Solid.Save : FontAwesome.Solid.Plus;
            BtnSave = new IconButton() {
                Width = S2VXCommandPanel.InputSize.Y,
                Height = S2VXCommandPanel.InputSize.Y,
                Icon = saveIcon
            };
            StartTime = new(currentTimeDelegate);
            EndTime = new(currentTimeDelegate);

            // We initialize the inputs here instead of in Load because the
            // outer CommandPanel needs some of these values to be set
            var allCommands = new List<string> {
                "All Commands"
            };
            var allCommandNames = S2VXCommand.GetCommandNames();
            allCommands.AddRange(allCommandNames);
            DropType.Items = allCommands;
            DropEasing.Items = Enum.GetNames(typeof(Easing));
            DropType.Current.BindValueChanged(handleTypeSelect);
            BtnSave.Action = handleCommitClick;
        }

        public void AddErrorIndicator() {
            StartTime.BorderThickness = 5;
            EndTime.BorderThickness = 5;
            StartValue.TxtValue.BorderThickness = 5;
            EndValue.TxtValue.BorderThickness = 5;
        }

        public void Reset() {
            StartTime.BorderThickness = 0;
            EndTime.BorderThickness = 0;
            StartValue.TxtValue.BorderThickness = 0;
            EndValue.TxtValue.BorderThickness = 0;

            // Checks if we have to re-enable color picker toggle
            var isColorValue = DropType.Current.Value.Contains("Color", StringComparison.Ordinal);
            StartValue.UseColorPicker(isColorValue);
            EndValue.UseColorPicker(isColorValue);
        }

        public string ValuesToString() {
            var data = new string[] {
                $"{DropType.Current.Value}",
                $"{StartTime.TxtValue.Current.Value}",
                $"{EndTime.TxtValue.Current.Value}",
                $"{DropEasing.Current.Value}",
                $"{StartValue.TxtValue.Current.Value}",
                $"{EndValue.TxtValue.Current.Value}"
            };
            var commandString = string.Join("|", data);
            return commandString;
        }

        public void CommandToValues(S2VXCommand command) {
            var data = command.ToString().Split('{', '|', '}');
            DropType.Current.Value = data[0];
            StartTime.TxtValue.Text = data[1];
            EndTime.TxtValue.Text = data[2];
            DropEasing.Current.Value = data[3];
            StartValue.TxtValue.Text = data[4];
            EndValue.TxtValue.Text = data[5];
            Reset();
        }

        [BackgroundDependencyLoader]
        private void Load() {
            AutoSizeAxes = Axes.X;
            Height = S2VXCommandPanel.InputBarHeight;

            AddInput("Type", DropType);
            AddTextInput("StartTime", StartTime);
            AddTextInput("StartValue", StartValue);
            AddTextInput("EndTime", EndTime);
            AddTextInput("EndValue", EndValue);
            AddInput("Easing", DropEasing);
            AddInput(" ", BtnSave);
            StartTime.UseApplyCurrentTime();
            EndTime.UseApplyCurrentTime();
        }

        private void AddInput(string text, Drawable input) =>
            Add(new FillFlowContainer {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Children = new Drawable[] {
                    new SpriteText { Text = text },
                    input
                }
            });

        private void AddTextInput(string text, CommandPanelValueInput input) {
            AddInput(text, input);
            input.TxtValue.TabbableContentContainer = this;
        }
    }
}
