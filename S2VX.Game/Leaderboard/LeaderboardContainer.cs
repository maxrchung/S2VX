using Newtonsoft.Json;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using System.IO;

namespace S2VX.Game.Leaderboard {
    public class LeaderboardContainer : BasicScrollContainer {

        private const string DefaultLeaderboardFileName = "leaderboard.json";
        private TextFlowContainer NameColumn { get; set; }
        private TextFlowContainer ScoreColumn { get; set; }
        public int EntryCount { get; set; }

        // Width is needed in the constructor so we know where to draw the ScoreColumn
        public LeaderboardContainer(string storyPath, float width, string leaderboardFileName = DefaultLeaderboardFileName) {
            var textSize = SizeConsts.TextSize1;
            Child = new Container {
                Width = width,
                AutoSizeAxes = Axes.Y,
                Children = new[] {
                    NameColumn = new TextFlowContainer(s => s.Font = new FontUsage("default", textSize)) {
                        AutoSizeAxes = Axes.Both,
                        Anchor = Anchor.TopLeft,
                        Origin = Anchor.TopLeft,
                        TextAnchor = Anchor.TopLeft,
                        Colour = Color4.White,
                        Margin = new MarginPadding {
                            Horizontal = textSize / 2,
                        },
                    },
                    ScoreColumn = new TextFlowContainer(s => s.Font = new FontUsage("default", textSize)) {
                        AutoSizeAxes = Axes.Both,
                        Anchor = Anchor.TopRight,
                        Origin = Anchor.TopRight,
                        TextAnchor = Anchor.TopRight,
                        Colour = Color4.White,
                        Margin = new MarginPadding {
                            Horizontal = textSize / 2,
                        },
                    },
                },
            };

            var leaderboardPath = Path.Combine(Path.GetDirectoryName(storyPath), leaderboardFileName);
            if (File.Exists(leaderboardPath)) {
                LoadLeaderboard(leaderboardPath);
            }
        }


        private void LoadLeaderboard(string leaderboardPath) {
            var text = File.ReadAllText(leaderboardPath);
            var data = JsonConvert.DeserializeObject<LeaderboardEntries>(text);
            try {
                foreach (var entry in data.Entries) {
                    NameColumn.AddParagraph(entry.Name);
                    ScoreColumn.AddParagraph(entry.Score);
                    ++EntryCount;
                }
            } catch (System.NullReferenceException) {
                NameColumn.AddParagraph("Malformed or corrupted Leaderboard file!");
                EntryCount = -1;
            }
        }
    }
}
