using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Lines;
using osu.Framework.Screens;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;
using System.Collections.Generic;

namespace S2VX.Game.Story.Note {
    public abstract class HoldNote : S2VXNote {
        public double EndTime { get; set; }
        public Vector2 EndCoordinates { get; set; }

        [Resolved]
        private S2VXStory Story { get; set; }

        [Resolved]
        private ScreenStack Screens { get; set; }

        [BackgroundDependencyLoader]
        private void Load() {
            AddInternal(SliderPath1);
            AddInternal(SliderPath2);
            AddInternal(StartPath);
            AddInternal(EndPath);
        }

        private static Path CreatePath() => new Path() {
            Colour = Color4.Red,
            Anchor = Anchor.Centre
        };

        private Path StartPath { get; set; } = CreatePath();

        private Path SliderPath1 { get; set; } = CreatePath();

        private Path SliderPath2 { get; set; } = CreatePath();

        private Path EndPath { get; set; } = CreatePath();

        protected override void UpdatePlacement() {
            var notes = Story.Notes;
            var camera = Story.Camera;
            var grid = Story.Grid;

            Rotation = camera.Rotation;
            Size = camera.Scale;

            var cameraFactor = 1 / camera.Scale.X;
            BoxOuter.Size = Vector2.One - cameraFactor * new Vector2(grid.Thickness);
            BoxInner.Size = BoxOuter.Size - 2 * cameraFactor * new Vector2(notes.OutlineThickness);

            if (Time.Current < HitTime) {
                Position = S2VXUtils.Rotate(Coordinates - camera.Position, Rotation) * Size.X;
            } else if (Time.Current < EndTime) {
                var coordinates = Interpolation.ValueAt(Time.Current, Coordinates, EndCoordinates, HitTime, EndTime);
                Position = S2VXUtils.Rotate(coordinates - camera.Position, Rotation) * Size.X;
            } else {
                Position = S2VXUtils.Rotate(EndCoordinates - camera.Position, Rotation) * Size.X;
            }
            BoxOuter.Colour = notes.OutlineColor;
        }

        protected void UpdateSliderPaths() {
            var pathRadius = 5;
            StartPath.PathRadius = pathRadius;
            EndPath.PathRadius = pathRadius;

            StartPath.Position = new Vector2(-pathRadius);
            EndPath.Position = new Vector2(-pathRadius);
            EndPath.Colour = Color4.Aqua;

            var drawWidth = Screens.DrawWidth;
            var camera = Story.Camera;
            var noteWidth = camera.Scale.X * drawWidth;
            var noteHalf = noteWidth / 2;
            StartPath.Vertices = new List<Vector2>() {
                new Vector2(0),
                new Vector2(250, 50),
                new Vector2(-noteHalf, -noteHalf),
                new Vector2(noteHalf, -noteHalf),
                new Vector2(noteHalf, noteHalf),
                new Vector2(-noteHalf, noteHalf),
                new Vector2(-noteHalf, -noteHalf),
            };

            var currCoordinates = Interpolation.ValueAt(Time.Current, Coordinates, EndCoordinates, HitTime, EndTime);
            var deltaPosition = (EndCoordinates - Coordinates) * noteWidth;
            EndPath.Vertices = new List<Vector2>() {
                new Vector2(0),
                new Vector2(250, 50),
                new Vector2(-noteHalf, -noteHalf),
                new Vector2(noteHalf, -noteHalf),
                new Vector2(noteHalf, noteHalf),
                new Vector2(-noteHalf, noteHalf),
                new Vector2(-noteHalf, -noteHalf),
            };
        }
    }
}
