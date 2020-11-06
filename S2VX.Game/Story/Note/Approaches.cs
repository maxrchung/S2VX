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
                EndTime = note.EndTime
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

        protected override void Update() {
            foreach (var approach in Children) {
                var notes = Story.Notes;
                var camera = Story.Camera;
                var approaches = Story.Approaches;

                var time = Time.Current;
                var endFadeOut = approach.EndTime + notes.FadeOutTime;

                if (time >= endFadeOut) {
                    approach.Alpha = 0;
                    // Continue early to save some calculations
                    continue;
                }

                var startTime = approach.EndTime - notes.ShowTime;
                var startFadeIn = startTime - notes.FadeInTime;

                var position = camera.Position;
                var rotation = camera.Rotation;
                var scale = camera.Scale;
                var thickness = approaches.Thickness;

                var offset = S2VXUtils.Rotate(approach.Coordinates - position, rotation) * scale;

                var distance = time < approach.EndTime
                    ? Interpolation.ValueAt(time, approaches.Distance, scale.X / 2, startFadeIn, approach.EndTime)
                    : scale.X / 2;
                var rotationX = S2VXUtils.Rotate(new Vector2(distance, 0), rotation);
                var rotationY = S2VXUtils.Rotate(new Vector2(0, distance), rotation);

                // Add extra thickness so corners overlap
                var overlap = distance * 2 + thickness;

                approach.Lines[0].Position = offset + rotationY;
                approach.Lines[0].Rotation = rotation;
                approach.Lines[0].Size = new Vector2(overlap, thickness);

                approach.Lines[1].Position = offset - rotationY;
                approach.Lines[1].Rotation = rotation;
                approach.Lines[1].Size = new Vector2(overlap, thickness);

                approach.Lines[2].Position = offset + rotationX;
                approach.Lines[2].Rotation = rotation;
                approach.Lines[2].Size = new Vector2(thickness, overlap);

                approach.Lines[3].Position = offset - rotationX;
                approach.Lines[3].Rotation = rotation;
                approach.Lines[3].Size = new Vector2(thickness, overlap);

                if (time >= approach.EndTime) {
                    var alpha = Interpolation.ValueAt(time, 1.0f, 0.0f, approach.EndTime, endFadeOut);
                    approach.Alpha = alpha;
                } else if (time >= startTime) {
                    approach.Alpha = 1;
                } else {
                    var alpha = Interpolation.ValueAt(time, 0.0f, 1.0f, startFadeIn, startTime);
                    approach.Alpha = alpha;
                }
            }
        }
    }
}
