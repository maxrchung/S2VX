using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

namespace S2VX.Game.Editor.UserInterface {
    public class NoteSnapDivisorDisplay : CompositeDrawable {
        [Resolved]
        private S2VXEditor Editor { get; set; }

        private TextFlowContainer TxtNoteSnapDivisor { get; set; }

        public Anchor TextAnchor { get; set; }

        [BackgroundDependencyLoader]
        private void Load() =>
            InternalChildren = new Drawable[]
            {
                TxtNoteSnapDivisor = new TextFlowContainer(s => s.Font = new FontUsage("default", Editor.DrawWidth / 40, "500")) {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    TextAnchor = TextAnchor,
                }
            };

        protected override void Update() {
            if (Editor.SnapDivisor == 0) {
                TxtNoteSnapDivisor.Text = "Snap Divisor: Free";
            } else {
                TxtNoteSnapDivisor.Text = "Snap Divisor: " + S2VXUtils.FloatToString(1.0f / Editor.SnapDivisor, 4);
            }
        }

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
