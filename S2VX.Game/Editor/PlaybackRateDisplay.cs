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

        private SpriteText TxtPlaybackRate { get; set; } = new SpriteText() {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Font = new FontUsage("default", 30, "500")
        };

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            Height = 0.075f;
            Width = 1.0f;
            Anchor = Anchor.BottomRight;
            Origin = Anchor.BottomRight;

            InternalChildren = new Drawable[]
            {
                TxtPlaybackRate
            };
        }

        protected override void Update() {
            TxtPlaybackRate.Text = $"Playback Rate: {Editor.Track.Rate.ToString("P0", CultureInfo.InvariantCulture)}";
            TxtPlaybackRate.Font = TxtPlaybackRate.Font.With(size: Editor.DrawWidth / 40);
        }

        protected override bool OnScroll(ScrollEvent e) {
            if (e.ScrollDelta.Y > 0) {
                //Editor.PlaybackIncreaseRate();
            } else {
                //Editor.PlaybackDecreaseRate();
            }
            return true;
        }
    }
}
