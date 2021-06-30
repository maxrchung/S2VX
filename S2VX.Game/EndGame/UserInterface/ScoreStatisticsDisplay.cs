using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using S2VX.Game.Play.Score;

namespace S2VX.Game.EndGame.UserInterface {
    public class ScoreStatisticsDisplay : GridContainer {
        public ScoreStatisticsDisplay(ScoreStatistics scoreStatistics) {
            Content = new Drawable[][] {
                CreateRow("Score", scoreStatistics.Score),
                CreateRow("Max Combo", scoreStatistics.MaxCombo),
                CreateRow("Perfect", scoreStatistics.PerfectCount),
                CreateRow("Early", scoreStatistics.EarlyCount),
                CreateRow("Late", scoreStatistics.LateCount),
                CreateRow("Miss", scoreStatistics.MissCount),
                CreateRow("Accuracy", scoreStatistics.Accuracy),
                CreateRow("Median", scoreStatistics.Median()),
            };
            Y = 450;
            Size = new(450);
        }

        private static Drawable[] CreateRow(string key, object value) {
            var keyDisplay = new SpriteText {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Font = FontUsage.Default.With(weight: "Bold"),
                Text = $"{key}:"
            };
            var valueDisplay = new SpriteText {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                X = 10,
                Text = value.ToString()
            };
            return new[] { keyDisplay, valueDisplay };
        }
    }
}
