using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace S2VX.Game.Editor.UserInterface {
    public class ToolDisplay : CompositeDrawable {
        [Resolved]
        private EditorScreen Editor { get; set; }

        private TextFlowContainer TxtTool { get; set; }

        public Anchor TextAnchor { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            AutoSizeAxes = Axes.Both;
            AddInternal(TxtTool = new TextFlowContainer(s => s.Font = new FontUsage("default", SizeConsts.TextSize2, "500")) {
                TextAnchor = TextAnchor,
                AutoSizeAxes = Axes.Both
            });
        }

        protected override void Update() => TxtTool.Text = $"Tool: {Editor.ToolState.DisplayName()}";

    }
}
