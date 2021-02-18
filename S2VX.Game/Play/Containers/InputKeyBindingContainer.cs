using osu.Framework.Input.Bindings;
using S2VX.Game.Play;
using System.Collections.Generic;

namespace S2VX.Game.Story.Play.Containers {
    public class InputKeyBindingContainer : KeyBindingContainer<InputAction> {
        public InputKeyBindingContainer() : base(SimultaneousBindingMode.All) { }
        public override IEnumerable<IKeyBinding> DefaultKeyBindings => new[] {
            new KeyBinding(
                new[] {
                    InputKey.Z,
                    InputKey.X,
                    InputKey.C,
                    InputKey.V,
                    InputKey.A,
                    InputKey.S,
                    InputKey.D,
                    InputKey.F,
                    InputKey.MouseLeft,
                    InputKey.MouseRight
                },
                InputAction.Input
            ),
        };
    }
}
