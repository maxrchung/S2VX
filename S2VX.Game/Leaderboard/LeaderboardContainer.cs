using Newtonsoft.Json;
using osu.Framework.Allocation;
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
        private string LeaderboardPath { get; set; }
        private TextFlowContainer NameColumn { get; set; }
        private TextFlowContainer ScoreColumn { get; set; }
        public int EntryCount { get; set; }

        public LeaderboardContainer(string storyDirectory, string leaderboardFileName = DefaultLeaderboardFileName) =>
            LeaderboardPath = Path.Combine(storyDirectory, leaderboardFileName);

        [BackgroundDependencyLoader]
        private void Load() {
            var textSize = SizeConsts.TextSize1;
            Child = new Container {
                Width = Width,
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

            if (File.Exists(LeaderboardPath)) {
                LoadLeaderboard(LeaderboardPath);
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
            } catch (Exception ex) {
                NameColumn.AddParagraph("Error parsing leaderboard!");
                EntryCount = -1;
                Console.WriteLine(ex);
            }
        }
    }
}
