using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Dialogs;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace S2VX.Game
{
    public class S2VXGame : S2VXGameBase
    {
        [Cached]
        public Story Story { get; } = new Story();

        private BindableInt sliderCurrent { get; set; } = new BindableInt(1) { MinValue = 0, MaxValue = 2, Default = 1 };

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                Story,
                new BasicMenu(Direction.Horizontal, true)
                {
                    Width = 1,
                    Height = 0.05f,
                    RelativeSizeAxes = Axes.Both,
                    Items = new[]
                    {
                        new MenuItem("File")
                        {
                            Items = new[]
                            {
                                new MenuItem("Open... (Ctrl+O)", open),
                                new MenuItem("Save... (Ctrl+S)", save)
                            }
                        }
                    }
                },

                new FillFlowContainer()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Width = 0.5f,
                    Height = 0.5f,
                    Spacing = new Vector2(0),
                    Children = new Drawable[]
                    {
                        new BasicButton
                        {
                            BackgroundColour = Color4.White,
                            Text = "+",
                            Width = 200,
                            Height = 200,
                        },
                        new BasicDropdown<int>
                        {
                            Width = 200,
                            Items = new List<int>
                            {
                                1, 2, 3
                            }
                        },
                        //new BasicPasswordTextBox(),
                        //new BasicRearrangeableListContainer<int>(),
                        //new BasicSliderBar<int>()
                        //{
                        //    Current = sliderCurrent
                        //},
                        //new BasicTextBox(),
                    }
                },

                new Timeline()
            };
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    Story.Play(!Story.IsPlaying);
                    break;
                case Key.X:
                    Story.Restart();
                    break;
                case Key.O:
                    if (e.ControlPressed)
                    {
                        open();
                    }
                    break;
                case Key.S:
                    if (e.ControlPressed)
                    {
                        save();
                    }
                    break;
            }
            return true;
        }

        private void open()
        {
            // The dialog runs synchronously so the game time will skip forward
            // after cancelling. To counteract this, we can always force the
            // game to pause.
            Story.Play(false);
            var dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("Story files", "json"));
            dialog.Filters.Add(new CommonFileDialogFilter("All files", "*"));
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Story.Open(dialog.FileName);
            }
        }

        private void save()
        {
            Story.Play(false);
            var dialog = new CommonSaveFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("Story files", "json"));
            dialog.Filters.Add(new CommonFileDialogFilter("All files", "*"));
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Story.Save(dialog.FileName);
            }
        }
    }
}
