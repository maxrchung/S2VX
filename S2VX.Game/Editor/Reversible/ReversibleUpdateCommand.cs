using S2VX.Game.Editor.CommandPanel;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleUpdateCommand : IReversible {
        private S2VXCommandPanel CommandPanel { get; set; }
        private S2VXStory Story { get; set; }
        private S2VXCommand OldCommand { get; set; }
        private S2VXCommand NewCommand { get; set; }

        public ReversibleUpdateCommand(S2VXCommand oldCommand, S2VXCommand newCommand, S2VXCommandPanel commandPanel) {
            OldCommand = oldCommand;
            NewCommand = newCommand;
            CommandPanel = commandPanel;
        }

        public void Undo() {
            CommandPanel.RemoveCommand(NewCommand);
            CommandPanel.AddCommand(OldCommand);  // Add and reload command panel
        }

        // Throws exception if CommandString is invalid
        public void Redo() {
            CommandPanel.AddCommand(NewCommand);
            CommandPanel.RemoveCommand(OldCommand);  // Add and reload command panel
        }
    }
}
