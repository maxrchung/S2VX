using S2VX.Game.Editor.Containers;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleUpdateCommand : IReversible {
        private string CommandString { get; set; }
        private CommandPanel CommandPanel { get; set; }
        private S2VXStory Story { get; set; }
        private S2VXCommand OldCommand { get; set; }
        private S2VXCommand NewCommand { get; set; }

        public ReversibleUpdateCommand(string commandString, S2VXCommand oldCommand, CommandPanel commandPanel, S2VXStory story) {
            CommandString = commandString;
            OldCommand = oldCommand;
            CommandPanel = commandPanel;
            Story = story;
        }

        public void Undo() {
            Story.RemoveCommand(NewCommand);
            CommandPanel.AddCommand(OldCommand);  // Add and update command panel
        }

        public void Redo() {  // throws exception if CommandString is invalid
            NewCommand = S2VXCommand.FromString(CommandString);
            Story.AddCommand(NewCommand);
            CommandPanel.RemoveCommand(OldCommand);  // Add and update command panel
            // Todo: Fix crashing when applying undo/redo while an edit window is open
        }
    }
}
