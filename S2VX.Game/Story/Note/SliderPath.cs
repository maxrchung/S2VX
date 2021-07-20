using osu.Framework.Graphics.Lines;
using osuTK.Graphics;

namespace S2VX.Game.Story.Note {
    public class SliderPath : SmoothPath {
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
    }
}
