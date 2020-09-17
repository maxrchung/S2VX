using S2VX.Game.Story;
using S2VX.Game.Story.Command;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleAddCommand : IReversible {
        public S2VXCommand Command { get; set; }

        private S2VXStory Story { get; set; }

        public ReversibleAddCommand(S2VXStory story, S2VXCommand command) {
            Story = story;
            Command = command;
        }

        public void Undo() => Story.RemoveCommand(Command);

        public void Redo() => Story.AddCommand(Command);
    }
}
