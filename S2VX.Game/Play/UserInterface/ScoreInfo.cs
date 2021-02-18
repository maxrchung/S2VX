using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using System.Globalization;

namespace S2VX.Game.Play.UserInterface {
    public class ScoreInfo : CompositeDrawable {
        public int Score { get; private set; }

        private TextFlowContainer TxtScore { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            Margin = new MarginPadding {
                Horizontal = S2VXGameBase.GameWidth / 60,
            };
            InternalChildren = new Drawable[] {
                TxtScore = new TextFlowContainer(s => s.Font = new FontUsage("default", S2VXGameBase.GameWidth / 20)) {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    TextAnchor = Anchor.CentreRight,
                    Text = Score.ToString(CultureInfo.InvariantCulture)
                }
            };
        }

        public void AddScore(int moreScore) {
            Score += moreScore;
            TxtScore.Text = Score.ToString(CultureInfo.InvariantCulture);
        }

        public void ClearScore() {
            Score = 0;
            TxtScore.Text = Score.ToString(CultureInfo.InvariantCulture);
        }
    }
}
