using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
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

        [BackgroundDependencyLoader]
        private void load()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;
            Width = 0.5f;
            Height = 0.5f;

            var dropdown = new BasicDropdown<string>()
            {
                Width = 200
            };
            var allCommands = new List<string> {
                "All Commands"
            };
            allCommands.AddRange(Enum.GetNames(typeof(Commands)));
            dropdown.Items = allCommands;

            var inputsRow = new FillFlowContainer
            {
                new Container
                {
                    new SpriteText()
                    {
                        Text = "Command"
                    },
                    dropdown,
                }
            };

            Children = new Drawable[]
            {
                inputsRow
                //new BasicButton
                //{
                //    BackgroundColour = Color4.White,
                //    Text = "+",
                //    Width = 200,
                //    Height = 200,
                //},
                //new BasicPasswordTextBox(),
                //new BasicRearrangeableListContainer<int>(),
                //new BasicSliderBar<int>()
                //{
                //    Current = sliderCurrent
                //},
                //new BasicTextBox(),
            };
        }
    }
}
