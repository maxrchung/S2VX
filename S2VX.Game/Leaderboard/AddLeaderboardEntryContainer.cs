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
        public BasicTextBox NameInput { get; private set; }
        private IconButton SaveButton { get; set; }

        private const float InputWidth = 450.0f;
        private const float InputHeight = 80.0f;

        public AddLeaderboardEntryContainer(LeaderboardContainer leaderboardContainer, double score) {
            LeaderboardContainer = leaderboardContainer;
            Score = score;
        }

        [BackgroundDependencyLoader]
        private void Load() {
            var textSize = SizeConsts.TextSize1;
            Width = InputWidth;
            Height = InputHeight;
            AutoSizeAxes = Axes.Both;
            Children = new Drawable[] {
                NameInput = new() {
                    Size = new Vector2(InputWidth * 0.75f, InputHeight),
                    BorderColour = Color4.Red,
                    Masking = true,
                },
                SaveButton = new() {
                    Action = () => {
                        LeaderboardContainer.AddEntry(NameInput.Text, Score);
                        Clear();
                    },
                    Width = InputWidth * 0.25f,
                    Height = InputHeight,
                    Icon = FontAwesome.Solid.Save,
                }
            };
            NameInput.OnCommit += (_, _) => SaveButton.Click();
        }
    }
}
