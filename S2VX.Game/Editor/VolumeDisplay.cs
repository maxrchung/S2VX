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

        private SpriteText TxtVolume { get; set; } = new SpriteText() {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Font = new FontUsage("default", 30, "500")
        };

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;
            Height = 0.03f;
            Width = 1.0f;
            X = 0.87f;
            Y = 0.075f;
            Margin = new MarginPadding { Vertical = 24 };

            InternalChildren = new Drawable[]
            {
                TxtVolume
            };
        }

        protected override void Update() {
            TxtVolume.Text = $"Volume: {Editor.Track.Volume.Value.ToString("P0", CultureInfo.InvariantCulture)}";
            TxtVolume.Font = TxtVolume.Font.With(size: Editor.DrawWidth / 40);
        }

        protected override bool OnScroll(ScrollEvent e) {
            if (e.ScrollDelta.Y > 0) {
                Editor.VolumeIncrease10();
            } else {
                Editor.VolumeDecrease10();
            }
            return true;
        }
    }
}
