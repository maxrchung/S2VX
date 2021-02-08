using S2VX.Game.Editor.Containers;
using S2VX.Game.Story.Command;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleAddCommand : IReversible {
        private S2VXCommand Command { get; set; }

        private CommandPanel CommandPanel { get; set; }

        public ReversibleAddCommand(S2VXCommand command, CommandPanel commandPanel) {
            Command = command;
            CommandPanel = commandPanel;
        }

        public void Undo() => CommandPanel.RemoveCommand(Command);

        public void Redo() => CommandPanel.AddCommand(Command);
    }
}
