using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;
using System;
using System.Globalization;

namespace S2VX.Game {
    public static class S2VXUtils {
        // https://en.wikipedia.org/wiki/Rotation_matrix
        public static Vector2 Rotate(Vector2 v, float degrees) {
            var x = v.X;
            var y = v.Y;
            var cos = (float)Math.Cos(MathUtils.DegreesToRadians(degrees));
            var sin = (float)Math.Sin(MathUtils.DegreesToRadians(degrees));
            var rotated = new Vector2(
                x * cos - y * sin,
                x * sin + y * cos
            );
            return rotated;
        }

        public static string FloatToString(float data, int precision = 0) {
            if (precision == 0) {
                return $"{data}";
            } else {
                var formatString = "{0:0." + new string('#', precision) + "}";
                return string.Format(CultureInfo.InvariantCulture, formatString, data);
            }
        }

        public static string Vector2ToString(Vector2 data, int precision = 0) {
            if (precision == 0) {
                return $"({data.X},{data.Y})";
            } else {
                var x = FloatToString(data.X, precision);
                var y = FloatToString(data.Y, precision);
                return $"({x},{y})";
            }
        }

        public static string Color4ToString(Color4 data) => $"({data.R},{data.G},{data.B})";

        public static Vector2 Vector2FromString(string data) {
            var split = data.Replace("(", "", StringComparison.Ordinal).Replace(")", "", StringComparison.Ordinal).Split(',');
            return new Vector2(float.Parse(split[0], CultureInfo.InvariantCulture), float.Parse(split[1], CultureInfo.InvariantCulture));
        }

        public static Color4 Color4FromString(string data) {
            var split = data.Replace("(", "", StringComparison.Ordinal).Replace(")", "", StringComparison.Ordinal).Split(',');
            return new Color4(
                float.Parse(split[0], CultureInfo.InvariantCulture),
                float.Parse(split[1], CultureInfo.InvariantCulture),
                float.Parse(split[2], CultureInfo.InvariantCulture),
                1
            );
        }
    }
}
