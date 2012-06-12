using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperCaissiere.Engine.Core;

namespace SuperCaissiere.Engine.World
{
    /// <summary>
    /// Basic moving world object
    /// </summary>
    public abstract class PhysicsEntity : Entity
    {

        /// <summary>
        /// 
        /// </summary>
        protected float mass;

        /// <summary>
        /// Moving speed
        /// </summary>
        protected Vector2 speed;

        /// <summary>
        /// Current velocity
        /// </summary>
        protected Vector2 velocity = new Vector2(0, -1);

        public bool IsOnGround { get; set; }

        public bool IsStuckLeft { get; set; }
        public bool IsStuckRight { get; set; }
        public bool IsStuckTop { get; set; }

        /// <summary>
        /// Platform where the entity is on
        /// </summary>
        protected Entity platform;

        /// <summary>
        /// New sprite
        /// </summary>
        /// <param name="assetName">Sprite to use</param>
        /// <param name="location">Screen location</param>
        /// <param name="spriteRect">Spritesheet source rectangle</param>
        /// <param name="speed">Moving speed</param>
        /// <param name="scale">Scale</param>
        public PhysicsEntity(String assetName, Vector2 location, Rectangle spriteRect, Vector2 speed, Vector2 scale)
            : base(assetName, location, spriteRect, scale)
        {
            this.speed = speed;

            // Register the entity as a physic one
            Application.PhysicsManager.RegisterEntity(this);
            OnDeath += new Action(PhysicsEntity_OnDeath);
        }

        void PhysicsEntity_OnDeath()
        {
            if (IsRemovable)
            {
                Application.PhysicsManager.UnregisterEntity(this);
            }
        }

        /// <summary>
        /// Raised when an entity is in collision with the Awareness box
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="depth"></param>
        public abstract void EntityDetected(Entity collider, Vector2 depth);

        /// <summary>
        /// Raised when an entity is in collision with the Collision box
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="depth"></param>
        public abstract void CollisionDetected(Entity collider, Vector2 depth);

        /// <summary>
        /// Optionnal floor collision event
        /// </summary>
        public Action<Vector2> FloorCollisionDetected { get; protected set; }

        public float Mass
        {
            get { return mass; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        /// If the entity is on a moving floor, it has to move with it
        /// </summary>
        public Vector2 MovingFloorMovement
        {
            get;
            set;
        }

        public Vector2 Speed
        {
            get { return speed; }
        }
    }
}
