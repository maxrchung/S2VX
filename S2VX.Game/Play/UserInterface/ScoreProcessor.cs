using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using S2VX.Game.Story;
using System;
using System.Globalization;

namespace S2VX.Game.Play.UserInterface {
    /// <summary>
    /// Handles the scoring of game notes and game hold notes - changes cursor
    /// color, plays hit sounds, and updates score amount
    /// </summary>
    public class ScoreProcessor : CompositeDrawable {
        [Resolved]
        private S2VXStory Story { get; set; }

        [Resolved]
        private S2VXCursor Cursor { get; set; }

        // Score should be a double type because during a drag, score may add
        // very small values that are between 0 and 1. If we int cast or round
        // this drag value, we'll always get 0.
        public double Score { get; private set; }
        private TextFlowContainer TxtScore { get; set; }
        public S2VXSample Hit { get; private set; }
        public S2VXSample Miss { get; private set; }

        public int PerfectCount { get; private set; }
        public int EarlyCount { get; private set; }
        public int LateCount { get; private set; }
        public int MissCount { get; private set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            Hit = new S2VXSample("hit", audio);
            Miss = new S2VXSample("miss", audio);

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
            Hit.Reset();
            Miss.Reset();
            PerfectCount = 0;
            EarlyCount = 0;
            LateCount = 0;
            MissCount = 0;
        }

        public double ProcessHit(double scoreTime, double noteHitTime) {
            var notes = Story.Notes;
            var relativeTime = scoreTime - noteHitTime;
            var score = Math.Abs(scoreTime - noteHitTime);

            if (relativeTime < -notes.MissThreshold) { // Before miss
                return 0;

            } else if (relativeTime < -notes.HitThreshold) { // Early miss
                AddScore(score);
                Cursor.UpdateColor(notes.MissColor);
                Miss.Play();
                ++MissCount;

            } else if (relativeTime < -notes.PerfectThreshold) { // Early
                AddScore(score);
                Cursor.UpdateColor(notes.EarlyColor);
                Hit.Play();
                ++EarlyCount;

            } else if (relativeTime < notes.PerfectThreshold) { // Perfect
                AddScore(score);
                Cursor.UpdateColor(notes.PerfectColor);
                Hit.Play();
                ++PerfectCount;

            } else if (relativeTime < notes.HitThreshold) { // Late
                AddScore(score);
                Cursor.UpdateColor(notes.LateColor);
                Hit.Play();
                ++LateCount;

            } else { // Late miss and beyond
                AddScore(notes.MissThreshold);
                Cursor.UpdateColor(notes.MissColor);
                Miss.Play();
                ++MissCount;
            }

            return score;
        }

        public double ProcessHold(double scoreTime, double lastReleaseTime, bool isPress, double noteHitTime, double noteEndTime) {
            var notes = Story.Notes;
            var score = 0.0;

            if (scoreTime < noteHitTime) { // Before hold
                // Return early so we don't update score, cursor, or hitsounds
                return score;

            } else if (scoreTime < noteEndTime) { // During hold
                if (isPress) {
                    // Score is dependent on the last release kept track by the GameHoldNote 
                    score = scoreTime - lastReleaseTime;
                    // Only update score on press
                    AddScore(score);
                    Cursor.UpdateColor(notes.LateColor);
                } else {
                    Cursor.UpdateColor(notes.MissColor);
                    Miss.Play();
                }

            } else { // After hold
                if (isPress) {
                    Hit.Play();
                } else {
                    score = noteEndTime - lastReleaseTime;
                    AddScore(score);
                    Cursor.UpdateColor(notes.MissColor);
                    Miss.Play();
                }
            }

            return score;
        }
    }
}
