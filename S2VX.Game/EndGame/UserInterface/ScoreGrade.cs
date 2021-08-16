using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace S2VX.Game.EndGame.UserInterface {
    public class ScoreGrade : CompositeDrawable {
        public string Grade { get; private set; } = "F";
        public bool IsSquared { get; private set; }

        public ScoreGrade(double accuracy, bool isFullCombo) {
            var containerSize = new Vector2(500);
            var gradeSize = new Vector2(200);
            var gradeOffset = new Vector2(50, 0);
            var squareSize = new Vector2(50);
            var squareOffset = new Vector2(75, -75);

            DetermineGrade(accuracy, isFullCombo);

            Origin = Anchor.Centre;
            Position = containerSize / 2;
            Size = containerSize;

            AddInternal(new SpriteText {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = Grade,
                Font = new FontUsage(size: gradeSize.X, weight: "bold"),
                // The Text in SpriteText is always drawn with left alignment.
                // This position adjustment helps center the text.
                Position = gradeOffset,
                Size = gradeSize
            });

            if (IsSquared) {
                AddInternal(new SpriteText {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = "2",
                    Font = new FontUsage(size: squareSize.X),
                    Position = squareOffset,
                    Size = squareSize,
                });
            }
        }

        private void DetermineGrade(double accuracy, bool isFullCombo) {
            if (accuracy == 1 && isFullCombo) {
                Grade = "S";
                IsSquared = true;
            } else if (accuracy >= 0.95 && isFullCombo) {
                Grade = "S";
            } else if (accuracy >= 0.9) {
                Grade = "A";
            } else if (accuracy >= 0.8) {
                Grade = "B";
            } else if (accuracy >= 0.7) {
                Grade = "C";
            } else if (accuracy >= 0.6) {
                Grade = "D";
            } else if (accuracy >= 0.5) {
                Grade = "E";
            }
        }
    }
}
