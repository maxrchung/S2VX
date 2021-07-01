using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game.Leaderboard {
    public class AddLeaderboardEntryContainer : FillFlowContainer {

        private LeaderboardContainer LeaderboardContainer { get; }
        private double Score { get; }
        public BasicTextBox NameInput { get; } = new() {
            Size = new Vector2(InputWidth, InputHeight),
            BorderColour = Color4.Red,
            Masking = true
        };

        private const float InputWidth = 200.0f;
        private const float InputHeight = 100.0f;

        public AddLeaderboardEntryContainer(LeaderboardContainer leaderboardContainer, double score) {
            LeaderboardContainer = leaderboardContainer;
            Score = score;
        }

        [BackgroundDependencyLoader]
        private void Load() {
            var textSize = SizeConsts.TextSize1;
            AutoSizeAxes = Axes.Both;
            Child = new Container {
                AutoSizeAxes = Axes.Both,
                Children = new Drawable[] {
                    NameInput,
                    new IconButton {
                        Action = () => LeaderboardContainer.AddEntry(NameInput.Text, Score),
                        Width = InputWidth / 4,
                        Height = InputHeight,
                        Icon = FontAwesome.Solid.Save
                    }
                }
            };
        }
    }
}
