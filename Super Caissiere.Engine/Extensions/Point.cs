using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Extension methods for Point
    /// </summary>
    public static class PointExtension
    {
        /// <summary>
        /// Convert Point to Vector2
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}
