using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osuTK;
using System.Collections.Generic;
using System.Linq;

namespace S2VX.Game.Story.Note {
    public abstract class HoldNote : S2VXNote {
        public double EndTime { get; set; }
        public List<Vector2> MidCoordinates { get; } = new() {
            new Vector2(2, -2),
            new Vector2(3, -1)
        };
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

        // MidCoordinates + EndCoordinates
        // Used for calculations during update
        private IEnumerable<Vector2> AllCoordinates { get; set; }
        private void InitAllCoordinates() => AllCoordinates = MidCoordinates.Append(EndCoordinates);

        // Total distance of the hold note
        // Calculating this once on initialization will optimize position calculations during update
        private float TotalDistance { get; set; }
        private void InitTotalDistance() {
            var startCoordinate = Coordinates;
            foreach (var coordinate in AllCoordinates) {
                var distance = (coordinate - startCoordinate).LengthFast;
                TotalDistance += distance;
                startCoordinate = coordinate;
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
        private void Load() {
            InitAllCoordinates();
            InitTotalDistance();
            AddInternal(SliderPath);
        }

        protected override void UpdatePosition() {
            UpdateIndicator();
            UpdateSliderPath();
        }

        private void UpdateIndicator() {
            var camera = Story.Camera;
            Rotation = camera.Rotation;
            Size = camera.Scale;

            var cameraFactor = 1 / camera.Scale.X;
            BoxOuter.Size = Vector2.One - cameraFactor * new Vector2(Story.Grid.Thickness);
            BoxInner.Size = BoxOuter.Size - 2 * cameraFactor * new Vector2(OutlineThickness);

            var currCoordinates = IndicatorCoordinates();
            Position = S2VXUtils.Rotate(currCoordinates - camera.Position, Rotation) * Size.X;
        }

        private Vector2 IndicatorCoordinates() {
            var startFraction = S2VXUtils.ClampedInterpolation(Time.Current, 0, 1, HitTime, EndTime);
            var startDistance = (float)startFraction * TotalDistance;

            var startCoordinate = Coordinates;
            var totalDistance = 0f;
            foreach (var coordinates in AllCoordinates) {
                var distance = (coordinates - startCoordinate).Length;

                if (totalDistance + distance > startDistance) {
                    var remainingDistance = startDistance - totalDistance;
                    var startCoordinates = S2VXUtils.ClampedInterpolation(remainingDistance, startCoordinate, coordinates, 0, distance);
                    return startCoordinates;
                } else {
                    totalDistance += distance;
                    startCoordinate = coordinates;
                }
            }
            return EndCoordinates;
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
            var endDistance = (float)endFraction * TotalDistance;
            var noteWidth = Story.Camera.Scale.X * S2VXGameBase.GameWidth;

            vertices.Add(Vector2.Zero);

            var startCoordinates = Coordinates;
            var totalDistance = 0f;
            foreach (var coordinates in AllCoordinates) {
                var distance = (coordinates - startCoordinates).LengthFast;

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
            var startDistance = (float)startFraction * TotalDistance;
            var noteWidth = Story.Camera.Scale.X * S2VXGameBase.GameWidth;

            vertices.Add(Vector2.Zero);

            var startCoordinates = Coordinates;
            var totalDistance = 0f;
            var hasFoundStart = false;
            var offsetCoordinates = new Vector2();
            foreach (var coordinates in AllCoordinates) {
                var distance = (coordinates - startCoordinates).LengthFast;

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
