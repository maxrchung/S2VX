using osu.Framework.Input.Bindings;
using System.Collections.Generic;

namespace S2VX.Game.Play.Containers {
    public class PlayKeyBindingContainer : KeyBindingContainer<PlayAction>, IKeyBindingHandler<PlayAction> {

        private PlayScreen PlayScreen { get; set; }

        public PlayKeyBindingContainer(PlayScreen playScreen) : base(SimultaneousBindingMode.All) => PlayScreen = playScreen;

        public override IEnumerable<IKeyBinding> DefaultKeyBindings => new[] {
            new KeyBinding(new[] { InputKey.Z }, PlayAction.Input),
            new KeyBinding(new[] { InputKey.X }, PlayAction.Input),
            new KeyBinding(new[] { InputKey.C }, PlayAction.Input),
            new KeyBinding(new[] { InputKey.V }, PlayAction.Input),
            new KeyBinding(new[] { InputKey.A }, PlayAction.Input),
            new KeyBinding(new[] { InputKey.S }, PlayAction.Input),
            new KeyBinding(new[] { InputKey.D }, PlayAction.Input),
            new KeyBinding(new[] { InputKey.F }, PlayAction.Input),
            new KeyBinding(new[] { InputKey.MouseLeft }, PlayAction.Input),
            new KeyBinding(new[] { InputKey.MouseRight }, PlayAction.Input),
            new KeyBinding(new[] { InputKey.Shift, InputKey.E }, PlayAction.ToggleHitErrorBarVisibility),
            new KeyBinding(new[] { InputKey.Shift, InputKey.Tab }, PlayAction.ToggleScoreVisibility)
        };

        public bool OnPressed(PlayAction action) {
            switch (action) {
                case PlayAction.ToggleHitErrorBarVisibility:
                    PlayScreen.ConfigHitErrorBarVisibility.Value = !PlayScreen.ConfigHitErrorBarVisibility.Value;
                    break;
                case PlayAction.ToggleScoreVisibility:
                    PlayScreen.ConfigScoreVisibility.Value = !PlayScreen.ConfigScoreVisibility.Value;
                    break;
            }
            return false;
        }

        public void OnReleased(PlayAction action) { }
    }
}
