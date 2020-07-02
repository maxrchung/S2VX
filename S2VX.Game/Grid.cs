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
        private Camera camera = new Camera();

        private GridMoveCommand command = new GridMoveCommand
        {
            StartTime = 0,
            EndTime = 10000,
            StartPosition = new Vector2(0, 0),
            EndPosition = new Vector2(5, 5),
            Easing = Easing.None
        };

        [BackgroundDependencyLoader]
        private void load(Camera camera)
        {
            RelativeSizeAxes = Axes.Both;
            this.camera = camera;
        }

        protected override void Update()
        {
            var position = command.Apply(Time.Current);

            var cellWidth = 0.02f;
            // https://stackoverflow.com/a/25641937/13183186
            var offset = new Vector2(
                position.X - (float)Math.Truncate(position.X),
                position.Y - (float)Math.Truncate(position.Y)
            );
            offset = Vector2.Multiply(offset, cellWidth);

            var grid = new List<Drawable>();
            for (float i = cellWidth / 2; i <= 0.75f; i += cellWidth)
            {
                grid.Add(new RelativeBox
                {
                    Colour = Color4.White,
                    Y = i + offset.Y,
                    Width = 1.5f,
                    Height = 0.005f
                });
                grid.Add(new RelativeBox
                {
                    Colour = Color4.White,
                    Y = -i + offset.Y,
                    Width = 1.5f,
                    Height = 0.005f
                });
                grid.Add(new RelativeBox
                {
                    Colour = Color4.White,
                    X = i + offset.X,
                    Width = 0.005f,
                    Height = 1.5f
                });
                grid.Add(new RelativeBox
                {
                    Colour = Color4.White,
                    X = -i + offset.X,
                    Width = 0.005f,
                    Height = 1.5f
                });
            }
            InternalChildren = grid;
        }
    }
}
