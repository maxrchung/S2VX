using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Utils;
using osuTK;
using System.Collections.Generic;

namespace S2VX.Game.Story.Note {
    public class Approaches : CompositeDrawable {
        public List<Approach> Children { get; private set; } = new List<Approach>();
        public void SetChildren(List<Approach> approaches) {
            Children = approaches;
            InternalChildren = Children;
        }

        public float Distance { get; set; }
        public float Thickness { get; set; }

        public Approach AddApproach(S2VXNote note) {
            var approach = new Approach {
                Coordinates = note.Coordinates,
                HitTime = note.HitTime
            };
            Children.Add(approach);
            AddInternal(approach);
            return approach;
        }

        public HoldApproach AddHoldApproach(HoldNote note) {
            var approach = new HoldApproach {
                Coordinates = note.Coordinates,
                HitTime = note.HitTime,
                EndTime = note.EndTime,
            };
            Children.Add(approach);
            AddInternal(approach);
            return approach;
        }

        public void RemoveApproach(S2VXNote note) {
            var approach = note.Approach;
            Children.Remove(approach);
            RemoveInternal(approach);
        }

        [Resolved]
        private S2VXStory Story { get; set; } = null;

        [BackgroundDependencyLoader]
        private void Load() => RelativeSizeAxes = Axes.Both;

        private void UpdateApproach(Approach approach, double approachTime, List<RelativeBox> approachLines) {
            var notes = Story.Notes;
            var camera = Story.Camera;
            var approaches = Story.Approaches;

            var time = Time.Current;
            var endFadeOut = approachTime + notes.FadeOutTime;

            if (time >= endFadeOut) {
                approachLines.ForEach(l => l.Alpha = 0);
                // Return early to save some calculations
                return;
            }

            var startTime = approachTime - notes.ShowTime;
            var startFadeIn = startTime - notes.FadeInTime;

            var position = camera.Position;
            var rotation = camera.Rotation;
            var scale = camera.Scale;
            var thickness = approaches.Thickness;

            var offset = S2VXUtils.Rotate(approach.Coordinates - position, rotation) * scale;

            var distance = time < approachTime
                ? Interpolation.ValueAt(time, approaches.Distance, scale.X / 2, startFadeIn, approachTime)
                : scale.X / 2;
            var rotationX = S2VXUtils.Rotate(new Vector2(distance, 0), rotation);
            var rotationY = S2VXUtils.Rotate(new Vector2(0, distance), rotation);

            // Add extra thickness so corners overlap
            var overlap = distance * 2 + thickness;

            approachLines[0].Position = offset + rotationY;
            approachLines[0].Rotation = rotation;
            approachLines[0].Size = new Vector2(overlap, thickness);

            approachLines[1].Position = offset - rotationY;
            approachLines[1].Rotation = rotation;
            approachLines[1].Size = new Vector2(overlap, thickness);

            approachLines[2].Position = offset + rotationX;
            approachLines[2].Rotation = rotation;
            approachLines[2].Size = new Vector2(thickness, overlap);

            approachLines[3].Position = offset - rotationX;
            approachLines[3].Rotation = rotation;
            approachLines[3].Size = new Vector2(thickness, overlap);

            float alpha;
            if (time >= approachTime) {
                alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, approachTime, endFadeOut);
            } else if (time >= startTime) {
                alpha = 1;
            } else {
                alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startFadeIn, startTime);
            }
            approachLines.ForEach(l => l.Alpha = alpha);
        }

        protected override void Update() {
            foreach (var approach in Children) {
                UpdateApproach(approach, approach.HitTime, approach.Lines);
                // Update release approach if this is a Hold Note
                if (approach is HoldApproach holdApproach) {
                    UpdateApproach(holdApproach, holdApproach.EndTime, holdApproach.ReleaseLines);
                }
            }
        }
    }
}
