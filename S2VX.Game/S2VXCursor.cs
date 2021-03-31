using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game {
    public class S2VXCursor : CursorContainer {
        // Convenience function for outside classes to use so they don't need to
        // remember to pass in ActiveCursor
        public void Reset() => ResetInternal(ActiveCursor);

        public void UpdateRotation(float rotation) =>
            ActiveCursor.Rotation = rotation;

        public void UpdateSize(Vector2 cameraScale) =>
            ActiveCursor.Size = cameraScale * S2VXGameBase.GameWidth / 4;

        public void UpdateColor(Color4 color) =>
            ActiveCursor.Colour = color;

        protected override Drawable CreateCursor() {
            var box = new Box();
            ResetInternal(box);
            return box;
        }

        private static void ResetInternal(Drawable cursor) {
            cursor.Size = new Vector2(20);
            cursor.Colour = S2VXColorConstants.LightYellow;
            cursor.Rotation = 0;
            cursor.Origin = Anchor.Centre;
        }
    }
}
