using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game
{
    public class CommandPanel : OverlayContainer
    {
        [Resolved]
        private Story story { get; set;  } = new Story();

        private static Vector2 panelSize { get; } = new Vector2(727, 727);
        private static Vector2 inputSize { get; } = new Vector2(100, 30);

        private FillFlowContainer inputBar { get; } = new FillFlowContainer { AutoSizeAxes = Axes.Both };
        private Dropdown<string> dropType { get; }  = new BasicDropdown<string>
        {
            Width = 160
        };
        private TextBox txtStartTime { get; } = new BasicTextBox() { Size = inputSize };
        private TextBox txtEndTime { get; } = new BasicTextBox() { Size = inputSize };
        private Dropdown<string> dropEasing { get; } = new BasicDropdown<string> { Width = inputSize.X };
        private TextBox txtStartValue { get; } = new BasicTextBox() { Size = inputSize };
        private TextBox txtEndValue { get; } = new BasicTextBox() { Size = inputSize };
        private Button btnAdd { get; } = new BasicButton()
        {
            Width = inputSize.Y,
            Height = inputSize.Y,
            Text = "+"
        };

        private FillFlowContainer commandsList = new FillFlowContainer
        {
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical
        };

        private void addInput(string text, Drawable input)
        {
            inputBar.Add(
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new SpriteText { Text = text },
                        input
                    }
                }
            );
        }

        private void loadCommandsList()
        {
            commandsList.Clear();
            var type = dropType.Current.Value;
            for(var i = 0; i < story.Commands.Count; ++i) 
            {
                var command = story.Commands[i];
                if (type == "All Commands" || type == command.Type.ToString())
                {
                    var localIndex = i;
                    commandsList.Add(new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            new BasicButton
                            {
                                Action = () => handleRemoveClick(localIndex),
                                Width = inputSize.Y,
                                Height = inputSize.Y,
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

        private void handleAddClick()
        {
            var data = new string[]
            {
                $"{dropType.Current.Value}",
                $"{txtStartTime.Current.Value}",
                $"{txtEndTime.Current.Value}",
                $"{dropEasing.Current.Value}",
                $"{txtStartValue.Current.Value}",
                $"{txtEndValue.Current.Value}"
            };
            var join = String.Join("|", data);
            var command = Command.FromString(join);
            story.AddCommand(command);
            loadCommandsList();
        }

        private void handleRemoveClick(int commandIndex)
        {
            story.RemoveCommand(commandIndex);
            loadCommandsList();
        }

        private void handleTypeSelect(ValueChangedEvent<string> e)
        {
            loadCommandsList();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Anchor = Anchor.TopRight;
            Origin = Anchor.TopRight;
            Size = panelSize;

            var allCommands = new List<string> {
                "All Commands"
            };
            allCommands.AddRange(Enum.GetNames(typeof(Commands)));
            dropType.Items = allCommands;
            dropEasing.Items = Enum.GetNames(typeof(Easing));

            addInput("Type", dropType);
            dropType.Current.BindValueChanged(handleTypeSelect);

            addInput("StartTime", txtStartTime);
            addInput("EndTime", txtEndTime);
            addInput("Easing", dropEasing);
            addInput("StartValue", txtStartValue);
            addInput("EndValue", txtEndValue);

            addInput(" ", btnAdd);
            btnAdd.Action = handleAddClick;

            loadCommandsList();

            Children = new Drawable[]
            {
                new RelativeBox { Colour = Color4.Black.Opacity(0.9f) },
                new BasicScrollContainer
                {
                    // This is so mega fucked?
                    Position = new Vector2(0, 70),
                    Width = panelSize.X,
                    Height = panelSize.Y - 70,
                    Child = commandsList
                },
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new SpriteText { Text = "Command Panel" },
                        inputBar
                    }
                }
            };
        }

        protected override void PopIn()
        {
            this.FadeIn(100);
        }

        protected override void PopOut()
        {
            this.FadeOut(100);
        }
    }
}
