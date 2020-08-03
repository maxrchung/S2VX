using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Dialogs;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
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
        private Story story { get; set; } = new Story();

        private CommandPanel commandPanel { get; } = new CommandPanel();
        private bool isCommandPanelVisible { get; set; } = false;

        private Timeline timeline { get; } = new Timeline();

        private NotesTimeline notesTimeline { get; } = new NotesTimeline();

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                story,
                new BasicMenu(Direction.Horizontal, true)
                {
                    BackgroundColour = Color4.Black.Opacity(0.9f),
                    Width = 1,
                    RelativeSizeAxes = Axes.X,
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
                commandPanel,
                timeline,
                notesTimeline,
            };
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    story.Play(!story.IsPlaying);
                    break;
                case Key.X:
                    story.Restart();
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
                case Key.Number1:
                {
                    if (e.ControlPressed)
                    {
                        if (isCommandPanelVisible)
                        {
                            commandPanel.Hide();
                        }
                        else
                        {
                            commandPanel.Show();
                        }
                        isCommandPanelVisible = !isCommandPanelVisible;
                    }
                    break;
                }
                case Key.T:
                    timeline.DisplayMS = !timeline.DisplayMS;
                    break;
            }
            return true;
        }

        private void open()
        {
            // The dialog runs synchronously so the game time will skip forward
            // after cancelling. To counteract this, we can always force the
            // game to pause.
            story.Play(false);
            var dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("Story files", "json"));
            dialog.Filters.Add(new CommonFileDialogFilter("All files", "*"));
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                story.Open(dialog.FileName);
            }
        }

        private void save()
        {
            story.Play(false);
            var dialog = new CommonSaveFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("Story files", "json"));
            dialog.Filters.Add(new CommonFileDialogFilter("All files", "*"));
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                story.Save(dialog.FileName);
            }
        }
    }
}
