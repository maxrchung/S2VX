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
using S2VX.Game.Story;

namespace S2VX.Game.Editor
{
    public class S2VXEditor : CompositeDrawable
    {
        public Vector2 MousePosition { get; private set; } = Vector2.Zero;

        [Cached]
        private S2VXStory story { get; set; } = new S2VXStory();

        private CommandPanel commandPanel { get; } = new CommandPanel();
        private bool isCommandPanelVisible { get; set; } = false;

        private Timeline timeline { get; } = new Timeline();

        private ToolState toolState { get; set; } = new SelectToolState();

        private Container toolContainer { get; set; } = new Container
        {
            RelativeSizeAxes = Axes.Both,
            Size = Vector2.One
        };

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Size = Vector2.One;

            toolContainer.Child = toolState;
            InternalChildren = new Drawable[]
            {
                story,
                toolContainer,
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
                                new MenuItem("Play/Pause (Space)", playbackPlay),
                                new MenuItem("Restart (X)", playbackRestart),
                                new MenuItem("Toggle Time Display (T)", playbackDisplay),
                            }
                        },
                        new MenuItem("Tool")
                        {
                            Items = new[]
                            {
                                new MenuItem("Select (1)", toolSelect),
                                new MenuItem("Note (2)", toolNote),
                            }
                        }
                    }
                },
                timeline,
                commandPanel,
            };
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            var mousePosition = ToLocalSpace(e.CurrentState.Mouse.Position);
            var relativePosition = (mousePosition - (story.DrawSize / 2)) / story.DrawWidth;
            var camera = story.Camera;
            var rotatedPosition = Utils.Rotate(relativePosition, -camera.Rotation);
            var scaledPosition = rotatedPosition * (1 / camera.Scale.X);
            var translatedPosition = scaledPosition + camera.Position;
            MousePosition = translatedPosition;
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
                        break;
                    }
                    toolSelect();
                    break;
                }
                case Key.Number2:
                {
                    if (e.ControlPressed)
                    {
                        viewCommandPanel();
                        break;
                    }
                    toolNote();
                    break;
                }
                case Key.Space:
                    playbackPlay();
                    break;
                case Key.X:
                    playbackRestart();
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

        private void toolSelect()
        {
            toolState = new SelectToolState();
            toolContainer.Child = toolState;
        }

        private void toolNote()
        {
            toolState = new NoteToolState();
            toolContainer.Child = toolState;
        }
    }
}
