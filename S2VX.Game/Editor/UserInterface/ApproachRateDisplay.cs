using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;

namespace S2VX.Game.Editor.UserInterface {
    public class ApproachRateDisplay : CompositeDrawable {
        [Resolved]
        private EditorScreen Editor { get; set; }

        private TextFlowContainer TxtApproachRate { get; set; }

        public Anchor TextAnchor { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            AutoSizeAxes = Axes.Both;
            AddInternal(TxtApproachRate = new(s => s.Font = new("default", SizeConsts.TextSize2, "500")) {
                TextAnchor = TextAnchor,
                AutoSizeAxes = Axes.Both
            });
        }

        protected override void Update() => TxtApproachRate.Text = $"Approach Rate: {Editor.EditorApproachRate}";

        protected override bool OnScroll(ScrollEvent e) {
            if (e.ScrollDelta.Y > 0) {
                Editor.ApproachRateIncrease();
            } else {
                Editor.ApproachRateDecrease();
            }
            return true;
        }
    }
}
