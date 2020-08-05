using System;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;

namespace S2VX.Game
{
    public class Utils
    {
        // https://en.wikipedia.org/wiki/Rotation_matrix
        public static Vector2 Rotate(Vector2 v, float degrees)
        {
            var x = v.X;
            var y = v.Y;
            var cos = (float)Math.Cos((double)MathUtils.DegreesToRadians(degrees));
            var sin = (float)Math.Sin((double)MathUtils.DegreesToRadians(degrees));
            var rotated = new Vector2(
                x * cos - y * sin,
                x * sin + y * cos
            );
            return rotated;
        }

        public static string Vector2ToString(Vector2 data)
        {
            return $"({data.X},{data.Y})";
        }
        public static string Color4ToString(Color4 data)
        {
            return $"({data.R},{data.G},{data.B})";
        }

        public static Vector2 Vector2FromString(string data)
        {
            var split = data.Replace("(", "").Replace(")", "").Split(',');
            return new Vector2(float.Parse(split[0]), float.Parse(split[1]));
        }

        public static Color4 Color4FromString(string data)
        {
            var split = data.Replace("(", "").Replace(")", "").Split(',');
            return new Color4(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]), 1);
        }
    }
}
