using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;
using S2VX.Game.Play.UserInterface;

namespace S2VX.Game.Play.Containers {
    public class PlayInfoBar : CompositeDrawable {

        private ScoreDisplay ScoreDisplay { get; } = new ScoreDisplay {
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            Width = 0.2f,
            TextAnchor = Anchor.TopRight,
            Colour = Color4.Black,
        };

        public const float InfoBarHeight = 0.03f;
        public const float InfoBarWidth = 1.0f;

        [BackgroundDependencyLoader]
        private void Load() {
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;
            Height = InfoBarHeight;
            Width = InfoBarWidth;

            InternalChildren = new Drawable[]
            {
                ScoreDisplay,
            };
        }
    }
}
