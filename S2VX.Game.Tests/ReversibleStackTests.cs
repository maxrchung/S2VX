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
        public void Push_Is1() {
            var reversibles = new ReversibleStack();
            var value = new Value();
            reversibles.Push(new Add1(value));
            Assert.AreEqual(1, value.Number);
        }

        [Test]
        public void PushTwice_Is2() {
            var reversibles = new ReversibleStack();
            var value = new Value();
            reversibles.Push(new Add1(value));
            reversibles.Push(new Add1(value));
            Assert.AreEqual(2, value.Number);
        }

        [Test]
        public void PushUndo_Is0() {
            var reversibles = new ReversibleStack();
            var value = new Value();
            reversibles.Push(new Add1(value));
            reversibles.Undo();
            Assert.AreEqual(0, value.Number);
        }

        [Test]
        public void PushUndoRedo_Is1() {
            var reversibles = new ReversibleStack();
            var value = new Value();
            reversibles.Push(new Add1(value));
            reversibles.Undo();
            reversibles.Redo();
            Assert.AreEqual(1, value.Number);
        }

        [Test]
        public void PushInMiddleOfStack_RemovesLaterReversibles() {
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

        [Test]
        public void PushThriceWithMaxCountOf2_Is1() {
            var reversibles = new ReversibleStack(2);
            var value = new Value();
            reversibles.Push(new Add1(value));
            reversibles.Push(new Add1(value));
            reversibles.Push(new Add1(value));
            reversibles.Undo();
            reversibles.Undo();
            reversibles.Undo();
            Assert.AreEqual(1, value.Number);
        }
    }
}
