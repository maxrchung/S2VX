using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using System.Globalization;

namespace S2VX.Game.Editor {
    public class VolumeDisplay : CompositeDrawable {
        [Resolved]
        private S2VXEditor Editor { get; set; }

        private TextFlowContainer TxtVolume { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;
            Anchor = Anchor.TopRight;
            Origin = Anchor.TopRight;
            Height = 0.03f;
            Width = 0.15f;
            Y = 0.075f;
            Margin = new MarginPadding { Vertical = 24 };

            InternalChildren = new Drawable[]
            {
                TxtVolume = new TextFlowContainer(s => s.Font = new FontUsage("default", Editor.DrawWidth / 40, "500")) {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    TextAnchor = Anchor.Centre,
                }
            };
        }

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
