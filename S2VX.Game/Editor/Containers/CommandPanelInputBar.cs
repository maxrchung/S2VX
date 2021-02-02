using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using S2VX.Game.Story.Command;
using System;
using System.Collections.Generic;

namespace S2VX.Game.Editor.Containers {
    public class CommandPanelInputBar : FillFlowContainer {
        private static Vector2 InputSize { get; } = new Vector2(100, 30);

        private Dropdown<string> DropType { get; } = new BasicDropdown<string> { Width = 160 };
        public TextBox TxtStartTime { get; } = new BasicTextBox() { Size = InputSize };
        public TextBox TxtEndTime { get; } = new BasicTextBox() { Size = InputSize };
        public Dropdown<string> DropEasing { get; } = new BasicDropdown<string> { Width = InputSize.X };
        public TextBox TxtStartValue { get; } = new BasicTextBox() { Size = InputSize };
        public TextBox TxtEndValue { get; } = new BasicTextBox() { Size = InputSize };
        public Button BtnAdd { get; } = new IconButton() {
            Width = InputSize.Y,
            Height = InputSize.Y,
            Icon = FontAwesome.Solid.Plus
        };

        private Action<ValueChangedEvent<string>> HandleTypeSelect { get; }

        private Action HandleAddClick { get; }

        public CommandPanelInputBar(Action<ValueChangedEvent<string>> handleTypeSelect, Action handleAddClick) {
            HandleTypeSelect = handleTypeSelect;
            HandleAddClick = handleAddClick;
        }

        [BackgroundDependencyLoader]
        private void Load() {
            AutoSizeAxes = Axes.Both;

            var allCommands = new List<string> {
                "All Commands"
            };
            var allCommandNames = S2VXCommand.GetCommandNames();
            allCommands.AddRange(allCommandNames);
            DropType.Items = allCommands;
            DropEasing.Items = Enum.GetNames(typeof(Easing));

            AddInput("Type", DropType);
            DropType.Current.BindValueChanged(HandleTypeSelect);

            AddTabbableInput("StartTime", TxtStartTime);
            AddTabbableInput("EndTime", TxtEndTime);
            AddTabbableInput("StartValue", TxtStartValue);
            AddTabbableInput("EndValue", TxtEndValue);
            AddInput("Easing", DropEasing);

            AddInput(" ", BtnAdd);
            BtnAdd.Action = HandleAddClick;
        }

        private void AddInput(string text, Drawable input) =>
            AddInternal(
                new FillFlowContainer {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[] {
                        new SpriteText { Text = text },
                        input
                    }
                }
            );

        private void AddTabbableInput(string text, TabbableContainer input) {
            AddInput(text, input);
            input.TabbableContentContainer = this;
        }
    }
}
