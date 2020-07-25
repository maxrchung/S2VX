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
                                new MenuItem("Open", open),
                                new MenuItem("Save As", save)
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
            }
            return true;
        }

        private void open()
        {
            Story.Play(false);
            var dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("Story files", "json"));
            dialog.Filters.Add(new CommonFileDialogFilter("All files", "*"));
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Story.Open(dialog.FileName);
            }
            else
            {
                Story.Play(true);
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
            else
            {
                Story.Play(true);
            }
        }
    }
}
