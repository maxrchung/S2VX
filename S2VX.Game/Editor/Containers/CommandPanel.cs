﻿using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Editor.Reversible;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace S2VX.Game.Editor.Containers {
    public class CommandPanel : OverlayContainer {
        [Resolved]
        private EditorScreen Editor { get; set; } = null;
        [Resolved]
        private S2VXStory Story { get; set; } = null;

        private static Vector2 PanelSize { get; } = new Vector2(727, 727);
        private static Vector2 InputSize { get; } = new Vector2(100, 30);

        private FillFlowContainer InputBar { get; } = new FillFlowContainer { AutoSizeAxes = Axes.Both };
        private Dropdown<string> DropType { get; } = new BasicDropdown<string> {
            Width = 160
        };
        private TextBox TxtStartTime { get; } = new BasicTextBox() { Size = InputSize };
        private TextBox TxtEndTime { get; } = new BasicTextBox() { Size = InputSize };
        private Dropdown<string> DropEasing { get; } = new BasicDropdown<string> { Width = InputSize.X };
        private TextBox TxtStartValue { get; } = new BasicTextBox() { Size = InputSize };
        private TextBox TxtEndValue { get; } = new BasicTextBox() { Size = InputSize };
        private Button BtnAdd { get; } = new IconButton() {
            Width = InputSize.Y,
            Height = InputSize.Y,
            Icon = FontAwesome.Solid.Plus
        };

        private Container ErrorContainer { get; } = new Container {
            RelativePositionAxes = Axes.Both,
            RelativeSizeAxes = Axes.Both,
        };

        private FillFlowContainer EditInputBar { get; set; }
        private Dropdown<string> EditDropType { get; set; }
        private TextBox EditTxtStartTime { get; set; }
        private TextBox EditTxtEndTime { get; set; }
        private Dropdown<string> EditDropEasing { get; set; }
        private TextBox EditTxtStartValue { get; set; }
        private TextBox EditTxtEndValue { get; set; }
        private S2VXCommand EditCommandReference { get; set; }

        private Container EditErrorContainer { get; set; }

        private readonly FillFlowContainer CommandsList = new FillFlowContainer {
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical
        };

        private void AddInput(string text, Drawable input) => AddInput(text, input, InputBar);

        private static void AddInput(string text, Drawable input, FillFlowContainer inputBar) => inputBar.Add(
                new FillFlowContainer {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new SpriteText { Text = text },
                        input
                    }
                }
            );

        private void LoadCommandsList() {
            CommandsList.Clear();
            var type = DropType.Current.Value;
            for (var i = 0; i < Story.Commands.Count; ++i) {
                var command = Story.Commands[i];
                if (type == "All Commands" || type == command.GetCommandName()) {
                    var localIndex = i;
                    if (EditCommandReference == command) {
                        CommandsList.Add(new FillFlowContainer {
                            AutoSizeAxes = Axes.Both,
                            Children = new Drawable[] {
                                new IconButton {
                                    Action = () => HandleCancelCommand(),
                                    Width = InputSize.Y,
                                    Height = InputSize.Y,
                                    Icon = FontAwesome.Solid.Times
                                },
                                new IconButton {
                                    Action = () => HandleSaveCommand(command),
                                    Width = InputSize.Y,
                                    Height = InputSize.Y,
                                    Icon = FontAwesome.Solid.Save
                                }
                            }
                        });
                        CommandsList.Add(EditInputBar);
                    } else {
                        CommandsList.Add(new FillFlowContainer {
                            AutoSizeAxes = Axes.Both,
                            Children = new Drawable[] {
                                new IconButton {
                                    Action = () => HandleRemoveClick(localIndex),
                                    Width = InputSize.Y,
                                    Height = InputSize.Y,
                                    Icon = FontAwesome.Solid.Trash
                                },
                                new IconButton {
                                    Action = () => HandleCopyClick(localIndex),
                                    Width = InputSize.Y,
                                    Height = InputSize.Y,
                                    Icon = FontAwesome.Solid.Clone
                                },
                                new IconButton {
                                    Action = () => HandleEditClick(command),
                                    Width = InputSize.Y,
                                    Height = InputSize.Y,
                                    Icon = FontAwesome.Solid.Edit
                                },
                                new SpriteText {
                                    Text = command.ToString()
                                },
                            }
                        });
                    }
                }
            }
        }

        private void AddErrorIndicator() => AddErrorIndicator(ErrorContainer);

        private static void AddErrorIndicator(Container errorContainer) {
            errorContainer.Add(new Box {
                RelativePositionAxes = Axes.Both,
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
                Colour = Color4.Red,
                Width = 0.95f,
                Height = 0.0025f,
                Y = .0555f,
                X = -.025f,
            });
            errorContainer.Add(new Box {
                RelativePositionAxes = Axes.Both,
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
                Colour = Color4.Red,
                Width = 0.95f,
                Height = 0.0025f,
                Y = .095f,
                X = -.025f,
            });
            errorContainer.Add(new Box {
                RelativePositionAxes = Axes.Both,
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.TopLeft,
                Origin = Anchor.Centre,
                Colour = Color4.Red,
                Width = 0.0025f,
                Height = 0.04f,
                Y = .075f,
                X = 0,
            });
            errorContainer.Add(new Box {
                RelativePositionAxes = Axes.Both,
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.TopRight,
                Origin = Anchor.Centre,
                Colour = Color4.Red,
                Width = 0.0025f,
                Height = 0.04f,
                Y = .075f,
                X = -.051f,
            });
        }

        private void HandleAddClick() {
            ErrorContainer.Clear();
            var data = new string[]
            {
                $"{DropType.Current.Value}",
                $"{TxtStartTime.Current.Value}",
                $"{TxtEndTime.Current.Value}",
                $"{DropEasing.Current.Value}",
                $"{TxtStartValue.Current.Value}",
                $"{TxtEndValue.Current.Value}"
            };
            var join = string.Join("|", data);
            try {
                var command = S2VXCommand.FromString(join);
                HandleAddCommand(command);
            } catch (FormatException ex) {
                AddErrorIndicator();
                Console.WriteLine(ex);
            } catch (TargetInvocationException ex) {
                AddErrorIndicator();
                Console.WriteLine(ex);
            } catch (NullReferenceException ex) {
                AddErrorIndicator();
                Console.WriteLine(ex);
            }
        }

        private void HandleRemoveClick(int commandIndex) => HandleRemoveCommand(Story.Commands[commandIndex]);

        private void HandleCopyClick(int commandIndex) => HandleCopyCommand(Story.Commands[commandIndex]);

        private void HandleEditClick(S2VXCommand command) {
            EditCommandReference = command;
            HandleEditCommand(command);
        }

        private void HandleCancelCommand() {
            EditCommandReference = null;
            LoadCommandsList();
        }

        private void HandleAddCommand(S2VXCommand command) => Editor.Reversibles.Push(new ReversibleAddCommand(command, this));

        private void HandleRemoveCommand(S2VXCommand command) => Editor.Reversibles.Push(new ReversibleRemoveCommand(command, this));

        private void HandleCopyCommand(S2VXCommand command) {
            ErrorContainer.Clear();
            var data = command.ToString().Split('{', '|', '}');

            DropType.Current.Value = data[0];
            TxtStartTime.Text = data[1];
            TxtEndTime.Text = data[2];
            DropEasing.Current.Value = data[3];
            TxtStartValue.Text = data[4];
            TxtEndValue.Text = data[5];
            LoadCommandsList();
        }

        private void HandleEditCommand(S2VXCommand command) {
            EditInputBar = new FillFlowContainer { AutoSizeAxes = Axes.Both };
            EditErrorContainer = new Container { }; // TODO: fix this
            //    RelativePositionAxes = Axes.Both,
            //    RelativeSizeAxes = Axes.Both,
            //};
            EditInputBar.Add(EditErrorContainer);
            EditDropType = new BasicDropdown<string> {
                Width = 160
            };
            EditTxtStartTime = new BasicTextBox() { Size = InputSize };
            EditTxtEndTime = new BasicTextBox() { Size = InputSize };
            EditDropEasing = new BasicDropdown<string> { Width = InputSize.X };
            EditTxtStartValue = new BasicTextBox() { Size = InputSize };
            EditTxtEndValue = new BasicTextBox() { Size = InputSize };

            var data = command.ToString().Split('{', '|', '}');

            EditDropType.Current.Value = data[0];
            EditTxtStartTime.Text = data[1];
            EditTxtEndTime.Text = data[2];
            EditDropEasing.Current.Value = data[3];
            EditTxtStartValue.Text = data[4];
            EditTxtEndValue.Text = data[5];

            EditDropType.Items = S2VXCommand.GetCommandNames();
            EditDropEasing.Items = Enum.GetNames(typeof(Easing));
            AddInput("Type", EditDropType, EditInputBar);
            AddInput("StartTime", EditTxtStartTime, EditInputBar);
            AddInput("EndTime", EditTxtEndTime, EditInputBar);
            AddInput("Easing", EditDropEasing, EditInputBar);
            AddInput("StartValue", EditTxtStartValue, EditInputBar);
            AddInput("EndValue", EditTxtEndValue, EditInputBar);
            LoadCommandsList();
        }

        private void HandleSaveCommand(S2VXCommand oldCommand) {
            EditErrorContainer.Clear();
            var data = new string[]
            {
                $"{EditDropType.Current.Value}",
                $"{EditTxtStartTime.Current.Value}",
                $"{EditTxtEndTime.Current.Value}",
                $"{EditDropEasing.Current.Value}",
                $"{EditTxtStartValue.Current.Value}",
                $"{EditTxtEndValue.Current.Value}"
            };
            var commandString = string.Join("|", data);
            var addSuccessful = false;
            try {
                Editor.Reversibles.Push(new ReversibleUpdateCommand(commandString, oldCommand, this, Story));
                addSuccessful = true;
            } catch (FormatException ex) {
                AddErrorIndicator(EditErrorContainer);
                Console.WriteLine(ex);
            } catch (TargetInvocationException ex) {
                AddErrorIndicator(EditErrorContainer);
                Console.WriteLine(ex);
            } catch (NullReferenceException ex) {
                AddErrorIndicator(EditErrorContainer);
                Console.WriteLine(ex);
            }
            if (addSuccessful) {
                EditCommandReference = null;
                LoadCommandsList();
            }
        }

        private void HandleTypeSelect(ValueChangedEvent<string> e) {
            ErrorContainer.Clear();
            HandleCancelCommand();  // Cancel edit if type filter is changed
            LoadCommandsList();
        }

        // Non-reversibly add a command and reload command list
        public void AddCommand(S2VXCommand command) {
            Story.AddCommand(command);
            LoadCommandsList();
        }

        // Non-reversibly remove a command and reload command list
        public void RemoveCommand(S2VXCommand command) {
            Story.RemoveCommand(command);
            LoadCommandsList();
        }

        [BackgroundDependencyLoader]
        private void Load() {
            Anchor = Anchor.TopRight;
            Origin = Anchor.TopRight;
            Size = PanelSize;

            var allCommands = new List<string> {
                "All Commands"
            };
            var allCommandNames = S2VXCommand.GetCommandNames();
            allCommands.AddRange(allCommandNames);
            DropType.Items = allCommands;
            DropEasing.Items = Enum.GetNames(typeof(Easing));

            AddInput("Type", DropType);
            DropType.Current.BindValueChanged(HandleTypeSelect);

            AddInput("StartTime", TxtStartTime);
            AddInput("EndTime", TxtEndTime);
            AddInput("Easing", DropEasing);
            AddInput("StartValue", TxtStartValue);
            AddInput("EndValue", TxtEndValue);

            AddInput(" ", BtnAdd);
            BtnAdd.Action = HandleAddClick;

            LoadCommandsList();
            Children = new Drawable[] {
                new RelativeBox { Colour = Color4.Black.Opacity(0.9f) },
                new BasicScrollContainer
                {
                    // This is so mega fucked?
                    Position = new Vector2(0, 70),
                    Width = PanelSize.X,
                    Height = PanelSize.Y - 70,
                    Child = CommandsList
                },
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new SpriteText { Text = "Command Panel" },
                        InputBar
                    }
                },
                ErrorContainer
            };
        }

        protected override void PopIn() => this.FadeIn(100);

        protected override void PopOut() => this.FadeOut(100);
    }
}
