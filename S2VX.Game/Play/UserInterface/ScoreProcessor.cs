using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using System;
using System.Globalization;

namespace S2VX.Game.Play.UserInterface {
    public class ScoreProcessor : CompositeDrawable {
        // Score should be a double type because during a drag, score may add
        // very small values that are between 0 and 1. If we int cast or round
        // this drag value, we'll always get 0.
        public double Score { get; private set; }

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

        public void AddScore(double moreScore) {
            Score += moreScore;
            TxtScore.Text = $"{Math.Round(Score)}";
        }

        public void Reset() {
            Score = 0;
            TxtScore.Text = $"{Math.Round(Score)}";
        }
    }
}
