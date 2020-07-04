using System;
using System.Collections.Generic;
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
        private GridMoveCommand move = new GridMoveCommand
        {
            StartTime = 0,
            EndTime = 10000,
            StartPosition = new Vector2(0, 0),
            EndPosition = new Vector2(0, 0),
            Easing = Easing.None
        };

        private GridRotateCommand rotate = new GridRotateCommand
        {
            StartTime = 0,
            EndTime = 10000,
            StartRotation = 0.0f,
            EndRotation = 90.0f,
            Easing = Easing.None
        };

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
        }

        protected override void Update()
        {
            var position = move.Apply(Time.Current);
            var rotation = rotate.Apply(Time.Current);

            var color = Color4.White;
            var cellWidth = 0.1f;
            var edge = 1.0f;
            var lineWidth = edge * 2;
            var lineHeight = 0.005f;

            // https://stackoverflow.com/a/25641937/13183186
            var offset = new Vector2(
                position.X - (float)Math.Truncate(position.X),
                position.Y - (float)Math.Truncate(position.Y)
            );
            offset = Vector2.Multiply(offset, cellWidth);

            var rotationX = Utils.Rotate(new Vector2(1, 0), rotation);
            var rotationY = Utils.Rotate(new Vector2(0, 1), rotation);

            var grid = new List<Drawable>();
            for (float i = cellWidth / 2; i <= edge; i += cellWidth)
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
            InternalChildren = grid;
        }
    }
}
