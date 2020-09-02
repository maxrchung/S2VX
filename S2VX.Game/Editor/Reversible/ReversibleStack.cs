namespace S2VX.Game.Editor.Reversible {
    public class ReversibleList {
        private ReversibleNode Head { get; set; }
        private ReversibleNode Pointer { get; set; }
        public int MaxCount { get; }
        public int CurrentCount { get; set; }

        public ReversibleList(int maxCount = 100) {
            MaxCount = maxCount;
            Head = new ReversibleNode {
                Value = null
            };
            Pointer = Head;
        }

        public void Push(IReversible reversible) {
            Pointer.Next = new ReversibleNode {
                Previous = Pointer,
                Value = reversible
            };
            Pointer = Pointer.Next;
            // Execute reversible
            reversible.Redo();

            if (CurrentCount == MaxCount) {
                Head = Head.Next;
                Head.Previous = null;
                Head.Value = null;
            } else {
                ++CurrentCount;
            }
        }

        public void Undo() {
            if (Pointer == Head) {
                return;
            }
            Pointer.Value.Undo();
            Pointer = Pointer.Previous;
            --CurrentCount;
        }

        public void Redo() {
            if (Pointer.Next == null) {
                return;
            }
            Pointer = Pointer.Next;
            Pointer.Value.Redo();
            ++CurrentCount;
        }
    }
}
