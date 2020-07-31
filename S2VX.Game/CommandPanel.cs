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

        [BackgroundDependencyLoader]
        private void load()
        {
            Anchor = Anchor.CentreRight;
            Origin = Anchor.CentreRight;
            RelativeSizeAxes = Axes.Both;
            Width = 0.5f;
            Height = 1;

            var dropdown = new BasicDropdown<string>
            {
                Width = 150
            };
            var allCommands = new List<string> {
                "All Commands"
            };
            allCommands.AddRange(Enum.GetNames(typeof(Commands)));
            dropdown.Items = allCommands;

            Children = new Drawable[]
            {
                new RelativeBox
                {
                    Colour = Color4.Black.Opacity(0.9f)
                },
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new SpriteText
                        {
                            Text = "Command Panel"
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Children = new Drawable[]
                            {
                                new FillFlowContainer
                                {
                                    RelativeSizeAxes = Axes.Y,
                                    AutoSizeAxes = Axes.X,
                                    Direction = FillDirection.Horizontal,
                                    Children = new Drawable[]
                                    {
                                        new FillFlowContainer
                                        {
                                            RelativeSizeAxes = Axes.Y,
                                            AutoSizeAxes = Axes.X,
                                            Direction = FillDirection.Vertical,
                                            Children = new Drawable[]
                                            {
                                                new SpriteText { Text = "Command Type" },
                                                dropdown
                                            }
                                        },
                                        new FillFlowContainer
                                        {
                                           RelativeSizeAxes = Axes.Y,
                                            AutoSizeAxes = Axes.X,
                                            Direction = FillDirection.Vertical,
                                            Children = new Drawable[]
                                            {
                                                new SpriteText { Text = "StartTime" },
                                                new BasicTextBox
                                                {
                                                    Width = 100,
                                                    Height = 30,
                                                }
                                            }
                                        },
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }


}
