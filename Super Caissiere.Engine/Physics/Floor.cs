using Microsoft.Xna.Framework;

namespace SuperCaissiere.Engine.Physics
{
    public class Floor
    {
        public Floor(Rectangle rect)
        {
            Rectangle = rect;
        }
        /// <summary>
        /// Rectangle
        /// </summary>
        public Rectangle Rectangle { get; set; }

        /// <summary>
        /// Floors you can land on but you cannot collide them from bottom
        /// </summary>
        public bool IsPassable { get; set; }

        /// <summary>
        /// Is the floor moving
        /// </summary>
        public bool IsMoving { get; set; }

        /// <summary>
        /// Get the current movement of this floor
        /// </summary>
        public Vector2 Movement { get; set; }

        /// <summary>
        /// Get the current speed of this floor
        /// </summary>
        public Vector2 Speed { get; set; }

    }
}
