using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Story;
using System;
using System.Collections.Generic;

namespace S2VX.Game.Editor {
    public class CommandPanel : OverlayContainer {
        [Resolved]
        private S2VXStory Story { get; set; } = null;

        private static Vector2 PanelSize { get; } = new Vector2(727, 727);
        private static Vector2 InputSize { get; } = new Vector2(100, 30);

        private FillFlowContainer InputBar { get; } = new FillFlowContainer { AutoSizeAxes = Axes.Both };
        private Dropdown<string> DropType { get; } = new BasicDropdown<string> {
            Width = 160
        };
        private TextBox TxtStartTime { get; } = new BasicTextBox() { Size = InputSize };
        private TextBox TxtEndTime { get; } = new BasicTextBox() { Size = InputSize };
        private Dropdown<string> DropEasing { get; } = new BasicDropdown<string> { Width = InputSize.X };
        private TextBox TxtStartValue { get; } = new BasicTextBox() { Size = InputSize };
        private TextBox TxtEndValue { get; } = new BasicTextBox() { Size = InputSize };
        private Button BtnAdd { get; } = new BasicButton() {
            Width = InputSize.Y,
            Height = InputSize.Y,
            Text = "+"
        };

        private readonly FillFlowContainer CommandsList = new FillFlowContainer {
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical
        };

        private void AddInput(string text, Drawable input) => InputBar.Add(
                new FillFlowContainer {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new SpriteText { Text = text },
                        input
                    }
                }
            );

        private void LoadCommandsList() {
            CommandsList.Clear();
            var type = DropType.Current.Value;
            for (var i = 0; i < Story.Commands.Count; ++i) {
                var command = Story.Commands[i];
                if (type == "All Commands" || type == command.Type.ToString()) {
                    var localIndex = i;
                    CommandsList.Add(new FillFlowContainer {
                        AutoSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            new BasicButton
                            {
                                Action = () => HandleRemoveClick(localIndex),
                                Width = InputSize.Y,
                                Height = InputSize.Y,
                                Text = "-"
                            },
                            new SpriteText {
                                Text = command.ToString()
                            },
                        }
                    });
                }
            }
        }

        private void HandleAddClick() {
            var data = new string[]
            {
                $"{DropType.Current.Value}",
                $"{TxtStartTime.Current.Value}",
                $"{TxtEndTime.Current.Value}",
                $"{DropEasing.Current.Value}",
                $"{TxtStartValue.Current.Value}",
                $"{TxtEndValue.Current.Value}"
            };
            var join = string.Join("|", data);
            var command = Command.FromString(join);
            Story.AddCommand(command);
            LoadCommandsList();
        }

        private void HandleRemoveClick(int commandIndex) {
            Story.RemoveCommand(commandIndex);
            LoadCommandsList();
        }

        private void HandleTypeSelect(ValueChangedEvent<string> e) => LoadCommandsList();

        [BackgroundDependencyLoader]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Load() {
            Anchor = Anchor.TopRight;
            Origin = Anchor.TopRight;
            Size = PanelSize;

            var allCommands = new List<string> {
                "All Commands"
            };
            allCommands.AddRange(Enum.GetNames(typeof(CommandType)));
            DropType.Items = allCommands;
            DropEasing.Items = Enum.GetNames(typeof(Easing));

            AddInput("Type", DropType);
            DropType.Current.BindValueChanged(HandleTypeSelect);

            AddInput("StartTime", TxtStartTime);
            AddInput("EndTime", TxtEndTime);
            AddInput("Easing", DropEasing);
            AddInput("StartValue", TxtStartValue);
            AddInput("EndValue", TxtEndValue);

            AddInput(" ", BtnAdd);
            BtnAdd.Action = HandleAddClick;

            LoadCommandsList();

            Children = new Drawable[]
            {
                new RelativeBox { Colour = Color4.Black.Opacity(0.9f) },
                new BasicScrollContainer
                {
                    // This is so mega fucked?
                    Position = new Vector2(0, 70),
                    Width = PanelSize.X,
                    Height = PanelSize.Y - 70,
                    Child = CommandsList
                },
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new SpriteText { Text = "Command Panel" },
                        InputBar
                    }
                }
            };
        }

        protected override void PopIn() => Alpha = 1;

        protected override void PopOut() => Alpha = 0;
    }
}
