namespace S2VX.Game.Story.Command {
    public abstract class GridCommand : S2VXCommand {
        public Grid Grid { get; } = new Grid();
    }
}
