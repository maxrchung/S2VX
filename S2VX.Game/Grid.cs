using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Transforms;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game
{
    public class Grid : CompositeDrawable
    {
        private Camera camera = new Camera();

        [BackgroundDependencyLoader]
        private void load(Camera camera)
        {
            RelativeSizeAxes = Axes.Both;
            this.camera = camera;
        }

        protected override void Update()
        {
            var color = Color4.White;
            var cellWidth = 0.1f;
            var edge = 1.0f;
            var lineWidth = edge * 2;
            var lineHeight = 0.005f;

            var position = camera.Position;
            var rotation = camera.Rotation;

            var closest = new Vector2(
                (float)Math.Round(position.X),
                (float)Math.Round(position.Y)
            );
            var offset = Utils.Rotate((closest - position) * cellWidth, rotation);

            var rotationX = Utils.Rotate(new Vector2(1, 0), rotation);
            var rotationY = Utils.Rotate(new Vector2(0, 1), rotation);

            var grid = new List<Drawable>();
            for (var i = cellWidth / 2; i <= edge; i += cellWidth)
            {
                var up = rotationY * i + offset;
                var down = -rotationY * i + offset;
                var right = rotationX * i + offset;
                var left = -rotationX * i + offset;

                grid.Add(new RelativeBox
                {
                    Colour = color,
                    Position = up,
                    Width = lineWidth,
                    Height = lineHeight,
                    Rotation = rotation
                });
                grid.Add(new RelativeBox
                {
                    Colour = color,
                    Position = down,
                    Width = lineWidth,
                    Height = lineHeight,
                    Rotation = rotation
                });
                grid.Add(new RelativeBox
                {
                    Colour = color,
                    Position = right,
                    Width = lineHeight,
                    Height = lineWidth,
                    Rotation = rotation
                });
                grid.Add(new RelativeBox
                {
                    Colour = color,
                    Position = left,
                    Width = lineHeight,
                    Height = lineWidth,
                    Rotation = rotation
                });
            }
            grid.Add(new RelativeBox
            {
                Colour = Color4.Green,
                Position = offset,
                Width = 0.01f,
                Height = 0.01f,
                Rotation = rotation
            });
            grid.Add(new RelativeBox
            {
                Colour = Color4.Red,
                Position = Vector2.Zero,
                Width = 0.01f,
                Height = 0.01f,
                Rotation = rotation
            });

            InternalChildren = grid;

            base.Update();
        }
    }
}
