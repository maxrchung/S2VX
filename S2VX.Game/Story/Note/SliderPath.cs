using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Lines;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osuTK.Graphics;
using osuTK.Graphics.ES30;
using System;
using System.Collections.Generic;

namespace S2VX.Game.Story.Note {
    public partial class SliderPath : SmoothPath {
        private Color4 OutlineColorProperty = Color4.White;
        public Color4 OutlineColor {
            get => OutlineColorProperty;
            set {
                if (OutlineColor == value) {
                    return;
                }
                OutlineColorProperty = value;
                InvalidateTexture();
            }
        }

        private IShader PathShader { get; set; }

        [BackgroundDependencyLoader]
        private void Load(ShaderManager shaders) =>
            PathShader = shaders.Load(VertexShaderDescriptor.TEXTURE_3, FragmentShaderDescriptor.TEXTURE);

        private float OutlineThicknessProperty = 0.1f;
        public float OutlineThickness {
            get => OutlineThicknessProperty;
            set {
                if (OutlineThicknessProperty == value) {
                    return;
                }
                OutlineThicknessProperty = value;
                InvalidateTexture();
            }
        }

        protected override Color4 ColourAt(float position) =>
            position <= OutlineThickness
                ? OutlineColor
                : Color4.Transparent;

        private readonly BufferedDrawNodeSharedData SharedData = new(new[] { RenderbufferInternalFormat.DepthComponent16 });
        protected override DrawNode CreateDrawNode() => new BufferedDrawNode(this, new SliderPathDrawNode(this), SharedData);

        private List<Line> Segments() {
            var segments = new List<Line>();
            if (Vertices.Count > 1) {
                var offset = VertexBounds().TopLeft;
                for (var i = 0; i < Vertices.Count - 1; ++i) {
                    segments.Add(new Line(Vertices[i] - offset, Vertices[i + 1] - offset));
                }
            }
            return segments;
        }

        private RectangleF VertexBounds() {
            if (Vertices.Count > 0) {
                float minX = 0;
                float minY = 0;
                float maxX = 0;
                float maxY = 0;

                for (int i = 0; i < Vertices.Count; ++i) {
                    var v = Vertices[i];
                    minX = Math.Min(minX, v.X - PathRadius);
                    minY = Math.Min(minY, v.Y - PathRadius);
                    maxX = Math.Max(maxX, v.X + PathRadius);
                    maxY = Math.Max(maxY, v.Y + PathRadius);
                }

                return new RectangleF(minX, minY, maxX - minX, maxY - minY);
            }
            return new RectangleF(0, 0, 0, 0);
        }
    }
}
