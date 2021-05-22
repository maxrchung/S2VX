using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;

namespace S2VX.Game.Editor.UserInterface {
    public class NoteSnapDivisorDisplay : CompositeDrawable {
        [Resolved]
        private EditorScreen Editor { get; set; }

        private TextFlowContainer TxtNoteSnapDivisor { get; set; }

        public Anchor TextAnchor { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            AutoSizeAxes = Axes.Both;
            AddInternal(TxtNoteSnapDivisor = new(s => s.Font = new("default", SizeConsts.TextSize2, "500")) {
                TextAnchor = TextAnchor,
                AutoSizeAxes = Axes.Both
            });
        }

        protected override void Update() => TxtNoteSnapDivisor.Text =
            Editor.SnapDivisor == 0 ? "Snap Divisor: Free" : "Snap Divisor: " + S2VXUtils.FloatToString(1.0f / Editor.SnapDivisor);

        protected override bool OnScroll(ScrollEvent e) {
            if (e.ScrollDelta.Y > 0) {
                Editor.SnapDivisorIncrease();
            } else {
                Editor.SnapDivisorDecrease();
            }
            return true;
        }
    }
}
