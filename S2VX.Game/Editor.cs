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
    public class Editor : CompositeDrawable
    {
        public Vector2 MousePosition { get; private set; } = Vector2.Zero;

        [Cached]
        private Story story { get; set; } = new Story();

        private CommandPanel commandPanel { get; } = new CommandPanel();
        private bool isCommandPanelVisible { get; set; } = false;

        private Timeline timeline { get; } = new Timeline();

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Size = Vector2.One;

            InternalChildren = new Drawable[]
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
                                new MenuItem("Open... (Ctrl+O)", fileOpen),
                                new MenuItem("Save... (Ctrl+S)", fileSave)
                            }
                        },
                        new MenuItem("View")
                        {
                            Items = new[]
                            {
                                new MenuItem("Command Panel (Ctrl+1)", viewCommandPanel),
                            }
                        },
                        new MenuItem("Playback")
                        {
                            Items = new[]
                            {
                                new MenuItem("Play/Pause (Space)", viewCommandPanel),
                                new MenuItem("Restart (X)", viewCommandPanel),
                                new MenuItem("Toggle Time Display (T)", viewCommandPanel),
                            }
                        }
                    }
                },
                commandPanel,
                timeline
            };
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            MousePosition = ToLocalSpace(e.CurrentState.Mouse.Position);
            return true;
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.O:
                    if (e.ControlPressed)
                    {
                        fileOpen();
                    }
                    break;
                case Key.S:
                    if (e.ControlPressed)
                    {
                        fileSave();
                    }
                    break;
                case Key.Number1:
                {
                    if (e.ControlPressed)
                    {
                        viewCommandPanel();
                    }
                    break;
                }
                case Key.Space:
                    playbackPlay();
                    break;
                case Key.X:
                    story.Restart();
                    break;
                case Key.T:
                    playbackDisplay();
                    break;
            }
            return true;
        }

        private void fileOpen()
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

        private void fileSave()
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

        private void viewCommandPanel()
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

        private void playbackPlay()
        {
            story.Play(!story.IsPlaying);
        }

        private void playbackRestart()
        {
            story.Restart();
        }

        private void playbackDisplay()
        {
            timeline.DisplayMS = !timeline.DisplayMS;
        }
    }
}
