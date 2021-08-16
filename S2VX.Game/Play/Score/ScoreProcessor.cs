using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using S2VX.Game.Story;
using S2VX.Game.Story.Note;
using System;
using System.Globalization;

namespace S2VX.Game.Play.Score {
    /// <summary>
    /// Handles the scoring of game notes and game hold notes - changes cursor
    /// color, plays hit sounds, and updates score amount
    /// </summary>
    public class ScoreProcessor : CompositeDrawable {
        [Resolved]
        private S2VXStory Story { get; set; }

        [Resolved]
        private S2VXCursor Cursor { get; set; }

        public ScoreStatistics ScoreStatistics { get; private set; } = new();
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
                    Text = ScoreStatistics.Score.ToString(CultureInfo.InvariantCulture)
                }
            };
        }

        private void AddScore(double moreScore) {
            ScoreStatistics.Score += moreScore;
            ScoreStatistics.Scores.Add(moreScore);
            TxtScore.Text = $"{Math.Round(ScoreStatistics.Score)}";
        }

        private void AddCombo() {
            if (++ScoreStatistics.Combo > ScoreStatistics.MaxCombo) {
                ScoreStatistics.MaxCombo = ScoreStatistics.Combo;
            }
        }

        private void UpdateMiss(double time, Vector2 notePos) {
            Story.HitMarkers.AddMarker(notePos, Notes.MissColor, time);
            Miss.Play();
            ++ScoreStatistics.MissCount;
            ScoreStatistics.Combo = 0;
        }

        public void Reset() {
            ScoreStatistics = new();
            TxtScore.Text = $"{Math.Round(ScoreStatistics.Score)}";
            Hit.Reset();
            Miss.Reset();
        }

        /// <summary>
        /// Converts current absolute cursor position to story coordinates
        /// </summary>
        /// <returns>Cursor position in story coordinates</returns>
        private Vector2 GetCursorPosition() {
            var relativePosition = (Cursor.ActiveCursor.Position - Story.DrawSize / 2) / Story.DrawWidth;
            var camera = Story.Camera;
            var rotatedPosition = S2VXUtils.Rotate(relativePosition, -camera.Rotation);
            var scaledPosition = rotatedPosition * (1 / camera.Scale.X);
            return scaledPosition + camera.Position;
        }

        public double ProcessHit(double scoreTime, double noteHitTime) {
            var relativeTime = scoreTime - noteHitTime;
            var score = Math.Abs(scoreTime - noteHitTime);

            var cursorPos = GetCursorPosition();

            if (relativeTime < -Notes.MissThreshold) { // Before miss
                return 0;

            } else if (relativeTime < -Notes.HitThreshold) { // Early miss
                AddScore(score);
                UpdateMiss(noteHitTime, cursorPos);

            } else if (relativeTime < -Notes.PerfectThreshold) { // Early
                AddScore(score);
                Story.HitMarkers.AddMarker(cursorPos, Notes.EarlyColor, noteHitTime);
                Hit.Play();
                ++ScoreStatistics.EarlyCount;
                AddCombo();

            } else if (relativeTime < Notes.PerfectThreshold) { // Perfect
                AddScore(score);
                Hit.Play();
                ++ScoreStatistics.PerfectCount;
                AddCombo();

            } else if (relativeTime < Notes.HitThreshold) { // Late
                AddScore(score);
                Story.HitMarkers.AddMarker(cursorPos, Notes.LateColor, noteHitTime);
                Hit.Play();
                ++ScoreStatistics.LateCount;
                AddCombo();

            } else { // Late miss and beyond
                AddScore(Notes.MissThreshold);
                UpdateMiss(noteHitTime, cursorPos);
            }

            return score;
        }

        public double ProcessHold(double scoreTime, double lastReleaseTime, bool isPress, double noteHitTime, double noteEndTime, bool isHovered) {
            var score = 0.0;
            var cursorPos = GetCursorPosition();

            if (scoreTime < noteHitTime) { // Before hold
                // Return early so we don't update score, cursor, or hitsounds
                return score;

            } else if (scoreTime < noteEndTime) { // During hold
                if (isPress) {
                    // Score is dependent on the last release kept track by the GameHoldNote 
                    score = scoreTime - lastReleaseTime;
                    // Only update score on press
                    AddScore(score);
                    Story.HitMarkers.AddMarker(cursorPos, Notes.LateColor, lastReleaseTime);
                } else {
                    UpdateMiss(lastReleaseTime, cursorPos);
                }

            } else { // After hold
                if (isPress) {
                    if (isHovered) {
                        Hit.Play();
                    } else {
                        score = Notes.MissThreshold;
                        AddScore(score);
                        UpdateMiss(noteEndTime, cursorPos);
                    }
                } else {
                    score = noteEndTime - lastReleaseTime;
                    if (!isHovered) {
                        score += Notes.MissThreshold;
                    }
                    AddScore(score);
                    UpdateMiss(noteEndTime, cursorPos);
                }
            }

            return score;
        }
    }
}
