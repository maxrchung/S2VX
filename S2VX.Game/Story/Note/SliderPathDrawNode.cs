// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Batches;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;
using osuTK.Graphics.ES30;
using System;
using System.Collections.Generic;

namespace S2VX.Game.Story.Note {
    public partial class SliderPath {
        private class SliderPathDrawNode : DrawNode {
            protected new SliderPath Source => (SliderPath)base.Source;

            private readonly List<Line> Segments = new();

            private Texture Texture;
            private Vector2 DrawSize;
            private float Radius;
            private IShader PathShader;

            private readonly LinearBatch<TexturedVertex3D> LinearBatch = new(2 * 100 * 3, 10, PrimitiveType.Triangles);
            private readonly QuadBatch<TexturedVertex3D> QuadBatch = new(200, 10);

            public SliderPathDrawNode(SliderPath source)
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

            private Color4 ColourAt(Vector2 localPos) => DrawColourInfo.Colour.HasSingleColour
                ? ((SRGBColour)DrawColourInfo.Colour).Linear
                : DrawColourInfo.Colour.Interpolate(RelativePosition(localPos)).Linear;

            private void AddLineCap(Vector2 origin, float theta, float thetaDiff, RectangleF texRect) {
                float dir = Math.Sign(thetaDiff);
                thetaDiff = dir * thetaDiff;
                var amountPoints = Precision.AlmostEquals(MathF.Abs(thetaDiff), MathF.PI) ? 2 : 1;
                if (dir < 0) {
                    theta += MathF.PI;
                }
                var closestCorner = FindClosestCorner(PointOnCircle(theta));
                var current = origin + closestCorner;
                var currentColour = ColourAt(current);
                current = Vector2Extensions.Transform(current, DrawInfo.Matrix);
                var screenOrigin = Vector2Extensions.Transform(origin, DrawInfo.Matrix);
                var originColour = ColourAt(origin);
                for (var i = 1; i <= amountPoints; i++) {
                    // Center point
                    LinearBatch.Add(new() {
                        Position = new(screenOrigin.X, screenOrigin.Y, 1),
                        TexturePosition = new(texRect.Right, texRect.Centre.Y),
                        Colour = originColour
                    });
                    // First outer point
                    LinearBatch.Add(new() {
                        Position = new(current.X, current.Y, 0),
                        TexturePosition = new(texRect.Left, texRect.Centre.Y),
                        Colour = currentColour
                    });
                    closestCorner = S2VXUtils.Rotate(closestCorner, 90);
                    current = origin + closestCorner;
                    currentColour = ColourAt(current);
                    current = Vector2Extensions.Transform(current, DrawInfo.Matrix);
                    // Second outer point
                    LinearBatch.Add(new() {
                        Position = new(current.X, current.Y, 0),
                        TexturePosition = new(texRect.Left, texRect.Centre.Y),
                        Colour = currentColour
                    });
                }
            }

            private void AddLineQuads(Line line, RectangleF texRect) {
                var closestCorner = FindClosestCorner(line.OrthogonalDirection);
                var oppositePoint = new Vector2(-closestCorner.X, -closestCorner.Y);
                var lineLeft = new Line(line.StartPoint + closestCorner, line.EndPoint + closestCorner);
                var lineRight = new Line(line.StartPoint + oppositePoint, line.EndPoint + oppositePoint);
                var screenLineLeft = new Line(Vector2Extensions.Transform(
                    lineLeft.StartPoint, DrawInfo.Matrix),
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
                    Colour = ColourAt(lineRight.EndPoint)
                });
                QuadBatch.Add(new() {
                    Position = new(screenLineRight.StartPoint.X, screenLineRight.StartPoint.Y, 0),
                    TexturePosition = new(texRect.Left, texRect.Centre.Y),
                    Colour = ColourAt(lineRight.StartPoint)
                });
                // Each "quad" of the slider is actually rendered as 2 quads, being split in half along the approximating line.
                // On this line the depth is 1 instead of 0, which is done properly handle self-overlap using the depth buffer.
                // Thus the middle vertices need to be added twice (once for each quad).
                var firstMiddlePoint = new Vector3(screenLine.StartPoint.X, screenLine.StartPoint.Y, 1);
                var secondMiddlePoint = new Vector3(screenLine.EndPoint.X, screenLine.EndPoint.Y, 1);
                var firstMiddleColour = ColourAt(line.StartPoint);
                var secondMiddleColour = ColourAt(line.EndPoint);
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
                    Colour = ColourAt(lineLeft.EndPoint)
                });
                QuadBatch.Add(new() {
                    Position = new(screenLineLeft.StartPoint.X, screenLineLeft.StartPoint.Y, 0),
                    TexturePosition = new(texRect.Left, texRect.Centre.Y),
                    Colour = ColourAt(lineLeft.StartPoint)
                });
            }

            private Vector2 FindClosestCorner(Vector2 direction) {
                var unitX = Vector2.UnitX * Radius;
                var unitY = Vector2.UnitY * Radius;
                var points = new List<Vector2> {
                    -unitX - unitY, // Top left
                    unitX - unitY, // Top right
                    -unitX + unitY, // Bottom left
                    unitX + unitY // Bottom right
                };
                var closestCorner = Vector2.Zero;
                var closestAngle = double.MaxValue;
                foreach (var point in points) {
                    var angle = Math.Abs(S2VXUtils.AngleBetween(direction, point));
                    if (angle < closestAngle) {
                        closestAngle = angle;
                        closestCorner = point;
                    }
                }
                return closestCorner;
            }

            private void UpdateVertexBuffer() {
                var line = Segments[0];
                var theta = line.Theta;
                // Offset by 0.5 pixels inwards to ensure we never sample texels outside the bounds
                var texRect = Texture.GetTextureRect(new(0.5f, 0.5f, Texture.Width - 1, Texture.Height - 1));
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
                LinearBatch.Dispose();
                QuadBatch.Dispose();
            }
        }
    }
}
