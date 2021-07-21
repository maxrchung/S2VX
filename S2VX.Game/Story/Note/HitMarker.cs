using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace S2VX.Game.Story.Note {
    public class HitMarker : CompositeDrawable {

        [Resolved]
        private S2VXStory Story { get; set; }

        public Vector2 Coordinates { get; init; }
        public double SpawnTime { get; init; }
        public float MarkerAlpha { get; init; }  // Maximum alpha from which the marker will start to fade out
        private RelativeBox Marker { get; } = new();

        [BackgroundDependencyLoader]
        private void Load() {
            RelativePositionAxes = Axes.Both;
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            InternalChildren = new[] {
                Marker
            };
        }

        /// <summary>
        /// Update loop for the hit marker
        /// </summary>
        /// <returns>Whether or not the marker should be removed</returns>
        public bool UpdateMarker() {
            if (Alpha == 0) {
                return true;
            }
            UpdatePosition();
            UpdateAlpha();
            return false;
        }

        private void UpdateAlpha() => Alpha = S2VXUtils.ClampedInterpolation(Time.Current, MarkerAlpha, 0.0f, SpawnTime, SpawnTime + 200);

        /// <summary>
        /// Updates a hit marker's position/rotation/size
        /// </summary>
        private void UpdatePosition() {
            var camera = Story.Camera;
            Rotation = camera.Rotation;
            Size = camera.Scale;

            var cameraFactor = 1 / camera.Scale.X;
            Marker.Size = Vector2.One / 2 - cameraFactor * new Vector2(Story.Grid.Thickness);

            Position = S2VXUtils.Rotate(Coordinates - camera.Position, Rotation) * Size.X;
        }
    }
}
