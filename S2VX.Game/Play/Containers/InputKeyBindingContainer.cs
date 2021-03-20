using osu.Framework.Input.Bindings;
using System.Collections.Generic;

namespace S2VX.Game.Play.Containers {
    public class InputKeyBindingContainer : KeyBindingContainer<InputAction>, IKeyBindingHandler<InputAction> {

        private PlayScreen PlayScreen { get; set; }

        public InputKeyBindingContainer(PlayScreen playScreen) : base(SimultaneousBindingMode.All) => PlayScreen = playScreen;

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
            new KeyBinding(new[] { InputKey.MouseRight }, InputAction.Input),
            new KeyBinding(new[] { InputKey.Shift, InputKey.E }, InputAction.ToggleHitErrorBarVisibility),
            new KeyBinding(new[] { InputKey.Shift, InputKey.Tab }, InputAction.ToggleScoreVisibility)
        };

        public bool OnPressed(InputAction action) {
            switch (action) {
                case InputAction.ToggleHitErrorBarVisibility:
                    PlayScreen.ConfigHitErrorBarVisibility.Value = !PlayScreen.ConfigHitErrorBarVisibility.Value;
                    break;
                case InputAction.ToggleScoreVisibility:
                    PlayScreen.ConfigScoreVisibility.Value = !PlayScreen.ConfigScoreVisibility.Value;
                    break;
            }
            return false;
        }

        public void OnReleased(InputAction action) { }
    }
}
