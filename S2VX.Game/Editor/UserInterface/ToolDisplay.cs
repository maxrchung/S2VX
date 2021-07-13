using osu.Framework.Allocation;

namespace S2VX.Game.Editor.UserInterface {
    public class ToolDisplay : EditorInfoDisplay {
        [Resolved]
        private EditorScreen Editor { get; set; }

        public override void UpdateDisplay() => UpdateDisplay($"Tool: {Editor.ToolState.DisplayName()}");
    }
}
