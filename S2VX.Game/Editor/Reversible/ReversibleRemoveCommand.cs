using S2VX.Game.Editor.Containers;
using S2VX.Game.Story;
using S2VX.Game.Story.Command;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleRemoveCommand : IReversible {
        private S2VXCommand Command { get; set; }

        private S2VXStory Story { get; set; }

        private CommandPanel CommandPanel { get; set; }

        public ReversibleRemoveCommand(S2VXStory story, S2VXCommand command, CommandPanel commandPanel) {
            Story = story;
            Command = command;
            CommandPanel = commandPanel;
        }

        public void Undo() {
            Story.AddCommand(Command);
            CommandPanel.LoadCommandsList();
        }

        public void Redo() {
            Story.RemoveCommand(Command);
            CommandPanel.LoadCommandsList();
        }
    }
}
