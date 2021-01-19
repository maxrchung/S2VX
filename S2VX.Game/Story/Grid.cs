using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;

namespace S2VX.Game.Story {
    public class Grid : CompositeDrawable {
        public float Thickness { get; set; }

        private float LineLength { get; } = 2;

        [Resolved]
        private S2VXStory Story { get; set; } = null;

        [BackgroundDependencyLoader]
        private void Load() => RelativeSizeAxes = Axes.Both;

        protected override void Update() {
            if (IsHidden()) {
                if (InternalChildren.Count != 0) {
                    ClearInternal();
                }
                return;
            }

            var camera = Story.Camera;
            var position = camera.Position;
            var rotation = camera.Rotation;
            var scale = camera.Scale.X;

            var cameraOffset = CalculateCameraOffset(position, rotation, scale);
            var unitX = S2VXUtils.Rotate(new Vector2(1, 0), rotation);
            var unitY = S2VXUtils.Rotate(new Vector2(0, 1), rotation);

            var startDistance = scale / 2;
            var endDistance = LineLength / 2;
            var distanceIncrement = scale;
            var lineIndex = 0;

            for (var distance = startDistance; distance <= endDistance; distance += distanceIncrement) {
                var up = unitY * distance + cameraOffset;
                var down = -unitY * distance + cameraOffset;
                var right = unitX * distance + cameraOffset;
                var left = -unitX * distance + cameraOffset;

                UpdateLineProperties(lineIndex++, up, LineLength, Thickness, rotation);
                UpdateLineProperties(lineIndex++, down, LineLength, Thickness, rotation);
                UpdateLineProperties(lineIndex++, right, Thickness, LineLength, rotation);
                UpdateLineProperties(lineIndex++, left, Thickness, LineLength, rotation);
            }

            CheckAndRemoveLines(lineIndex);
        }

        private bool IsHidden() => Alpha <= 0 || Thickness <= 0;

        private static Vector2 CalculateCameraOffset(Vector2 position, float rotation, float scale) {
            var closestCoordinate = new Vector2(
                (float)Math.Round(position.X),
                (float)Math.Round(position.Y)
            );
            var offset = S2VXUtils.Rotate(closestCoordinate - position, rotation) * scale;
            return offset;
        }

        private void UpdateLineProperties(int index, Vector2 position, float width, float height, float rotation) {
            CheckAndAddLine(index);

            var line = InternalChildren[index];
            line.Position = position;
            line.Width = width;
            line.Height = height;
            line.Rotation = rotation;
        }

        private void CheckAndAddLine(int index) {
            if (InternalChildren.Count <= index) {
                AddInternal(new RelativeBox());
            }
        }

        private void CheckAndRemoveLines(int count) {
            if (InternalChildren.Count > count) {
                var numberToRemove = InternalChildren.Count - count;
                for (var i = 0; i < numberToRemove; ++i) {
                    RemoveInternal(InternalChildren[InternalChildren.Count - 1]);
                }
            }
        }
    }
}
