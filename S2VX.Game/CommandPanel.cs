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
        protected override void PopIn()
        {
            this.FadeIn(100);
        }

        protected override void PopOut()
        {
            this.FadeOut(100);
        }

        private static Vector2 inputSize { get; set; } = new Vector2(100, 30);

        private FillFlowContainer inputBar { get; set; } = new FillFlowContainer { Direction = FillDirection.Horizontal };
        private Dropdown<string> dropType = new BasicDropdown<string> { Width = 160 };
        private TextBox txtStartTime { get; set; } = new BasicTextBox() { Size = inputSize };
        private TextBox txtEndTime { get; set; } = new BasicTextBox() { Size = inputSize };
        private Dropdown<string> dropEasing = new BasicDropdown<string> { Width = inputSize.X };
        private TextBox txtStartValue { get; set; } = new BasicTextBox() { Size = inputSize };
        private TextBox txtEndValue { get; set; } = new BasicTextBox() { Size = inputSize };
        private Button btnAdd { get; set; } = new BasicButton()
        {
            Width = inputSize.Y,
            Height = inputSize.Y,
            Text = "+"
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

        [BackgroundDependencyLoader]
        private void load()
        {
            Anchor = Anchor.CentreRight;
            Origin = Anchor.CentreRight;
            RelativeSizeAxes = Axes.Both;
            Width = 0.7f;
            Height = 1;

            var allCommands = new List<string> {
                "All Commands"
            };
            allCommands.AddRange(Enum.GetNames(typeof(Commands)));
            dropType.Items = allCommands;
            dropEasing.Items = Enum.GetNames(typeof(Easing));

            addInput("Type", dropType);
            addInput("StartTime", txtStartTime);
            addInput("EndTime", txtEndTime);
            addInput("Easing", dropEasing);
            addInput("StartValue", txtStartValue);
            addInput("EndValue", txtEndValue);
            addInput("Add", btnAdd);

            Children = new Drawable[]
            {
                new RelativeBox { Colour = Color4.Black.Opacity(0.9f) },
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new SpriteText { Text = "Command Panel" },
                        inputBar
                    }
                }
            };
        }
    }


}
