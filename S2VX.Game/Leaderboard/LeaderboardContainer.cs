using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using S2VX.Game.Play.Score;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace S2VX.Game.Leaderboard {
    public class LeaderboardContainer : BasicScrollContainer {

        private const string DefaultLeaderboardFileName = "leaderboard.json";
        public string LeaderboardPath { get; }
        private ScoreStatistics ScoreStatistics { get; }

        private List<LeaderboardEntry> LeaderboardData { get; set; }
        private TextFlowContainer NameColumn { get; set; }
        private TextFlowContainer ScoreColumn { get; set; }
        public int EntryCount { get; set; }

        /// <summary>
        /// Basic usage:
        ///     For a read-only leaderboard (i.e. hide AddLeaderboardEntryContainer), leave scoreStatistics null
        ///     For a leaderboard with a new score to add, pass in scoreStatistics
        /// </summary>
        /// <param name="storyDirectory">Directory of the story and leaderboard, not including the file name</param>
        /// <param name="leaderboardFileName">(Optional) File name to use for the JSON leaderboard</param>
        /// <param name="scoreStatistics">(Optional) The new score to add to the leaderboard. Leave null to hide the AddLeaderboardEntryContainer</param>
        public LeaderboardContainer(string storyDirectory, string leaderboardFileName = DefaultLeaderboardFileName, ScoreStatistics scoreStatistics = null) {
            LeaderboardPath = Path.Combine(storyDirectory, leaderboardFileName);
            ScoreStatistics = scoreStatistics;
        }

        [BackgroundDependencyLoader]
        private void Load() {
            var textSize = SizeConsts.TextSize1;
            NameColumn = new TextFlowContainer(s => s.Font = new FontUsage("default", textSize)) {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                TextAnchor = Anchor.TopLeft,
                Colour = Color4.White,
                Margin = new MarginPadding {
                    Horizontal = textSize / 2,
                },
            };
            ScoreColumn = new TextFlowContainer(s => s.Font = new FontUsage("default", textSize)) {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                TextAnchor = Anchor.TopRight,
                Colour = Color4.White,
                Margin = new MarginPadding {
                    Horizontal = textSize / 2,
                },
            };
            var nameAndScore = new Container {
                Width = Width,
                AutoSizeAxes = Axes.Y,
                Children = new[] {
                    NameColumn,
                    ScoreColumn
                },
            };

            if (ScoreStatistics == null) {
                // Read-only leaderboard
                Child = nameAndScore;
            } else {
                Child = new FillFlowContainer {
                    Width = Width,
                    AutoSizeAxes = Axes.Y,
                    Children = new Drawable[] {
                        new AddLeaderboardEntryContainer(this, ScoreStatistics.Score),
                        nameAndScore
                    }
                };
            }

            if (File.Exists(LeaderboardPath)) {
                LoadLeaderboard();
            }
        }

        public void LoadLeaderboard() {
            LeaderboardData?.Clear();
            NameColumn.Clear();
            ScoreColumn.Clear();
            EntryCount = 0;
            var text = File.ReadAllText(LeaderboardPath);
            try {
                LeaderboardData = JsonConvert.DeserializeObject<List<LeaderboardEntry>>(text);
                foreach (var entry in LeaderboardData) {
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

        public void AddEntry(string name, double score) {
            var entry = new LeaderboardEntry(name, Math.Round(score).ToString(CultureInfo.InvariantCulture));
            LeaderboardData.Add(entry);
            LeaderboardData.Sort();
            File.WriteAllText(LeaderboardPath, JsonConvert.SerializeObject(LeaderboardData));
            LoadLeaderboard(); // Reload the leaderboard
        }
    }
}
