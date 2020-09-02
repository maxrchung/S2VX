namespace S2VX.Game.Editor.Reversible {
    public class ReversibleNode {
        public ReversibleNode Previous { get; set; }
        public IReversible Value { get; set; }
        public ReversibleNode Next { get; set; }
    }
}
