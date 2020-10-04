using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using System.Globalization;

namespace S2VX.Game.Play.UserInterface {
    public class ScoreInfo : CompositeDrawable {
        [Resolved]
        private ScreenStack Screens { get; set; }

        private int Score { get; set; }

        private TextFlowContainer TxtScore { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            Margin = new MarginPadding {
                Horizontal = Screens.DrawWidth / 60,
            };
            InternalChildren = new Drawable[] {
                TxtScore = new TextFlowContainer(s => s.Font = new FontUsage("default", Screens.DrawWidth / 20)) {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    TextAnchor = Anchor.CentreRight,
                }
            };
        }

        public void AddScore(int moreScore) {
            Score += moreScore;
            TxtScore.Text = Score.ToString(CultureInfo.InvariantCulture);
        }
    }
}
