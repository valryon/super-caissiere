using System;
using System.Collections.Generic;
using SuperCaissiere.Engine.Core;
using SuperCaissiere.Engine.Graphics;
using SuperCaissiere.Engine.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCaissiere.Engine.Physics
{
    /// <summary>
    /// Simple physics Engine for entities
    /// </summary>
    public class PhysicsManager : Manager
    {
        /// <summary>
        /// Friction
        /// </summary>
        protected Vector2 groundFriction = new Vector2(1500, 0);
        protected Vector2 airFriction = new Vector2(1000, 0);

        private List<PhysicsEntity> _registeredEntities;
        private List<Rectangle> _debug;
        private List<PhysicsEntity> _unregisteredEntities;

        public PhysicsManager()
        {
            _registeredEntities = new List<PhysicsEntity>();
            _unregisteredEntities = new List<PhysicsEntity>();
            _debug = new List<Rectangle>();

            Gravity = new Vector2(0, 9.81f);

            AdditionalFloor = new List<Floor>();
        }

        public void Initialize()
        {
            _registeredEntities.Clear();
            WorldEntities = new List<Entity>();
        }

        public void Update(GameTime gameTime)
        {
            // Make sure the physic system is activated
            if (IsEnable)
            {
                var step = (float)gameTime.ElapsedGameTime.TotalSeconds;

                foreach (PhysicsEntity ent in _registeredEntities)
                {
                    applyPhysics(ent, step);
                }

                handleEntitiesCollisions();

                handleFloorCollisions();

                foreach (PhysicsEntity ent in _registeredEntities)
                {
                    applyVelocity(ent, step);
                }

                // Clean
                foreach (PhysicsEntity ent in _unregisteredEntities)
                {
                    _registeredEntities.Remove(ent);
                }
                _unregisteredEntities.Clear();
            }
        }

        private void applyPhysics(PhysicsEntity entity, float elapsedTime)
        {
            //Update gravity if required
            if ((entity.Mass != 0f) && (entity.IsOnGround == false))
            {
                entity.Velocity += Gravity * entity.Mass;
            }

            Vector2 friction = entity.IsOnGround ? groundFriction : airFriction;

            // Apply air or ground friction
            if (entity.Velocity.X > 0)
            {
                entity.Flip = SpriteEffects.None;
                entity.Velocity -= new Vector2(friction.X * elapsedTime, 0);
                if (entity.Velocity.X < 0f) entity.Velocity = new Vector2(0f, entity.Velocity.Y);
            }

            if (entity.Velocity.X < 0)
            {
                entity.Flip = SpriteEffects.FlipHorizontally;
                entity.Velocity += new Vector2(friction.X * elapsedTime, 0);
                if (entity.Velocity.X > 0f) entity.Velocity = new Vector2(0f, entity.Velocity.Y);
            }

            if (entity.Velocity.Y > 0)
            {
                entity.Velocity -= new Vector2(0f, friction.Y * elapsedTime);
                if (entity.Velocity.Y < 0f) entity.Velocity = new Vector2(entity.Velocity.X, 0f);
            }

            if (entity.Velocity.Y < 0)
            {
                entity.Velocity += new Vector2(0f, friction.Y * elapsedTime);
                if (entity.Velocity.Y > 0f) entity.Velocity = new Vector2(entity.Velocity.X, 0f);
            }

            // Keep velocity in [-speed, speed]
            if (entity.Speed.X != 0f)
            {
                entity.Velocity = new Vector2(MathHelper.Clamp(entity.Velocity.X, -entity.Speed.X, entity.Speed.X), entity.Velocity.Y);
            }

            if (entity.Speed.Y != 0f)
            {
                entity.Velocity = new Vector2(entity.Velocity.X, MathHelper.Clamp(entity.Velocity.Y, -entity.Speed.Y, entity.Speed.Y));
            }
        }

        /// <summary>
        /// Apply the velocity to make the entity move
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="step"></param>
        private void applyVelocity(PhysicsEntity entity, float elapsedTime)
        {
            //Update location
            if ((entity.Velocity.X != 0 || entity.Velocity.Y != 0) || (entity.MovingFloorMovement.X != 0 || entity.MovingFloorMovement.Y != 0))
            {
                var deltaMove = new Vector2(entity.Velocity.X * elapsedTime, entity.Velocity.Y * elapsedTime);

                entity.Location += deltaMove + entity.MovingFloorMovement;
            }
        }

        private void handleFloorCollisions()
        {
            _debug.Clear();

            // Make sure registered entities collide with the floor
            foreach (PhysicsEntity regEnt in _registeredEntities)
            {
                regEnt.IsOnGround = false;
                regEnt.IsStuckRight = false;
                regEnt.IsStuckLeft = false;
                regEnt.IsStuckTop = false;

                if ((regEnt.Hitbox != null) && regEnt.IsAlive)
                {
                    Rectangle entBox = regEnt.Hitbox.Dimensions;

                    // Find the floors around the entity
                    List<Floor> neighborhoodsTiles = new List<Floor>();

                    foreach (Rectangle rect in FloorCollisions.Keys)
                    {
                        if (rect.Intersects(entBox) || rect.Contains(entBox))
                        {
                            neighborhoodsTiles.AddRange(FloorCollisions[rect]);
                        }
                    }

                    handleEntityFloorCollisions(regEnt, neighborhoodsTiles);

                    // Add some extra floor rectangles, like platforms
                    regEnt.MovingFloorMovement = Vector2.Zero;

                    foreach (Floor m in AdditionalFloor)
                    {
                        if (handleEntityFloorCollisions(regEnt, new Floor[] { m }))
                        {
                            if (m.IsMoving)
                            {
                                // Collision with a moving floor: attach entity to the floor
                                regEnt.MovingFloorMovement = m.Movement;
                            }
                        }
                    }
                }
            } // foreach entity
        }

        private bool handleEntityFloorCollisions(PhysicsEntity regEnt, IEnumerable<Floor> floors)
        {
            bool collisionDetected = false;

            foreach (Floor floor in floors)
            {
                Vector2 collision;

                // Do not cross the floor
                if (Hitbox.Collide(regEnt.Hitbox, floor.Rectangle, out collision))
                {
                    float absDepthX = Math.Abs(collision.X);
                    float absDepthY = Math.Abs(collision.Y);

                    // Resolve the collision along the shallow axis.
                    if (absDepthY <= absDepthX)
                    {
                        bool floorOnTop = floor.Rectangle.Y < regEnt.Hitbox.Dimensions.Y;

                        // If we crossed the top of a tile, we are on the ground.
                        if (floorOnTop == false)
                        {
                            // Hack for passable platforms
                            if (floor.IsPassable == false)
                            {
                                regEnt.IsOnGround = true;
                            }
                            else if (regEnt.Velocity.Y >= 0)
                            {
                                regEnt.IsOnGround = true;
                            }
                        }

                        if (floor.IsPassable == false || regEnt.IsOnGround)
                        {
                            regEnt.Location = new Vector2(regEnt.Location.X, regEnt.Location.Y + collision.Y);

                            if (absDepthY != 0)
                            {
                                regEnt.Velocity = new Vector2(regEnt.Velocity.X, 0f);
                            }
                        }
                    }
                    else
                    {
                        regEnt.Location = new Vector2(regEnt.Location.X + collision.X, regEnt.Location.Y);

                        bool passable = (floor.IsPassable == true);

                        if (!passable)
                        {
                            // If going in the direction of the collision
                            if (floor.Rectangle.X > regEnt.Hitbox.Dimensions.X)
                            {
                                regEnt.IsStuckRight = true;
                            }
                            else if (floor.Rectangle.X < regEnt.Hitbox.Dimensions.X)
                            {
                                regEnt.IsStuckLeft = true;
                            }

                            if (absDepthX != 0)
                            {
                                regEnt.Velocity = new Vector2(0f, regEnt.Velocity.Y);
                            }
                        }
                    }

                    regEnt.Hitbox.UpdateBounds();

                    if (regEnt.FloorCollisionDetected != null)
                    {
                        regEnt.FloorCollisionDetected(collision);
                    }

                    if (regEnt.IsAlive == false) break;
                    collisionDetected = true;
                }
            }

            return collisionDetected;
        }

        private void handleEntitiesCollisions()
        {
            // Find collisions between entities
            foreach (Entity worldEnt in WorldEntities)
            {
                foreach (PhysicsEntity regEnt in _registeredEntities)
                {
                    if (worldEnt == regEnt) continue;

                    // Look for collisions
                    if ((regEnt.Hitbox != null) && (worldEnt.Hitbox != null))
                    {
                        Vector2 collision = Hitbox.Collide(regEnt.Hitbox, worldEnt.Hitbox);

                        // Raise event
                        if (collision != Vector2.Zero)
                        {
                            if (regEnt.IsInvincible == false)
                            {
                                regEnt.CollisionDetected(worldEnt, collision);
                            }
                        }
                    }

                    // Look for collisions
                    if ((regEnt.AwarenessBox != null) && (worldEnt.Hitbox != null) && worldEnt.IsAlive)
                    {
                        Vector2 collision = Hitbox.Collide(regEnt.AwarenessBox, worldEnt.Hitbox);

                        // Raise event
                        if (collision != Vector2.Zero)
                        {
                            regEnt.EntityDetected(worldEnt, collision);
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatchProxy spriteBatch)
        {
            foreach (Rectangle r in _debug)
            {
                spriteBatch.DrawRectangle(r, Color.Green * 0.75f);
            }
        }

        /// <summary>
        /// Add an entity to the list so it will be checked for collisions detection
        /// </summary>
        /// <param name="ent"></param>
        public void RegisterEntity(PhysicsEntity ent)
        {
            _registeredEntities.Add(ent);
        }

        /// <summary>
        /// Remove an entity from the collision detection
        /// </summary>
        /// <param name="ent"></param>
        public void UnregisterEntity(PhysicsEntity ent)
        {
            _unregisteredEntities.Add(ent);
        }

        /// <summary>
        /// Physics can be disabled
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// Current gravity
        /// </summary>
        public Vector2 Gravity { get; set; }

        /// <summary>
        /// Entities known in the world
        /// </summary>
        public List<Entity> WorldEntities { get; set; }

        /// <summary>
        /// Collisions with the world
        /// </summary>
        public Dictionary<Rectangle, List<Floor>> FloorCollisions { get; set; }

        /// <summary>
        /// Extra floor definition, out of the grid
        /// </summary>
        public List<Floor> AdditionalFloor { get; set; }
    }
}
