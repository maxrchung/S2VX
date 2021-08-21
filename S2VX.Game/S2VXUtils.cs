using osu.Framework.Graphics;
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

        // Porting some random S2RYBRUH code for calculating signed angle between 2 vectors
        // https://github.com/maxrchung/S2RYBRUH/blob/master/S2RYBRUH/S2RYBRUH/Vector2.cpp#L45
        // http://stackoverflow.com/questions/11022446/direction-of-shortest-rotation-between-two-vectors
        public static double AngleBetween(Vector2 v1, Vector2 v2) {
            if (v1.LengthSquared == 0 || v2.LengthSquared == 0) {
                return 0;
            }

            var n1 = v1.Normalized();
            var n2 = v2.Normalized();

            if (Precision.AlmostEquals(n1, n2)) {
                return 0;
            }

            var dot = Vector2.Dot(n1, n2);
            var clamped = Math.Clamp(dot, -1.0f, 1.0f);

            var rot = Math.Acos(clamped);

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

        public static string DoubleToString(double data, int precision = 0) {
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

        // Since .NET Core 3.0, values that are too large or small are rounded
        // to infinity. This can cause crashes within our code since arithmetic
        // can't be done with infinity. To address this, we're using our own
        // parse functions that throw an exception if parsing returns bad
        // values. See these remarks for more details:
        // https://docs.microsoft.com/en-us/dotnet/api/system.single.parse?view=net-5.0#remarks
        public static float StringToFloat(string data) {
            var value = float.Parse(data, CultureInfo.InvariantCulture);
            // Per these remarks, you should check for both IsNaN and IsInfinity:
            // https://docs.microsoft.com/en-us/dotnet/api/system.double.isnan?view=net-5.0#remarks
            return
                float.IsNaN(value) || float.IsInfinity(value)
                ? throw new ArgumentException("Failed to parse value.", nameof(data))
                : value;
        }

        public static double StringToDouble(string data) {
            var value = double.Parse(data, CultureInfo.InvariantCulture);
            return
                double.IsNaN(value) || double.IsInfinity(value)
                ? throw new ArgumentException("Failed to parse value.", nameof(data))
                : value;
        }

        public static Vector2 StringToVector2(string data) {
            var split = data.Replace("(", "", StringComparison.Ordinal).Replace(")", "", StringComparison.Ordinal).Split(',');
            return new Vector2(StringToFloat(split[0]), StringToFloat(split[1]));
        }

        public static Color4 StringToColor4(string data) {
            var split = data.Replace("(", "", StringComparison.Ordinal).Replace(")", "", StringComparison.Ordinal).Split(',');
            return new Color4(StringToFloat(split[0]), StringToFloat(split[1]), StringToFloat(split[2]), 1);
        }

        public static float ClampedInterpolation(double time, float val1, float val2, double startTime, double endTime, Easing easing = Easing.None) {
            if (time <= startTime || endTime - startTime == 0) {
                return val1;
            } else if (time >= endTime) {
                return val2;
            }
            return Interpolation.ValueAt(time, val1, val2, startTime, endTime, easing);
        }

        public static double ClampedInterpolation(double time, double val1, double val2, double startTime, double endTime, Easing easing = Easing.None) {
            if (time <= startTime || endTime - startTime == 0) {
                return val1;
            } else if (time >= endTime) {
                return val2;
            }
            return Interpolation.ValueAt(time, val1, val2, startTime, endTime, easing);
        }

        public static Vector2 ClampedInterpolation(double time, Vector2 val1, Vector2 val2, double startTime, double endTime, Easing easing = Easing.None) {
            if (time <= startTime || endTime - startTime == 0) {
                return val1;
            } else if (time >= endTime) {
                return val2;
            }
            return Interpolation.ValueAt(time, val1, val2, startTime, endTime, easing);
        }

        public static Color4 ClampedInterpolation(double time, Color4 val1, Color4 val2, double startTime, double endTime, Easing easing = Easing.None) {
            if (time <= startTime || endTime - startTime == 0) {
                return val1;
            } else if (time >= endTime) {
                return val2;
            }
            return Interpolation.ValueAt(time, val1, val2, startTime, endTime, easing);
        }
    }
}
