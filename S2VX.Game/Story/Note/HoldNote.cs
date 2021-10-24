using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osuTK;
using System.Collections.Generic;
using System.Linq;

namespace S2VX.Game.Story.Note {
    public abstract class HoldNote : S2VXNote {
        public double EndTime { get; set; }
        public List<Vector2> MidCoordinates { get; } = new();
        public Vector2 EndCoordinates { get; set; }
        protected HoldApproach HoldApproach { get; set; }
        public override Approach Approach {
            get => HoldApproach;
            set {
                if (value is HoldApproach holdApproach) {
                    HoldApproach = holdApproach;
                }
            }
        }

        [Resolved]
        private S2VXStory Story { get; set; }

        public SliderPath SliderPath { get; set; } = new() {
            // Show slider path behind note
            Depth = 100,
            Anchor = Anchor.Centre
        };

        [BackgroundDependencyLoader]
        private void Load() => AddInternal(SliderPath);

        protected override void UpdatePosition() {
            UpdateIndicator();
            UpdateSliderPath();
        }

        // These are static helpers for calculating hold related values
        // TODO: There's probably a better way to share this between HoldApproach and HoldNote for calculating these values
        // TODO: Optimize these so that they are called once on initialization instead of ran every update loop
        private static IEnumerable<Vector2> CombineAllCoordinates(Vector2 coordinates, List<Vector2> midCoordinates, Vector2 endCoordinates)
            => new List<Vector2> { coordinates }.Concat(midCoordinates.Append(endCoordinates));
        private static float CalculateTotalDistance(IEnumerable<Vector2> allCoordinates) {
            var totalDistance = 0f;
            var startCoordinate = allCoordinates.First();
            foreach (var coordinate in allCoordinates.Skip(1)) {
                var distance = (coordinate - startCoordinate).Length;
                totalDistance += distance;
                startCoordinate = coordinate;
            }
            return totalDistance;
        }
        public static Vector2 InterpolateCoordinates(
            double currentTime,
            double hitTime,
            double endTime,
            Vector2 startCoordinates,
            List<Vector2> midCoordinates,
            Vector2 endCoordinates
        ) {
            var initialFraction = S2VXUtils.ClampedInterpolation(currentTime, 0, 1, hitTime, endTime);
            var allCoordinates = CombineAllCoordinates(startCoordinates, midCoordinates, endCoordinates);
            var initialDistance = (float)initialFraction * CalculateTotalDistance(allCoordinates);

            var totalDistance = 0f;
            var start = startCoordinates;
            foreach (var coordinates in allCoordinates.Skip(1)) {
                var distance = (coordinates - start).Length;

                if (totalDistance + distance > initialDistance) {
                    var remainingDistance = initialDistance - totalDistance;
                    var offsetCoordinates = S2VXUtils.ClampedInterpolation(remainingDistance, start, coordinates, 0, distance);
                    return offsetCoordinates;
                } else {
                    totalDistance += distance;
                    start = coordinates;
                }
            }
            return endCoordinates;
        }

        private void UpdateIndicator() {
            var camera = Story.Camera;
            Rotation = camera.Rotation;
            Size = camera.Scale;

            var cameraFactor = 1 / camera.Scale.X;
            BoxOuter.Size = Vector2.One - cameraFactor * new Vector2(Story.Grid.Thickness);
            BoxInner.Size = BoxOuter.Size - 2 * cameraFactor * new Vector2(OutlineThickness);

            var currCoordinates = InterpolateCoordinates(Time.Current, HitTime, EndTime, Coordinates, MidCoordinates, EndCoordinates);
            Position = S2VXUtils.Rotate(currCoordinates - camera.Position, Rotation) * Size.X;
        }

        private void UpdateSliderPath() {
            UpdateVertices();

            SliderPath.PathRadius = Story.Camera.Scale.X * S2VXGameBase.GameWidth / 2;
            SliderPath.OutlineThickness = OutlineThickness / Story.Camera.Scale.X * 2;
            SliderPath.OutlineColor = Story.Notes.HoldNoteOutlineColor;
            // https://github.com/ppy/osu/blob/86cf42d6107246842a30d9802a4a4fc1c09720c7/osu.Game.Rulesets.Osu/Skinning/Default/SnakingSliderBody.cs#L149
            SliderPath.Position = -SliderPath.PositionInBoundingBox(Vector2.Zero);
        }

        private void UpdateVertices() {
            var vertices = Time.Current < HitTime
                ? SnakeOutVertices()
                : SnakeInVertices();
            SliderPath.Vertices = vertices;
        }

        private List<Vector2> SnakeOutVertices() {
            var vertices = new List<Vector2>();
            var startTime = HitTime - (EndTime - HitTime);
            var endFraction = S2VXUtils.ClampedInterpolation(Time.Current, 0, 1, startTime, HitTime);
            var allCoordinates = CombineAllCoordinates(Coordinates, MidCoordinates, EndCoordinates);
            var endDistance = (float)endFraction * CalculateTotalDistance(allCoordinates);
            var noteWidth = Story.Camera.Scale.X * S2VXGameBase.GameWidth;

            vertices.Add(Vector2.Zero);

            var startCoordinates = allCoordinates.First();
            var totalDistance = 0f;
            foreach (var coordinates in allCoordinates.Skip(1)) {
                var distance = (coordinates - startCoordinates).Length;

                if (totalDistance + distance > endDistance) {
                    var remainingDistance = endDistance - totalDistance;
                    var offsetCoordinates = S2VXUtils.ClampedInterpolation(remainingDistance, startCoordinates, coordinates, 0, distance);
                    // Subtract Coordinates so that path is positioned relative to start coordinate
                    vertices.Add((offsetCoordinates - Coordinates) * noteWidth);
                    break;
                } else {
                    vertices.Add((coordinates - Coordinates) * noteWidth);
                    totalDistance += distance;
                    startCoordinates = coordinates;
                }
            }
            return vertices;
        }

        private List<Vector2> SnakeInVertices() {
            var vertices = new List<Vector2>();
            var startFraction = S2VXUtils.ClampedInterpolation(Time.Current, 0, 1, HitTime, EndTime);
            var allCoordinates = CombineAllCoordinates(Coordinates, MidCoordinates, EndCoordinates);
            var startDistance = (float)startFraction * CalculateTotalDistance(allCoordinates);
            var noteWidth = Story.Camera.Scale.X * S2VXGameBase.GameWidth;

            vertices.Add(Vector2.Zero);

            var startCoordinates = allCoordinates.First();
            var totalDistance = 0f;
            var hasFoundStart = false;
            var offsetCoordinates = new Vector2();
            foreach (var coordinates in allCoordinates.Skip(1)) {
                var distance = (coordinates - startCoordinates).Length;

                if (!hasFoundStart) {
                    if (totalDistance + distance > startDistance) {
                        var remainingDistance = startDistance - totalDistance;
                        offsetCoordinates = S2VXUtils.ClampedInterpolation(remainingDistance, startCoordinates, coordinates, 0, distance);
                        vertices.Add((coordinates - offsetCoordinates) * noteWidth);
                        hasFoundStart = true;
                    }
                } else {
                    vertices.Add((coordinates - offsetCoordinates) * noteWidth);
                }

                totalDistance += distance;
                startCoordinates = coordinates;
            }
            return vertices;
        }
    }
}
