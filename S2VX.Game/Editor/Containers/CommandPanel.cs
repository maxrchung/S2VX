using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using S2VX.Game.Editor.Reversible;
using S2VX.Game.Editor.UserInterface;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;
using System;

namespace S2VX.Game.Editor.Containers {
    public class CommandPanel : S2VXOverlayContainer {

        [Resolved]
        private EditorScreen Editor { get; set; } = null;
        [Resolved]
        private S2VXStory Story { get; set; } = null;

        public static Vector2 InputSize { get; } = new Vector2(106, 30);
        public static float InputBarHeight { get; } = 70;
        private static Vector2 PanelSize { get; } = new Vector2(727, 800);
        public CommandPanelInputBar AddInputBar { get; private set; }
        private CommandPanelInputBar EditInputBar { get; set; }
        private S2VXCommand EditCommandReference { get; set; }
        private readonly FillFlowContainer CommandsList = new() {
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical
        };

        private CommandPanelInputBar CreateAddInputBar() =>
            CommandPanelInputBar.CreateAddInputBar(HandleTypeSelect, HandleAddClick, Editor.CurrentTime);

        private CommandPanelInputBar CreateEditInputBar() {
            var editInputBar = CommandPanelInputBar.CreateEditInputBar(() => HandleSaveCommand(EditCommandReference), Editor.CurrentTime);
            editInputBar.Depth = -1;
            return editInputBar;
        }

        private void LoadCommandsList() {
            CommandsList.Clear();
            if (EditCommandReference != null) {
                // Reconstruct EditInputBar since it has been disposed
                ResetEditInputBar(EditCommandReference);
            }
            var type = AddInputBar.DropType.Current.Value;
            for (var i = 0; i < Story.Commands.Count; ++i) {
                var command = Story.Commands[i];
                if (type == "All Commands" || type == command.GetCommandName()) {
                    if (EditCommandReference == command) {
                        AddEditBarToCommandsList();
                    } else {
                        AddCommandToCommandsList(i, command);
                    }
                }
            }
            CommandsList.Add(new Container { Height = 350 });
        }

        private void AddEditBarToCommandsList() {
            CommandsList.Add(new FillFlowContainer {
                AutoSizeAxes = Axes.Both,
                Children = new Drawable[] {
                    new IconButton {
                        Action = () => HandleCancelCommand(),
                        Width = InputSize.Y,
                        Height = InputSize.Y,
                        Icon = FontAwesome.Solid.Times
                    }
                }
            });
            CommandsList.Add(EditInputBar);
        }

        private void AddCommandToCommandsList(int localIndex, S2VXCommand command) =>
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

        private void HandleAddClick() {
            AddInputBar.Reset();
            var commandString = AddInputBar.ValuesToString();
            try {
                var command = S2VXCommand.FromString(commandString);
                HandleAddCommand(command);
            } catch (Exception ex) {
                AddInputBar.AddErrorIndicator();
                Console.WriteLine(ex);
            }
        }

        private void HandleRemoveClick(int commandIndex) => HandleRemoveCommand(Story.Commands[commandIndex]);

        private void HandleCopyClick(int commandIndex) => HandleCopyCommand(Story.Commands[commandIndex]);

        private void HandleEditClick(S2VXCommand command) {
            EditCommandReference = command;
            LoadCommandsList();
        }

        private void HandleCancelCommand() {
            EditCommandReference = null;
            LoadCommandsList();
        }

        private void HandleAddCommand(S2VXCommand command) => Editor.Reversibles.Push(new ReversibleAddCommand(command, this));

        private void HandleRemoveCommand(S2VXCommand command) => Editor.Reversibles.Push(new ReversibleRemoveCommand(command, this));

        private void HandleCopyCommand(S2VXCommand command) {
            AddInputBar.CommandToValues(command);
            LoadCommandsList();
        }

        private void ResetEditInputBar(S2VXCommand command) {
            EditInputBar = CreateEditInputBar();
            EditInputBar.CommandToValues(command);
        }

        private void HandleSaveCommand(S2VXCommand oldCommand) {
            var commandString = EditInputBar.ValuesToString();
            var addSuccessful = false;
            try {
                var newCommand = S2VXCommand.FromString(commandString);
                Editor.Reversibles.Push(new ReversibleUpdateCommand(oldCommand, newCommand, this));
                addSuccessful = true;
            } catch (Exception ex) {
                EditInputBar.AddErrorIndicator();
                Console.WriteLine(ex);
            }
            if (addSuccessful) {
                EditCommandReference = null;
                LoadCommandsList();
            }
        }

        private void HandleTypeSelect(ValueChangedEvent<string> e) {
            AddInputBar.Reset();
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
            Y = 125;
            Size = PanelSize;

            AddInputBar = CreateAddInputBar();
            EditInputBar = CreateEditInputBar();

            LoadCommandsList();
            Children = new Drawable[] {
                new RelativeBox { Colour = Color4.Black.Opacity(0.9f) },
                new S2VXScrollContainer
                {
                    // This is so mega fucked?
                    Position = new Vector2(0, InputBarHeight),
                    Width = PanelSize.X,
                    Height = PanelSize.Y - InputBarHeight,
                    Child = CommandsList
                },
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new SpriteText { Text = "Command Panel" },
                        AddInputBar
                    }
                }
            };
        }

        protected override bool OnScroll(ScrollEvent e) => false;
    }
}
