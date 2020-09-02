using NUnit.Framework;
using S2VX.Game.Editor.Reversible;

namespace S2VX.Game.Tests {
    public class Value {
        public int Number { get; set; }
    }

    public class Add1 : IReversible {
        private Value Value { get; }
        public Add1(Value value) => Value = value;
        public void Undo() => --Value.Number;
        public void Redo() => ++Value.Number;
    }

    [TestFixture]
    public class ReversibleStackTests {
        [Test]
        public void UndoAtZeroReversibles_DoesNothing() {
            var reversibles = new ReversibleStack();
            reversibles.Undo();
        }

        [Test]
        public void RedoAtZeroReversibles_DoesNothing() {
            var reversibles = new ReversibleStack();
            reversibles.Redo();
        }

        [Test]
        public void Add1_Adds1ToValue() {
            var reversibles = new ReversibleStack();
            var value = new Value();
            reversibles.Push(new Add1(value));
            Assert.AreEqual(1, value.Number);
        }

        [Test]
        public void Add1Twice_Adds2ToValue() {
            var reversibles = new ReversibleStack();
            var value = new Value();
            reversibles.Push(new Add1(value));
            reversibles.Push(new Add1(value));
            Assert.AreEqual(2, value.Number);
        }

        [Test]
        public void Add1Undo_Is0() {
            var reversibles = new ReversibleStack();
            var value = new Value();
            reversibles.Push(new Add1(value));
            reversibles.Undo();
            Assert.AreEqual(0, value.Number);
        }

        [Test]
        public void Add1UndoRedo_Adds1ToValue() {
            var reversibles = new ReversibleStack();
            var value = new Value();
            reversibles.Push(new Add1(value));
            reversibles.Undo();
            reversibles.Redo();
            Assert.AreEqual(1, value.Number);
        }

        [Test]
        public void Add1InMiddleOfStack_RemovesLaterReversibles() {
            var reversibles = new ReversibleStack();
            var value = new Value();
            reversibles.Push(new Add1(value));
            reversibles.Push(new Add1(value));
            reversibles.Push(new Add1(value));
            reversibles.Undo();
            reversibles.Undo();
            reversibles.Push(new Add1(value));
            reversibles.Redo();
            reversibles.Redo();
            Assert.AreEqual(2, value.Number);
        }
    }
}
