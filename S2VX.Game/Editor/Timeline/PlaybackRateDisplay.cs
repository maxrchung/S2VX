using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using System.Globalization;

namespace S2VX.Game.Editor {
    public class PlaybackRateDisplay : CompositeDrawable {
        [Resolved]
        private S2VXEditor Editor { get; set; }

        private TextFlowContainer TxtPlaybackRate { get; set; }

        public Anchor TextAnchor { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.CentreRight;
            Origin = Anchor.CentreRight;
            Width = 0.15f;
            InternalChildren = new Drawable[]
            {
                TxtPlaybackRate = new TextFlowContainer(s => s.Font = new FontUsage("default", Editor.DrawWidth / 40, "500")) {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    TextAnchor = TextAnchor,
                }
            };
        }

        protected override void Update() => TxtPlaybackRate.Text = $"Speed: {Editor.Track.Rate.ToString("P0", CultureInfo.InvariantCulture)}";

        protected override bool OnScroll(ScrollEvent e) {
            if (e.ScrollDelta.Y > 0) {
                Editor.PlaybackIncreaseRate();
            } else {
                Editor.PlaybackDecreaseRate();
            }
            return true;
        }
    }
}
