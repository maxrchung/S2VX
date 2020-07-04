using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Transforms;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;
using SixLabors.ImageSharp;

namespace S2VX.Game
{
    public class Grid : CompositeDrawable
    {
        private Camera camera = new Camera();

        private GridAlphaCommand alpha = new GridAlphaCommand
        {
            StartTime = 0,
            EndTime = 30000,
            StartAlpha = 1.0f,
            EndAlpha = 0.1f,
            Easing = Easing.InOutQuad
        };

        private GridColorCommand color = new GridColorCommand
        {
            StartTime = 0,
            EndTime = 10000,
            StartColor = Color4.Coral,
            EndColor = Color4.Black,
            Easing = Easing.InSine
        };

        private GridThicknessCommand thickness = new GridThicknessCommand
        {
            StartTime = 0,
            EndTime = 10000,
            StartThickness = 0.05f,
            EndThickness = 0,
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
            if (Time.Current <= alpha.EndTime)
            {
                Alpha = alpha.Apply(Time.Current);
            }

            if (Time.Current <= color.EndTime)
            {
                Colour = color.Apply(Time.Current);
            }

            var lineThickness = 0.005f;
            if (Time.Current <= thickness.EndTime)
            {
                lineThickness = thickness.Apply(Time.Current);
            }

            var position = camera.Position;
            var rotation = camera.Rotation;
            var scale = camera.Scale.X;

            var edge = 1.0f;
            var lineLength = edge * 2;

            var closest = new Vector2(
                (float)Math.Round(position.X),
                (float)Math.Round(position.Y)
            );
            var offset = Utils.Rotate((closest - position) * scale, rotation);

            var rotationX = Utils.Rotate(new Vector2(1, 0), rotation);
            var rotationY = Utils.Rotate(new Vector2(0, 1), rotation);

            var grid = new List<Drawable>();
            for (var i = scale / 2; i <= edge; i += scale)
            {
                var up = rotationY * i + offset;
                var down = -rotationY * i + offset;
                var right = rotationX * i + offset;
                var left = -rotationX * i + offset;

                grid.Add(new RelativeBox
                {
                    Position = up,
                    Width = lineLength,
                    Height = lineThickness,
                    Rotation = rotation
                });
                grid.Add(new RelativeBox
                {
                    Position = down,
                    Width = lineLength,
                    Height = lineThickness,
                    Rotation = rotation
                });
                grid.Add(new RelativeBox
                {
                    Position = right,
                    Width = lineThickness,
                    Height = lineLength,
                    Rotation = rotation
                });
                grid.Add(new RelativeBox
                {
                    Position = left,
                    Width = lineThickness,
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
