using System;
using Microsoft.WindowsAPICodePack.Dialogs;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK.Input;

namespace S2VX.Game
{
    public class S2VXGame : S2VXGameBase
    {
        [Cached]
        public Story Story { get; } = new Story();

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
                }
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
                case Key.T:
                    Story.Timeline.DisplayMS = !Story.Timeline.DisplayMS;
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
