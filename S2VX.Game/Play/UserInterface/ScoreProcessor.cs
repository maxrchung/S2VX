﻿using osu.Framework.Allocation;
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
        }

        public void ProcessHit(double scoreTime, double noteHitTime) {
            var notes = Story.Notes;
            var relativeTime = scoreTime - noteHitTime;
            var score = Math.Abs(scoreTime - noteHitTime);

            if (relativeTime < -notes.MissThreshold) { // Before miss
                return;

            } else if (relativeTime < -notes.HitThreshold) { // Early miss
                AddScore(notes.MissThreshold);
                Cursor.UpdateColor(notes.MissColor);
                Miss.Play();

            } else if (relativeTime < -notes.PerfectThreshold) { // Early
                AddScore(score);
                Cursor.UpdateColor(notes.EarlyColor);
                Hit.Play();

            } else if (relativeTime < notes.PerfectThreshold) { // Perfect
                AddScore(score);
                Cursor.UpdateColor(notes.PerfectColor);
                Hit.Play();

            } else if (relativeTime < notes.HitThreshold) { // Late
                AddScore(score);
                Cursor.UpdateColor(notes.LateColor);
                Hit.Play();

            } else { // Late miss and beyond
                AddScore(notes.MissThreshold);
                Cursor.UpdateColor(notes.MissColor);
                Miss.Play();
            }
        }

        public void ProcessHold(double scoreTime, double previousPressTime, bool isPress, double noteHitTime, double noteEndTime) {
            var notes = Story.Notes;
            // Score is dependent on the previous press kept track by the GameHoldNote 
            var score = scoreTime - previousPressTime;

            if (scoreTime < noteHitTime) { // Before during
                return;

            } else if (scoreTime < noteEndTime) { // During
                if (isPress) {
                    Cursor.UpdateColor(notes.LateColor);
                    Hit.Play();
                } else {
                    // Only update score on release
                    AddScore(score);
                    Cursor.UpdateColor(notes.MissColor);
                    Miss.Play();
                }

            } else { // After hold period
                if (isPress) {
                    Hit.Play();
                } else {
                    AddScore(score);
                    Cursor.UpdateColor(notes.MissColor);
                    Miss.Play();
                }
            }
        }
    }
}
