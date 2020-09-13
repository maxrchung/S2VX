using S2VX.Game.Story;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleAddCommand : IReversible {
        private S2VXStory Story { get; set; }

        private Command Command { get; }

        public ReversibleAddCommand(S2VXStory story, Command command) {
            Story = story;
            Command = command;
        }

        public void Undo() => Story.RemoveCommand(Command);

        public void Redo() => Story.AddCommand(Command);
    }
}
