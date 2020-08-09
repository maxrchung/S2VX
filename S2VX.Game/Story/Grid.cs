using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace S2VX.Game.Story
{
    public class Grid : CompositeDrawable
    {
        public float Thickness { get; set; } = 0.005f;

        private float lineLength { get; } = 2;

        [Resolved]
        private S2VXStory story { get; set; } = null;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            AlwaysPresent = true;
        }

        protected override void Update()
        {
            var camera = story.Camera;

            if (Alpha <= 0 || Thickness <= 0)
            {
                return;
            }

            var position = camera.Position;
            var rotation = camera.Rotation;
            var scale = camera.Scale.X;

            var closest = new Vector2(
                (float)Math.Round(position.X),
                (float)Math.Round(position.Y)
            );
            var offset = Utils.Rotate(closest - position, rotation) * scale;

            var rotationX = Utils.Rotate(new Vector2(1, 0), rotation);
            var rotationY = Utils.Rotate(new Vector2(0, 1), rotation);

            var grid = new List<Drawable>();

            for (var i = scale / 2; i <= lineLength / 2; i += scale)
            {
                var up = rotationY * i + offset;
                var down = -rotationY * i + offset;
                var right = rotationX * i + offset;
                var left = -rotationX * i + offset;

                grid.Add(new RelativeBox
                {
                    Position = up,
                    Width = lineLength,
                    Height = Thickness,
                    Rotation = rotation
                });
                grid.Add(new RelativeBox
                {
                    Position = down,
                    Width = lineLength,
                    Height = Thickness,
                    Rotation = rotation
                });
                grid.Add(new RelativeBox
                {
                    Position = right,
                    Width = Thickness,
                    Height = lineLength,
                    Rotation = rotation
                });
                grid.Add(new RelativeBox
                {
                    Position = left,
                    Width = Thickness,
                    Height = lineLength,
                    Rotation = rotation
                });
            }

            grid.Add(new RelativeBox
            {
                Position = offset,
                Width = 0.01f,
                Height = 0.01f,
            });
            grid.Add(new RelativeBox
            {
                Position = Vector2.Zero,
                Width = 0.01f,
                Height = 0.01f,
            });

            InternalChildren = grid;
        }
    }
}
