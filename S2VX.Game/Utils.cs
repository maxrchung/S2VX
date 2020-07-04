using System;
using System.Collections.Generic;
using System.Text;
using Markdig.Extensions.Yaml;
using osu.Framework.Utils;
using osuTK;

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
    }
}
