using Newtonsoft.Json;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace S2VX.Game.Leaderboard {
    public class LeaderboardContainer : BasicScrollContainer {

        private const string DefaultLeaderboardFileName = "leaderboard.json";
        private TextFlowContainer NameColumn { get; }
        private TextFlowContainer ScoreColumn { get; }
        public int EntryCount { get; set; }

        // Width is needed in the constructor so we know where to draw the ScoreColumn
        public LeaderboardContainer(string storyPath, float width = 500, string leaderboardFileName = DefaultLeaderboardFileName) {
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
            try {
                var data = JsonConvert.DeserializeObject<IEnumerable<LeaderboardEntry>>(text);
                foreach (var entry in data) {
                    NameColumn.AddParagraph(entry.Name);
                    ScoreColumn.AddParagraph(entry.Score);
                    ++EntryCount;
                }
            } catch (Exception) {
                NameColumn.AddParagraph("Malformed or corrupted Leaderboard file!");
                EntryCount = -1;
            }
        }
    }
}
