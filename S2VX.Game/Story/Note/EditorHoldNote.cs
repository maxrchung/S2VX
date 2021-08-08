using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Lines;
using osuTK;
using S2VX.Game.Editor;
using S2VX.Game.Editor.Reversible;
using System.Collections.Generic;
using System.Linq;

namespace S2VX.Game.Story.Note {
    public class EditorHoldNote : HoldNote {
        public S2VXSample Hit { get; private set; }

        private int NumHitSounds { get; set; }
        private List<double> HitSoundTimes { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }
        [Resolved]
        private EditorScreen Editor { get; set; }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio) {
            Hit = new("hit", audio);
            HitSoundTimes = new() { HitTime, EndTime };
            HeadAnchor = new(this, true);
            TailAnchor = new(this, false);
            AddInternal(AnchorPath);
            AddInternal(HeadAnchor);
            AddInternal(MidAnchors);
            AddInternal(TailAnchor);
        }

        private Path AnchorPath { get; set; } = new() {
            Anchor = Anchor.Centre,
            PathRadius = EditorHoldNoteAnchor.AnchorWidth / 2.0f
        };

        private EditorHoldNoteAnchor HeadAnchor { get; set; }

        private EditorHoldNoteAnchor TailAnchor { get; set; }

        public Container<EditorHoldNoteAnchor> MidAnchors { get; } = new() {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre
        };

        public override void UpdateHitTime(double hitTime) {
            EndTime = hitTime + EndTime - HitTime;
            base.UpdateHitTime(hitTime);
            HoldApproach.EndTime = EndTime;
            HitSoundTimes = new() { HitTime, EndTime };
        }

        public override void UpdateEndTime(double endTime) {
            if (endTime < HitTime) {
                return;
            }
            EndTime = endTime;
            HoldApproach.EndTime = EndTime;
            HitSoundTimes = new() { HitTime, EndTime };
        }

        public void UpdateEndCoordinates(Vector2 coordinates) {
            EndCoordinates = coordinates;
            HoldApproach.EndCoordinates = EndCoordinates;
        }

        private void UpdateAnchorPath() {
            // Dynamically add mid anchors since this could be changed during preview
            // Note that we do not handle removal of anchors
            if (MidCoordinates.Count > MidAnchors.Count) {
                for (var i = 0; i < MidCoordinates.Count; ++i) {
                    var midAnchor = new EditorHoldNoteAnchor(this, true);
                    MidAnchors.Add(midAnchor);
                }
            }

            var time = Time.Current;
            var drawWidth = S2VXGameBase.GameWidth;
            var camera = Story.Camera;
            var noteWidth = camera.Scale.X * drawWidth;
            var currCoordinates = InterpolateCoordinates(time, HitTime, EndTime, Coordinates, MidCoordinates, EndCoordinates);

            var vertices = new List<Vector2>() { (Coordinates - currCoordinates) * noteWidth };
            HeadAnchor.Position = vertices.First();
            for (var i = 0; i < MidCoordinates.Count; ++i) {
                var midCoordinates = MidCoordinates[i];
                var offsetPosition = (midCoordinates - currCoordinates) * noteWidth;
                vertices.Add(offsetPosition);
                MidAnchors[i].Position = offsetPosition;
            }
            vertices.Add((EndCoordinates - currCoordinates) * noteWidth);
            TailAnchor.Position = vertices.Last();

            AnchorPath.Vertices = vertices;
            // Explained in HoldNote.cs UpdateSliderPath() Lol
            AnchorPath.Position = -AnchorPath.PositionInBoundingBox(Vector2.Zero);
        }

        public override bool UpdateNote() {
            UpdateColor();
            UpdatePosition();
            UpdateAnchorPath();

            var time = Time.Current;
            // Deduct number of hit sounds to play once we've passed each HitSoundTime
            if (NumHitSounds > 0 && time >= HitSoundTimes[^NumHitSounds]) {
                --NumHitSounds;
                Hit.Play();
            }

            // Reset hit sound counter if clock is running and before timing points
            if (Clock.IsRunning) {
                NumHitSounds = HitSoundTimes.Count - GetNumTimingPointsPassed();
            }

            return false;
        }

        protected override void UpdateColor() {
            var time = Time.Current;
            var notes = Story.Notes;
            var maxAlpha = notes.HoldNoteAlpha;
            InnerColor = notes.HoldNoteColor;
            OutlineColor = notes.HoldNoteOutlineColor;
            OutlineThickness = notes.HoldNoteOutlineThickness;
            // Fade in time to Show time
            if (time < HitTime - notes.ShowTime) {
                var startTime = HitTime - notes.ShowTime - Editor.EditorApproachRate * notes.FadeInTime;
                var endTime = HitTime - notes.ShowTime;
                Alpha = S2VXUtils.ClampedInterpolation(time, 0.0f, maxAlpha, startTime, endTime);
            }
            // Show time (to Hit time) to End time
            else if (time < EndTime) {
                Alpha = maxAlpha;
            }
            // End time to Fade out time
            else if (time < EndTime + Editor.EditorApproachRate * notes.FadeOutTime) {
                var startTime = EndTime;
                var endTime = EndTime + Editor.EditorApproachRate * notes.FadeOutTime;
                Alpha = S2VXUtils.ClampedInterpolation(time, maxAlpha, 0.0f, startTime, endTime);
            } else {
                Alpha = 0;
            }
        }

        private int GetNumTimingPointsPassed() {
            var time = Time.Current;
            var ans = 0;
            foreach (var hitSoundTime in HitSoundTimes) {
                if (time >= hitSoundTime) {
                    ++ans;
                } else {
                    break;
                }
            }
            return ans;
        }

        public override void ReversibleRemove(S2VXStory story, EditorScreen editor) =>
            editor.Reversibles.Push(new ReversibleRemoveHoldNote(story, this, editor));

        public override Approach AddApproach() {
            var approach = new EditorHoldApproach {
                Coordinates = Coordinates,
                HitTime = HitTime,
                EndTime = EndTime,
                EndCoordinates = EndCoordinates
            };
            approach.MidCoordinates.AddRange(MidCoordinates);
            Approach = approach;
            return approach;
        }
    }
}
