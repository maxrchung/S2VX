using System.Collections.Generic;
using System.Linq;

namespace S2VX.Game.Editor.Reversible {
    public class ReversibleStack {
        private int Pointer { get; set; }
        private List<IReversible> Reversibles { get; set; } = new List<IReversible>();

        public void Push(IReversible reversible) {
            // Execute reversible
            reversible.Redo();

            // If pointer is in the middle of the list, drop all previously saved commands
            if (Pointer != Reversibles.Count - 1) {
                Reversibles = Reversibles.Take(Pointer + 1).ToList();
            }
            Reversibles.Add(reversible);
            ++Pointer;
        }

        public void Undo() {
            if (Pointer == 0) {
                return;
            }
            Reversibles[Pointer--].Undo();
        }

        public void Redo() {
            if (Pointer == Reversibles.Count - 1) {
                return;
            }
            Reversibles[Pointer++].Redo();
        }
    }
}
