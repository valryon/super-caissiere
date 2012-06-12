using Microsoft.Xna.Framework;
using System;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Useful class for Vectors
    /// </summary>
    public static class Vector2Extension
    {
        /// <summary>
        /// Calculate the angle between 2 Vector2
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double GetAngle(this Vector2 v1, Vector2 v2)
        {
            return Math.Atan2((v2.Y - v1.Y) * -1, v2.X - v1.X);
        }

        public static Point ToPoint(this Vector2 v)
        {
            return new Point((int)v.X, (int)v.Y);
        }
    }
}
