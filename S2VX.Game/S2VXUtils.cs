﻿using osu.Framework.Utils;
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

        // Porting some random S2RYBRUH code for calculating signed angle between 2 vectors
        // https://github.com/maxrchung/S2RYBRUH/blob/master/S2RYBRUH/S2RYBRUH/Vector2.cpp#L45
        // http://stackoverflow.com/questions/11022446/direction-of-shortest-rotation-between-two-vectors
        public static double AngleBetween(Vector2 v1, Vector2 v2) {
            if (v1.LengthSquared == 0 || v2.LengthSquared == 0) {
                return 0;
            }

            var n1 = v1.Normalized();
            var n2 = v2.Normalized();

            if (n1 == n2) {
                return 0;
            }

            var dot = Vector2.Dot(n1, n2);
            Math.Clamp(dot, -1.0f, 1.0f);

            var rot = Math.Acos(dot);

            // Use cross vector3 to determine direction
            var cross = Vector3.Cross(new Vector3(n1.X, n1.Y, 0), new Vector3(n2.X, n2.Y, 0));
            return cross.Z > 0 ? -rot : rot;
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
