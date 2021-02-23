using osu.Framework.Input.Bindings;
using S2VX.Game.Play;
using System.Collections.Generic;

namespace S2VX.Game.Story.Play.Containers {
    public class InputKeyBindingContainer : KeyBindingContainer<InputAction> {
        public InputKeyBindingContainer() : base(SimultaneousBindingMode.All) { }

        public override IEnumerable<IKeyBinding> DefaultKeyBindings => new[] {
            new KeyBinding(new[] { InputKey.Z }, InputAction.Input),
            new KeyBinding(new[] { InputKey.X }, InputAction.Input),
            new KeyBinding(new[] { InputKey.C }, InputAction.Input),
            new KeyBinding(new[] { InputKey.V }, InputAction.Input),
            new KeyBinding(new[] { InputKey.A }, InputAction.Input),
            new KeyBinding(new[] { InputKey.S }, InputAction.Input),
            new KeyBinding(new[] { InputKey.D }, InputAction.Input),
            new KeyBinding(new[] { InputKey.F }, InputAction.Input),
            new KeyBinding(new[] { InputKey.MouseLeft }, InputAction.Input),
            new KeyBinding(new[] { InputKey.MouseRight }, InputAction.Input)
        };
    }
}
