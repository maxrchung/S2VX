using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace S2VX.Game.Editor.UserInterface {
    public class ToolDisplay : CompositeDrawable {
        [Resolved]
        private S2VXEditor Editor { get; set; }

        private TextFlowContainer TxtTool { get; set; }

        public Anchor TextAnchor { get; set; }

        [BackgroundDependencyLoader]
        private void Load() =>
            InternalChildren = new Drawable[]
            {
                TxtTool = new TextFlowContainer(s => s.Font = new FontUsage("default", Editor.DrawWidth / 40, "500")) {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    TextAnchor = TextAnchor,
                }
            };

        protected override void Update() => TxtTool.Text = $"Tool: {Editor.ToolState.DisplayName()}";

    }
}
