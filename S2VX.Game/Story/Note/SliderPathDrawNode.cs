// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Batches;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Lines;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;
using System;
using System.Collections.Generic;

namespace S2VX.Game.Story.Note {
    public partial class SliderPath {
        private class SliderPathDrawNode : DrawNode {
            protected new SliderPath Source => (SliderPath)base.Source;

            private List<Line> Segments { get; } = new();

            private Texture Texture;
            private Vector2 DrawSize;
            private float Radius;
            private IShader PathShader;

            private QuadBatch<TexturedVertex3D> QuadBatch { get; } = new(200, 10);

            public SliderPathDrawNode(Path source) : base(source) { }

            public override void ApplyState() {
                base.ApplyState();
                Segments.Clear();
                Segments.AddRange(Source.Segments());
                Texture = Source.Texture;
                DrawSize = Source.DrawSize;
                Radius = Source.PathRadius;
                PathShader = Source.PathShader;
            }

            private static Vector2 PointOnCircle(float angle) => new(MathF.Sin(angle), -MathF.Cos(angle));

            private Vector2 RelativePosition(Vector2 localPos) => Vector2.Divide(localPos, DrawSize);

            private Color4 ColorAt(Vector2 localPos) => DrawColourInfo.Colour.HasSingleColour
                ? ((SRGBColour)DrawColourInfo.Colour).Linear
                : DrawColourInfo.Colour.Interpolate(RelativePosition(localPos)).Linear;

            private void AddCap(Vector2 vertex, RectangleF texRect) {
                var unitX = Vector2.UnitX * Radius;
                var unitY = Vector2.UnitY * Radius;
                var topLeft = vertex - unitX - unitY;
                var topRight = vertex + unitX - unitY;
                //var left = vertex - unitX;
                //var right = vertex + unitX;
                var bottom = vertex - unitY;
                var top = vertex + unitY;
                var bottomLeft = vertex - unitX + unitY;
                var bottomRight = vertex + unitX + unitY;

                //AddLineQuads(new Line(topLeft, topRight), new Line(left, right), new Line(bottomLeft, bottomRight), texRect);
                AddLineQuads(new Line(bottomLeft, topLeft), new Line(bottom, top), new Line(bottomRight, topRight), texRect);
            }

            private void AddSegment(Line line, RectangleF texRect) {
                var ortho = line.OrthogonalDirection;
                var lineLeft = new Line(line.StartPoint + ortho * Radius, line.EndPoint + ortho * Radius);
                var lineRight = new Line(line.StartPoint - ortho * Radius, line.EndPoint - ortho * Radius);
                AddLineQuads(lineLeft, line, lineRight, texRect);
            }

            private void AddLineQuads(Line left, Line middle, Line right, RectangleF texRect) {
                var screenLineLeft = new Line(
                    Vector2Extensions.Transform(left.StartPoint, DrawInfo.Matrix),
                    Vector2Extensions.Transform(left.EndPoint, DrawInfo.Matrix)
                );
                var screenLineRight = new Line(
                    Vector2Extensions.Transform(right.StartPoint, DrawInfo.Matrix),
                    Vector2Extensions.Transform(right.EndPoint, DrawInfo.Matrix)
                );
                var screenLine = new Line(
                    Vector2Extensions.Transform(middle.StartPoint, DrawInfo.Matrix),
                    Vector2Extensions.Transform(middle.EndPoint, DrawInfo.Matrix)
                );
                QuadBatch.Add(new() {
                    Position = new(screenLineRight.EndPoint.X, screenLineRight.EndPoint.Y, 0),
                    TexturePosition = new(texRect.Left, texRect.Centre.Y),
                    Colour = ColorAt(right.EndPoint)
                });
                QuadBatch.Add(new() {
                    Position = new(screenLineRight.StartPoint.X, screenLineRight.StartPoint.Y, 0),
                    TexturePosition = new(texRect.Left, texRect.Centre.Y),
                    Colour = ColorAt(right.StartPoint)
                });
                // Each "quad" of the slider is actually rendered as 2 quads, being split in half along the approximating line.
                // On this line the depth is 1 instead of 0, which is done properly handle self-overlap using the depth buffer.
                // Thus the middle vertices need to be added twice (once for each quad).
                var firstMiddlePoint = new Vector3(screenLine.StartPoint.X, screenLine.StartPoint.Y, 1);
                var secondMiddlePoint = new Vector3(screenLine.EndPoint.X, screenLine.EndPoint.Y, 1);
                var firstMiddleColour = ColorAt(middle.StartPoint);
                var secondMiddleColour = ColorAt(middle.EndPoint);
                for (var i = 0; i < 2; ++i) {
                    QuadBatch.Add(new() {
                        Position = firstMiddlePoint,
                        TexturePosition = new(texRect.Right, texRect.Centre.Y),
                        Colour = firstMiddleColour
                    });
                    QuadBatch.Add(new() {
                        Position = secondMiddlePoint,
                        TexturePosition = new(texRect.Right, texRect.Centre.Y),
                        Colour = secondMiddleColour
                    });
                }
                QuadBatch.Add(new() {
                    Position = new(screenLineLeft.EndPoint.X, screenLineLeft.EndPoint.Y, 0),
                    TexturePosition = new(texRect.Left, texRect.Centre.Y),
                    Colour = ColorAt(left.EndPoint)
                });
                QuadBatch.Add(new() {
                    Position = new(screenLineLeft.StartPoint.X, screenLineLeft.StartPoint.Y, 0),
                    TexturePosition = new(texRect.Left, texRect.Centre.Y),
                    Colour = ColorAt(left.StartPoint)
                });
            }


            private void UpdateVertexBuffer() {
                //var line = Segments[0];
                //var theta = line.Theta;
                // Offset by 0.5 pixels inwards to ensure we never sample texels outside the bounds
                var texRect = Texture.GetTextureRect(new RectangleF(0.5f, 0.5f, Texture.Width - 1, Texture.Height - 1));
                //AddLineCap(line.StartPoint, theta + MathF.PI, MathF.PI, texRect);
                //for (var i = 1; i < Segments.Count; ++i) {
                //    var nextLine = Segments[i];
                //    var nextTheta = nextLine.Theta;
                //    //AddLineCap(line.EndPoint, theta, nextTheta - theta, texRect);

                //    line = nextLine;
                //    theta = nextTheta;
                //}
                //AddLineCap(line.EndPoint, theta, MathF.PI, texRect);

                var offset = Source.VertexBounds().TopLeft;
                for (var i = 0; i < Source.Vertices.Count - 1; ++i) {
                    AddCap(Source.Vertices[i] - offset, texRect);
                }

                //foreach (var segment in Segments) {
                //    AddLineQuads(segment, texRect);
                //}
            }

            public override void Draw(Action<TexturedVertex2D> vertexAction) {
                base.Draw(vertexAction);
                if (Texture?.Available != true || Segments.Count == 0) {
                    return;
                }
                GLWrapper.PushDepthInfo(DepthInfo.Default);
                // Blending is removed to allow for correct blending between the wedges of the path.
                GLWrapper.SetBlend(BlendingParameters.None);
                PathShader.Bind();
                Texture.TextureGL.Bind();
                UpdateVertexBuffer();
                PathShader.Unbind();
                GLWrapper.PopDepthInfo();
            }

            protected override void Dispose(bool isDisposing) {
                base.Dispose(isDisposing);
                QuadBatch.Dispose();
            }
        }
    }
}
