using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace S2VX.Game.Editor.UserInterface {
    public abstract class EditorInfoDisplay : CompositeDrawable {

        public Anchor TextAnchor { get; set; }
        private TextFlowContainer Text { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            AutoSizeAxes = Axes.Both;
            AddInternal(Text = new(s => s.Font = new("default", SizeConsts.TextSize2, "500")) {
                TextAnchor = TextAnchor,
                AutoSizeAxes = Axes.Both
            });
        }

        public abstract void UpdateDisplay();

        protected void UpdateDisplay(string text) => Text.Text = text;
    }
}
