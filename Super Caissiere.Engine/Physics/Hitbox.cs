using System;
using Microsoft.Xna.Framework;
using SuperCaissiere.Engine.World;

namespace SuperCaissiere.Engine.Physics
{
    /// <summary>
    /// Simple rectangle hitbox
    /// </summary>
    public class Hitbox
    {
        /// <summary>
        /// Real hitbox item
        /// </summary>
        private Rectangle hitbox;
        private Vector2 _entityLocation;
        private Rectangle _dimensionRect;

        /// <summary>
        /// Collision box with specified dimension
        /// </summary>
        /// <param name="entityLocation"></param>
        /// <param name="dimensionRect"></param>
        public Hitbox(Rectangle dimensionRect)
        {
            //Init  hitbox
            _dimensionRect = dimensionRect;
            UpdateBounds();
        }

        public void UpdateBounds()
        {
            hitbox = _dimensionRect;
            hitbox.X += (int)_entityLocation.X;
            hitbox.Y += (int)_entityLocation.Y;
        }

        public void Update(GameTime gameTime, Vector2 entityLocation)
        {
            _entityLocation = entityLocation;

            UpdateBounds();
        }

        public void Draw(Graphics.SpriteBatchProxy spriteBatch)
        {
            spriteBatch.DrawRectangle(hitbox, Color.Red * 0.5f);
        }

        public Vector2 EntityLocation
        {
            get { return _entityLocation; }
        }

        public Rectangle Dimensions
        {
            get { return hitbox; }
        }

        /// <summary>
        /// Tile collisions
        /// </summary>
        /// <param name="hitboxA"></param>
        /// <param name="zoneB"></param>
        /// <returns></returns>
        public static bool Collide(Hitbox rectBox, Rectangle zoneB, out Vector2 depth)
        {
            depth = Vector2.Zero;

            return rectBox.Dimensions.GetIntersectionDepth(zoneB, out depth);
        }

        /// <summary>
        /// Collision with another rectangle hitbox (from Platformer 4.0)
        /// </summary>
        /// <param name="anotherHitbox"></param>
        /// <returns></returns>
        public static Vector2 Collide(Hitbox rectHitboxA, Hitbox rectHitboxB)
        {
            // Calculate half sizes.
            float halfWidthA = rectHitboxA.Dimensions.Width / 2.0f;
            float halfHeightA = rectHitboxA.Dimensions.Height / 2.0f;
            float halfWidthB = rectHitboxB.Dimensions.Width / 2.0f;
            float halfHeightB = rectHitboxB.Dimensions.Height / 2.0f;

            // Calculate centers.
            Vector2 centerA = new Vector2(rectHitboxA.EntityLocation.X + halfWidthA, rectHitboxA.EntityLocation.Y + halfHeightA);
            Vector2 centerB = new Vector2(rectHitboxB.EntityLocation.X + halfWidthB, rectHitboxB.EntityLocation.Y + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;

            Vector2 result = new Vector2(depthX, depthY);

            return result;
        }
    }
}