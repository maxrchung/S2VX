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
using osuTK.Graphics.ES30;
using System;
using System.Collections.Generic;

namespace S2VX.Game.Story.Note {
    public partial class SliderPath {
        private class SliderPathDrawNode : DrawNode {
            public const int MaxRes = 24;

            protected new SliderPath Source => (SliderPath)base.Source;

            private readonly List<Line> Segments = new();

            private Texture Texture;
            private Vector2 DrawSize;
            private float Radius;
            private IShader PathShader;

            // We multiply the size param by 3 such that the amount of vertices is a multiple of the amount of vertices
            // per primitive (triangles in this case). Otherwise overflowing the batch will result in wrong
            // grouping of vertices into primitives.
            private readonly LinearBatch<TexturedVertex3D> HalfCircleBatch = new(MaxRes * 100 * 3, 10, PrimitiveType.Triangles);
            private readonly QuadBatch<TexturedVertex3D> QuadBatch = new(200, 10);

            public SliderPathDrawNode(Path source)
                : base(source) {
            }

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

            private void AddLineCap(Vector2 origin, float theta, float thetaDiff, RectangleF texRect) {
                const float step = MathF.PI / MaxRes;
                float dir = Math.Sign(thetaDiff);
                thetaDiff = dir * thetaDiff;
                var amountPoints = (int)Math.Ceiling(thetaDiff / step);
                if (dir < 0) {
                    theta += MathF.PI;
                }
                var current = origin + PointOnCircle(theta) * Radius;
                var currentColour = ColorAt(current);
                current = Vector2Extensions.Transform(current, DrawInfo.Matrix);
                var screenOrigin = Vector2Extensions.Transform(origin, DrawInfo.Matrix);
                var originColour = ColorAt(origin);
                for (var i = 1; i <= amountPoints; i++) {
                    // Center point
                    HalfCircleBatch.Add(new() {
                        Position = new(screenOrigin.X, screenOrigin.Y, 1),
                        TexturePosition = new(texRect.Right, texRect.Centre.Y),
                        Colour = originColour
                    });
                    // First outer point
                    HalfCircleBatch.Add(new() {
                        Position = new(current.X, current.Y, 0),
                        TexturePosition = new(texRect.Left, texRect.Centre.Y),
                        Colour = currentColour
                    });
                    var angularOffset = Math.Min(i * step, thetaDiff);
                    current = origin + PointOnCircle(theta + dir * angularOffset) * Radius;
                    currentColour = ColorAt(current);
                    current = Vector2Extensions.Transform(current, DrawInfo.Matrix);
                    // Second outer point
                    HalfCircleBatch.Add(new() {
                        Position = new(current.X, current.Y, 0),
                        TexturePosition = new(texRect.Left, texRect.Centre.Y),
                        Colour = currentColour
                    });
                }
            }

            private void AddLineQuads(Line line, RectangleF texRect) {
                var ortho = line.OrthogonalDirection;
                var lineLeft = new Line(line.StartPoint + ortho * Radius, line.EndPoint + ortho * Radius);
                var lineRight = new Line(line.StartPoint - ortho * Radius, line.EndPoint - ortho * Radius);
                var screenLineLeft = new Line(
                    Vector2Extensions.Transform(lineLeft.StartPoint, DrawInfo.Matrix),
                    Vector2Extensions.Transform(lineLeft.EndPoint, DrawInfo.Matrix)
                );
                var screenLineRight = new Line(
                    Vector2Extensions.Transform(lineRight.StartPoint, DrawInfo.Matrix),
                    Vector2Extensions.Transform(lineRight.EndPoint, DrawInfo.Matrix)
                );
                var screenLine = new Line(
                    Vector2Extensions.Transform(line.StartPoint, DrawInfo.Matrix),
                    Vector2Extensions.Transform(line.EndPoint, DrawInfo.Matrix)
                );
                QuadBatch.Add(new() {
                    Position = new(screenLineRight.EndPoint.X, screenLineRight.EndPoint.Y, 0),
                    TexturePosition = new(texRect.Left, texRect.Centre.Y),
                    Colour = ColorAt(lineRight.EndPoint)
                });
                QuadBatch.Add(new() {
                    Position = new(screenLineRight.StartPoint.X, screenLineRight.StartPoint.Y, 0),
                    TexturePosition = new(texRect.Left, texRect.Centre.Y),
                    Colour = ColorAt(lineRight.StartPoint)
                });
                // Each "quad" of the slider is actually rendered as 2 quads, being split in half along the approximating line.
                // On this line the depth is 1 instead of 0, which is done properly handle self-overlap using the depth buffer.
                // Thus the middle vertices need to be added twice (once for each quad).
                var firstMiddlePoint = new Vector3(screenLine.StartPoint.X, screenLine.StartPoint.Y, 1);
                var secondMiddlePoint = new Vector3(screenLine.EndPoint.X, screenLine.EndPoint.Y, 1);
                var firstMiddleColour = ColorAt(line.StartPoint);
                var secondMiddleColour = ColorAt(line.EndPoint);
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
                    Colour = ColorAt(lineLeft.EndPoint)
                });
                QuadBatch.Add(new() {
                    Position = new(screenLineLeft.StartPoint.X, screenLineLeft.StartPoint.Y, 0),
                    TexturePosition = new(texRect.Left, texRect.Centre.Y),
                    Colour = ColorAt(lineLeft.StartPoint)
                });
            }

            private void UpdateVertexBuffer() {
                var line = Segments[0];
                var theta = line.Theta;
                // Offset by 0.5 pixels inwards to ensure we never sample texels outside the bounds
                var texRect = Texture.GetTextureRect(new RectangleF(0.5f, 0.5f, Texture.Width - 1, Texture.Height - 1));
                AddLineCap(line.StartPoint, theta + MathF.PI, MathF.PI, texRect);
                for (var i = 1; i < Segments.Count; ++i) {
                    var nextLine = Segments[i];
                    var nextTheta = nextLine.Theta;
                    AddLineCap(line.EndPoint, theta, nextTheta - theta, texRect);

                    line = nextLine;
                    theta = nextTheta;
                }
                AddLineCap(line.EndPoint, theta, MathF.PI, texRect);
                foreach (var segment in Segments) {
                    AddLineQuads(segment, texRect);
                }
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
                HalfCircleBatch.Dispose();
                QuadBatch.Dispose();
            }
        }
    }
}
