﻿using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Lines;
using osu.Framework.Screens;
using osu.Framework.Utils;
using osuTK;
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

            var clampedTime = MathHelper.Clamp(Time.Current, HitTime, EndTime);
            var currCoordinates = Interpolation.ValueAt(clampedTime, Coordinates, EndCoordinates, HitTime, EndTime);
            Position = S2VXUtils.Rotate(currCoordinates - camera.Position, Rotation) * Size.X;
        }

        private void UpdateSliderPath() {
            var drawWidth = Screens.DrawWidth;
            var pathRadius = OutlineThickness * Screens.DrawWidth / 2;
            SliderPath.PathRadius = pathRadius;

            var time = Time.Current;
            var notes = Story.Notes;
            var camera = Story.Camera;
            var noteWidth = camera.Scale.X * drawWidth;
            Vector2 endPosition;
            // If between Fade in time and Hit time, then snake out the slider end
            if (time < HitTime) {
                var startTime = HitTime - notes.ShowTime - notes.FadeInTime;
                var clampedTime = MathHelper.Clamp(time, startTime, HitTime);
                var snakeCoordinates = Interpolation.ValueAt(clampedTime, Vector2.Zero, EndCoordinates - Coordinates, startTime, HitTime);
                endPosition = snakeCoordinates * noteWidth;
            }
            // Otherwise, snake in the slider start
            else {
                var clampedTime = MathHelper.Clamp(time, HitTime, EndTime);
                var currCoordinates = Interpolation.ValueAt(clampedTime, Coordinates, EndCoordinates, HitTime, EndTime);
                endPosition = (EndCoordinates - currCoordinates) * noteWidth;
            }

            var midPosition = endPosition / 2;
            // Account for pathRadius in note width calculation
            var fittedWidth = camera.Scale.X * drawWidth - pathRadius * 2;
            var noteHalf = fittedWidth / 2;

            var vertices = new List<Vector2>() {
                new Vector2(-noteHalf, -noteHalf),
                new Vector2(noteHalf, -noteHalf),
                new Vector2(noteHalf, noteHalf),
                new Vector2(-noteHalf, noteHalf),
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
