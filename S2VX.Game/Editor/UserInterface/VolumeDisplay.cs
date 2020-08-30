using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using System.Globalization;

namespace S2VX.Game.Editor.UserInterface {
    public class VolumeDisplay : CompositeDrawable {
        [Resolved]
        private S2VXEditor Editor { get; set; }

        private TextFlowContainer TxtVolume { get; set; }

        public Anchor TextAnchor { get; set; }

        [BackgroundDependencyLoader]
        private void Load() =>
            InternalChildren = new Drawable[]
            {
                TxtVolume = new TextFlowContainer(s => s.Font = new FontUsage("default", Editor.DrawWidth / 40, "500")) {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    TextAnchor = TextAnchor,
                }
            };

        protected override void Update() => TxtVolume.Text = $"Volume: {Editor.Track.Volume.Value.ToString("P0", CultureInfo.InvariantCulture)}";

        protected override bool OnScroll(ScrollEvent e) {
            if (e.ScrollDelta.Y > 0) {
                Editor.VolumeIncrease();
            } else {
                Editor.VolumeDecrease();
            }
            return true;
        }
    }
}
