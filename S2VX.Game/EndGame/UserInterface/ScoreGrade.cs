using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;

namespace S2VX.Game.EndGame.UserInterface {
    public class ScoreGrade : CompositeDrawable {
        public string Grade { get; private set; } = "F";
        public bool IsSquared { get; private set; }

        public ScoreGrade(double accuracy, bool isFullCombo) {
            DetermineGrade(accuracy, isFullCombo);

            Origin = Anchor.Centre;
            Position = new(250);
            Size = new(500);

            AddInternal(new SpriteText {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = Grade,
                Font = new FontUsage(size: 200, weight: "bold"),
                // The Text in SpriteText is always drawn with left alignment.
                // This position adjustment helps center the text.
                Position = new(50, 0),
                Size = new(200, 200)
            });

            if (IsSquared) {
                AddInternal(new SpriteText {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = "2",
                    Font = new FontUsage(size: 50),
                    Position = new(75, -75),
                    Size = new(50),
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
