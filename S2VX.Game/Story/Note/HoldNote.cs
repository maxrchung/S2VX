using osu.Framework.Allocation;
using osu.Framework.Graphics.Lines;
using osu.Framework.Screens;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace S2VX.Game.Story.Note {
    public abstract class HoldNote : S2VXNote {
        public double EndTime { get; set; }
        public Vector2 EndCoordinates { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [Resolved]
        private ScreenStack Screens { get; set; }

        [BackgroundDependencyLoader]
        private void Load() => AddInternal(SliderPath);

        private Path SliderPath { get; set; } = new Path() {
            Colour = Color4.Red
        };

        protected override void UpdatePlacement() {
            var camera = Story.Camera;
            var grid = Story.Grid;

            Rotation = camera.Rotation;
            Size = camera.Scale;

            var cameraFactor = 1 / camera.Scale.X;
            BoxOuter.Size = Vector2.One - cameraFactor * new Vector2(grid.Thickness);
            BoxInner.Size = BoxOuter.Size - 2 * cameraFactor * new Vector2(OutlineThickness);

            if (Time.Current < HitTime) {
                Position = S2VXUtils.Rotate(Coordinates - camera.Position, Rotation) * Size.X;
            } else if (Time.Current < EndTime) {
                var coordinates = Interpolation.ValueAt(Time.Current, Coordinates, EndCoordinates, HitTime, EndTime);
                Position = S2VXUtils.Rotate(coordinates - camera.Position, Rotation) * Size.X;
            } else {
                Position = S2VXUtils.Rotate(EndCoordinates - camera.Position, Rotation) * Size.X;
            }
        }

        protected void UpdateSliderPaths() {
            var pathRadius = 5;
            SliderPath.PathRadius = pathRadius;
            // Account for path radius by setting Position
            SliderPath.Position = new Vector2(-pathRadius);

            var drawWidth = Screens.DrawWidth;
            var camera = Story.Camera;
            var noteWidth = camera.Scale.X * drawWidth;
            var noteHalf = noteWidth / 2;

            var currCoordinates = Interpolation.ValueAt(Time.Current, Coordinates, EndCoordinates, HitTime, EndTime);
            var deltaPosition = (EndCoordinates - currCoordinates) * noteWidth;
            var midPosition = deltaPosition / 2;

            var vertices = new List<Vector2>() {
                new Vector2(-noteHalf, -noteHalf),
                new Vector2(noteHalf, -noteHalf),
                new Vector2(noteHalf, noteHalf),
                new Vector2(-noteHalf, noteHalf),
                deltaPosition + new Vector2(-noteHalf, -noteHalf),
                deltaPosition + new Vector2(noteHalf, -noteHalf),
                deltaPosition + new Vector2(noteHalf, noteHalf),
                deltaPosition + new Vector2(-noteHalf, noteHalf)
            };

            // Sort based on descending distance to midPosition
            vertices.Sort((left, right) =>
                (right - midPosition).LengthSquared.CompareTo(
                    (left - midPosition).LengthSquared
                )
            );

            // Drop the last 2 (the closest) vertices
            vertices.RemoveAt(vertices.Count - 1);
            vertices.RemoveAt(vertices.Count - 1);

            // Sort based on relative angle to unit vector
            vertices.Sort((left, right) =>
                S2VXUtils.AngleBetween(Vector2.UnitX, left - midPosition).CompareTo(
                    S2VXUtils.AngleBetween(Vector2.UnitX, right - midPosition)
                )
            );

            // Connect last vertex back to first
            vertices.Add(vertices.First());

            SliderPath.Vertices = vertices;
        }
    }
}
