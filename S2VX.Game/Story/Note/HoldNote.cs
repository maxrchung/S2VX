using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Lines;
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

        // Total distance of the hold note
        // Calculating this once on initialization will optimize position calculations during update
        private float TotalDistance { get; set; }
        private void CalculateTotalDistance() {
            var allCoordinates = MidCoordinates.Append(EndCoordinates);
            var startCoordinate = Coordinates;
            foreach (var endCoordinate in allCoordinates) {
                var distance = (endCoordinate - startCoordinate).Length;
                TotalDistance += distance;
                startCoordinate = endCoordinate;
            }
        }

        private Vector2 CalculateCurrentPosition(double time) {
            // Find distance based on given time
            var offsetFraction = S2VXUtils.ClampedInterpolation(time, 0, 1, HitTime, EndTime);
            var offsetDistance = (float)offsetFraction * TotalDistance;

            // Calculate offset position
            var allCoordinates = MidCoordinates.Append(EndCoordinates);
            var startCoordinate = Coordinates;
            var totalDistance = 0f;
            foreach (var endCoordinate in allCoordinates) {
                var distance = (endCoordinate - startCoordinate).Length;

                // If our position is within the current coordinate
                if (totalDistance + distance > offsetDistance) {
                    var remainingDistance = offsetDistance - totalDistance;
                    var offsetCoordinates = S2VXUtils.ClampedInterpolation(remainingDistance, startCoordinate, endCoordinate, 0, distance);
                    return offsetCoordinates;
                } else {
                    totalDistance += distance;
                    startCoordinate = endCoordinate;
                }
            }
            return EndCoordinates;
        }

        [Resolved]
        private S2VXStory Story { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            CalculateTotalDistance();
            AddInternal(SliderPath);
        }

        public Path SliderPath { get; set; } = new() {
            // Show slider path behind note
            Depth = 100,
            Anchor = Anchor.Centre
        };

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

            var currCoordinates = CalculateCurrentPosition(Time.Current);
            Position = S2VXUtils.Rotate(currCoordinates - camera.Position, Rotation) * Size.X;
        }

        protected Vector2 GetVertexEndPosition(float noteWidth, double time) {
            var currCoordinates = S2VXUtils.ClampedInterpolation(time, Coordinates, EndCoordinates, HitTime, EndTime);
            return (EndCoordinates - currCoordinates) * noteWidth;
        }

        private void UpdateSliderPath() {
            var drawWidth = S2VXGameBase.GameWidth;
            var pathRadius = OutlineThickness * drawWidth / 2;
            SliderPath.PathRadius = pathRadius;
            SliderPath.Colour = Story.Notes.HoldNoteOutlineColor;

            var time = Time.Current;
            var notes = Story.Notes;
            var camera = Story.Camera;
            var noteWidth = camera.Scale.X * drawWidth;
            Vector2 endPosition;
            // If between Fade in time and Hit time, then snake out the slider end
            if (time < HitTime) {
                var startTime = HitTime - (EndTime - HitTime);
                var snakeCoordinates = S2VXUtils.ClampedInterpolation(time, Vector2.Zero, EndCoordinates - Coordinates, startTime, HitTime);
                endPosition = snakeCoordinates * noteWidth;
            }
            // Otherwise, snake in the slider start
            else {
                endPosition = GetVertexEndPosition(noteWidth, time);
            }

            var midPosition = endPosition / 2;
            // Account for pathRadius in note width calculation
            var fittedWidth = camera.Scale.X * drawWidth - pathRadius * 2;
            var noteHalf = fittedWidth / 2;

            var vertices = new List<Vector2>() {
                new(-noteHalf, -noteHalf),
                new(noteHalf, -noteHalf),
                new(noteHalf, noteHalf),
                new(-noteHalf, noteHalf),
                endPosition + new Vector2(-noteHalf, -noteHalf),
                endPosition + new Vector2(noteHalf, -noteHalf),
                endPosition + new Vector2(noteHalf, noteHalf),
                endPosition + new Vector2(-noteHalf, noteHalf)
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
            // ?????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????
            // wtf this black magic shit is wack
            // https://github.com/ppy/osu/blob/86cf42d6107246842a30d9802a4a4fc1c09720c7/osu.Game.Rulesets.Osu/Skinning/Default/SnakingSliderBody.cs#L149
            SliderPath.Position = -SliderPath.PositionInBoundingBox(Vector2.Zero);
        }
    }
}
