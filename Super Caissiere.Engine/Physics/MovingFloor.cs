using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Lapins.Engine.Physics
{
    /// <summary>
    /// Get the floor behavior for a part of an entity
    /// </summary>
    public class MovingFloor
    {
        /// <summary>
        /// Define some extra rectangles with the smae behavior as floor
        /// </summary>
        public Rectangle[] ExtraCollisionsRect { get; set; }

        /// <summary>
        /// Get the current movement of this floor
        /// </summary>
        public Vector2 Movement { get; set; }
    }
}
