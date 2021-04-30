using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;
using S2VX.Game.Editor.ColorPicker;
using S2VX.Game.Story.Command;
using System;
using System.Collections.Generic;

namespace S2VX.Game.Editor.Containers {
    public class CommandPanelInputBar : FillFlowContainer {
        public Dropdown<string> DropType { get; } = new BasicDropdown<string> { Width = 160 };
        public TextBox TxtStartTime { get; } = CreateErrorTextBox();
        public TextBox TxtEndTime { get; } = CreateErrorTextBox();
        public TextBox TxtStartValue { get; } = CreateErrorTextBox();
        public FillFlowContainer StartColorContainer { get; private set; }
        public S2VXColorPicker StartColorPicker { get; } = new S2VXColorPicker();
        public TextBox TxtEndValue { get; } = CreateErrorTextBox();
        public FillFlowContainer EndColorContainer { get; private set; }
        public S2VXColorPicker EndColorPicker { get; } = new S2VXColorPicker();
        public Dropdown<string> DropEasing { get; } = new BasicDropdown<string> { Width = CommandPanel.InputSize.X };
        public Button BtnSave { get; }
        private Action<ValueChangedEvent<string>> HandleTypeSelect { get; }

        private static TextBox CreateErrorTextBox() =>
            new BasicTextBox() {
                Size = CommandPanel.InputSize,
                BorderColour = Color4.Red,
                Masking = true
            };

        public static CommandPanelInputBar CreateAddInputBar(Action<ValueChangedEvent<string>> handleTypeSelect, Action handleAddClick) =>
            new(false, handleTypeSelect, handleAddClick);

        public static CommandPanelInputBar CreateEditInputBar(Action handleSaveClick) =>
            new(true, _ => { }, handleSaveClick);

        private CommandPanelInputBar(bool isEditBar, Action<ValueChangedEvent<string>> handleTypeSelect, Action handleSaveClick) {
            var saveIcon = isEditBar ? FontAwesome.Solid.Save : FontAwesome.Solid.Plus;
            BtnSave = new IconButton() {
                Width = CommandPanel.InputSize.Y,
                Height = CommandPanel.InputSize.Y,
                Icon = saveIcon
            };
            HandleTypeSelect = handleTypeSelect;

            // We initialize the inputs here instead of in Load because the
            // outer CommandPanel needs some of these values to be set
            var allCommands = new List<string> {
                "All Commands"
            };
            var allCommandNames = S2VXCommand.GetCommandNames();
            allCommands.AddRange(allCommandNames);
            DropType.Items = allCommands;
            DropEasing.Items = Enum.GetNames(typeof(Easing));
            DropType.Current.BindValueChanged(HandleTypeSelect);
            BtnSave.Action = handleSaveClick;
        }

        public void AddErrorIndicator() {
            TxtStartTime.BorderThickness = 5;
            TxtEndTime.BorderThickness = 5;
            TxtStartValue.BorderThickness = 5;
            TxtEndValue.BorderThickness = 5;
        }

        public void ClearErrorIndicator() {
            TxtStartTime.BorderThickness = 0;
            TxtEndTime.BorderThickness = 0;
            TxtStartValue.BorderThickness = 0;
            TxtEndValue.BorderThickness = 0;
        }

        public string ValuesToString() {
            var data = new string[]
            {
                $"{DropType.Current.Value}",
                $"{TxtStartTime.Current.Value}",
                $"{TxtEndTime.Current.Value}",
                $"{DropEasing.Current.Value}",
                $"{TxtStartValue.Current.Value}",
                $"{TxtEndValue.Current.Value}"
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
            TxtStartValue.Text = data[4];
            TxtEndValue.Text = data[5];
        }

        [BackgroundDependencyLoader]
        private void Load() {
            AutoSizeAxes = Axes.X;
            Height = CommandPanel.InputBarHeight;

            AddInput("Type", DropType);
            AddTabbableInput("StartTime", TxtStartTime);
            AddTabbableInput("EndTime", TxtEndTime);

            StartColorContainer = CreateColorContainer(StartColorPicker);
            AddValueInput("EndValue", TxtEndValue, StartColorContainer);

            EndColorContainer = CreateColorContainer(EndColorPicker);
            AddValueInput("EndValue", TxtEndValue, EndColorContainer);

            AddInput("Easing", DropEasing);
            AddInput(" ", BtnSave);

            TxtEndValue.OnPressed(_ => Console.WriteLine("hello"));
        }

        private static FillFlowContainer CreateColorContainer(S2VXColorPicker colorPicker) =>
            new() {
                Direction = FillDirection.Vertical,
                Children = new Drawable[] {
                    colorPicker,
                    new BasicButton {
                        Text = "OK",
                        Action = () => colorPicker.Hide(),
                        Size = CommandPanel.InputSize
                    }
                }
            };

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

        private void AddValueInput(string text, TabbableContainer input, FillFlowContainer colorContainer) {
            Add(new FillFlowContainer {
                Width = CommandPanel.InputSize.X,
                Direction = FillDirection.Vertical,
                Children = new Drawable[] {
                    new SpriteText { Text = text },
                    input,
                    colorContainer
                }
            });
            input.TabbableContentContainer = this;
        }
    }
}
