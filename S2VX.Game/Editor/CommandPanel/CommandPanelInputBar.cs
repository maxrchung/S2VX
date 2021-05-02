using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;
using S2VX.Game.Story.Command;
using System;
using System.Collections.Generic;

namespace S2VX.Game.Editor.CommandPanel {
    public class CommandPanelInputBar : FillFlowContainer {
        public Dropdown<string> DropType { get; } = new BasicDropdown<string> { Width = 160 };
        public TextBox TxtStartTime { get; } = CreateErrorTextBox();
        public TextBox TxtEndTime { get; } = CreateErrorTextBox();
        public CommandPanelValueInput StartValue { get; } = new();
        public CommandPanelValueInput EndValue { get; } = new();
        public Dropdown<string> DropEasing { get; } = new BasicDropdown<string> { Width = S2VXCommandPanel.InputSize.X };
        public Button BtnSave { get; }

        public static BasicTextBox CreateErrorTextBox() =>
            new() {
                Size = S2VXCommandPanel.InputSize,
                BorderColour = Color4.Red,
                Masking = true
            };

        public static CommandPanelInputBar CreateAddInputBar(Action<ValueChangedEvent<string>> handleTypeSelect, Action handleAddClick) =>
            new(false, handleTypeSelect, handleAddClick);

        public static CommandPanelInputBar CreateEditInputBar(Action handleSaveClick) =>
            new(true, _ => { }, handleSaveClick);

        private CommandPanelInputBar(bool isEditBar, Action<ValueChangedEvent<string>> handleTypeSelect, Action handleCommitClick) {
            var saveIcon = isEditBar ? FontAwesome.Solid.Save : FontAwesome.Solid.Plus;
            BtnSave = new IconButton() {
                Width = S2VXCommandPanel.InputSize.Y,
                Height = S2VXCommandPanel.InputSize.Y,
                Icon = saveIcon
            };

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
            TxtStartTime.BorderThickness = 5;
            TxtEndTime.BorderThickness = 5;
            StartValue.TxtValue.BorderThickness = 5;
            EndValue.TxtValue.BorderThickness = 5;
        }

        public void Reset() {
            TxtStartTime.BorderThickness = 0;
            TxtEndTime.BorderThickness = 0;
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
                $"{TxtStartTime.Current.Value}",
                $"{TxtEndTime.Current.Value}",
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
            TxtStartTime.Text = data[1];
            TxtEndTime.Text = data[2];
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
            AddTabbableInput("StartTime", TxtStartTime);
            AddTabbableInput("EndTime", TxtEndTime);
            AddValueInput("StartValue", StartValue);
            AddValueInput("EndValue", EndValue);
            AddInput("Easing", DropEasing);
            AddInput(" ", BtnSave);
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

        private void AddTabbableInput(string text, TabbableContainer input) {
            AddInput(text, input);
            input.TabbableContentContainer = this;
        }

        private void AddValueInput(string text, CommandPanelValueInput input) {
            AddInput(text, input);
            input.TxtValue.TabbableContentContainer = this;
        }
    }
}
