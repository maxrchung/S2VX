using osu.Framework.Allocation;

namespace S2VX.Game.Editor.UserInterface {
    public class MousePositionDisplay : EditorInfoDisplay {
        [Resolved]
        private EditorScreen Editor { get; set; }

        public override void UpdateDisplay() => UpdateDisplay(S2VXUtils.Vector2ToString(Editor.MousePosition, 2));

    }
}
