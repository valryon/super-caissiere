using System;
using Microsoft.Xna.Framework;

namespace SuperCaissiere.Engine.Util
{
    public class RandomMachine
    {
        private Random _random;

        /// <summary>
        /// Random init with seed
        /// </summary>
        public RandomMachine(int seed)
        {
            _random = new Random(seed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fMin"></param>
        /// <param name="fmax"></param>
        /// <returns></returns>
        public float GetRandomFloat(double fMin, double fmax)
        {
            return (float)(_random.NextDouble() * (fmax - fMin) + fMin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public int GetRandomInt(int min, int max)
        {
            return _random.Next(min, max);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <returns></returns>
        public int GetRandomInt(int max)
        {
            return _random.Next(max);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMin"></param>
        /// <param name="yMax"></param>
        /// <returns></returns>
        public Vector2 GetRandomVector2(float xMin, float xMax, float yMin, float yMax)
        {
            return new Vector2(GetRandomFloat(xMin, xMax),
                GetRandomFloat(yMin, yMax));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public Vector2 GetRandomVector2(Vector2 center, double radius)
        {
            double randomTeta = (double)GetRandomFloat(0, 360);

            double x = Math.Cos(randomTeta) * radius;
            double y = Math.Sin(randomTeta) * radius;

            return new Vector2(center.X + (float)x, center.Y + (float)y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Vector2 GetRandomTrajectory(double radius,double anglemin=0, double anglemax = 360)
        {
            double randomTeta = (double)GetRandomFloat(anglemin, anglemax);

            double x = Math.Cos(randomTeta) * radius;
            double y = Math.Sin(randomTeta) * radius;

            return new Vector2((float)x, (float)y);
        }

    }
}
