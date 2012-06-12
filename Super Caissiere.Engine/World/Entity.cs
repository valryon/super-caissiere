using System;
using SuperCaissiere.Engine.Core;
using SuperCaissiere.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperCaissiere.Engine.Physics;
using System.Collections.Generic;

namespace SuperCaissiere.Engine.World
{
    /// <summary>
    /// Basic world object
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Sprite location on screen
        /// </summary>
        protected Vector2 location;

        /// <summary>
        /// Orientation
        /// </summary>
        protected double rotation;

        /// <summary>
        /// Source and display rect
        /// </summary>
        protected Rectangle sRect, dRect;

        /// <summary>
        /// Flip
        /// </summary>
        protected SpriteEffects flip;

        /// <summary>
        /// Scale
        /// </summary>
        protected Vector2 scale;

        /// <summary>
        /// Asset name
        /// </summary>
        protected String assetName;

        /// <summary>
        /// Initialized
        /// </summary>
        protected bool initialized;

        /// <summary>
        /// XNA rotation center
        /// </summary>
        protected Vector2 spriteOrigin;

        /// <summary>
        /// Animation object
        /// </summary>
        protected SpriteAnimation animation;

        protected Hitbox hitbox;
        protected Hitbox awarenessbox;

        /// <summary>
        /// Sprite size
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// Sprite color
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Entity define some floor collisions
        /// </summary>
        public Floor Floor { get; protected set; }

        /// <summary>
        /// Drawing order (default 100)
        /// </summary>
        public int LayerDepth { get; set; }

        /// <summary>
        /// Entities can be invincible for a while
        /// </summary>
        protected float invincibleTime;

        /// <summary>
        /// New sprite
        /// </summary>
        /// <param name="assetName">TODO</param>
        /// <param name="location">Screen location</param>
        /// <param name="spriteRect">Spritesheet source rectangle</param>
        /// <param name="speed">Moving speed</param>
        /// <param name="scale">Scale</param>
        public Entity(String assetName, Vector2 location, Rectangle spriteRect, Vector2 scale)
        {
            this.assetName = assetName;

            this.location = location;
            this.sRect = spriteRect;
            this.scale = scale;

            this.ComputeDstRect();

            //Default
            this.spriteOrigin = Vector2.Zero;
            this.flip = SpriteEffects.None;
            this.animation = null; //No animation

            this.IsRemovable = true;
            this.Size = new Vector2(spriteRect.Width, spriteRect.Height);

            this.hitbox = null;
            this.awarenessbox = null;

            this.Color = Color.White;

            IsAlive = true;

            Floor = null;
            LayerDepth = 100;
        }

        /// <summary>
        /// Sprite init.
        /// Allow to easily define a location after the sprite creation.
        /// </summary>
        /// <param name="location">New sprite location</param>
        public virtual void Initialize(Vector2 location)
        {
            this.location = location;
            this.dRect.X = (int)this.location.X;
            this.dRect.Y = (int)this.location.Y;

            this.initialized = true;
        }

        /// <summary>
        /// Update sprite
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            //Update display rect
            this.dRect.X = (int)this.location.X;
            this.dRect.Y = (int)this.location.Y;

            // Update hitbox
            if (hitbox != null)
            {
                hitbox.Update(gameTime, location);
            }
            if (awarenessbox != null)
            {
                awarenessbox.Update(gameTime, location);
            }

            // Update animation
            if (animation != null)
            {
                animation.Update(gameTime);
                sRect = animation.AnimatedRect;
            }

            if (invincibleTime >= 0)
            {
                invincibleTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        /// <summary>
        /// Display sprite on screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatchProxy spriteBatch)
        {
            if (IsOnScreen())
            {
                spriteBatch.Draw(Application.MagicContentManager.GetTexture(assetName), dRect, sRect, Color, (float)rotation, spriteOrigin, flip, 1.0f);

                if (Application.IsDebugMode)
                {
                    if (hitbox != null)
                    {
                        hitbox.Draw(spriteBatch);
                    }
                    if (awarenessbox != null)
                    {
                        awarenessbox.Draw(spriteBatch);
                    }
                }
            }
        }

        /// <summary>
        /// Set the sprite's origin to the middle of the sRect
        /// </summary>
        protected void SetSpriteOriginToMiddle()
        {
            this.spriteOrigin = new Vector2(sRect.Width / 2, sRect.Height / 2);
        }

        /// <summary>
        /// Compute display rect with scale
        /// </summary>
        protected virtual void ComputeDstRect()
        {
            Rectangle dst = new Rectangle();

            dst.X = (int)location.X;
            dst.Y = (int)location.Y;
            dst.Width = (int)((float)sRect.Width * scale.X);
            dst.Height = (int)((float)sRect.Height * scale.Y);
            this.dRect = dst;
        }

        /// <summary>
        /// Clone item 
        /// </summary>
        /// <returns></returns>
        public abstract Entity Clone();

        #region Events

        /// <summary>
        /// Triggered once when IsAlive is set to false
        /// </summary>
        public event Action OnDeath;

        #endregion

        #region Propriétés

        /// <summary>
        /// Collision box
        /// </summary>
        public Hitbox Hitbox
        {
            get
            {
                return hitbox;
            }
        }

        /// <summary>
        /// Awareness box
        /// </summary>
        public Hitbox AwarenessBox
        {
            get
            {
                return awarenessbox;
            }
        }

        /// <summary>
        /// Is the entity removable from the game
        /// </summary>
        public bool IsRemovable { get; set; }

        public bool IsInvincible
        {
            get
            {
                return invincibleTime > 0;
            }
        }

        /// <summary>
        /// Is the entity alive ? If not, we may remove it from the game
        /// </summary>
        private bool _isAlive;

        public bool IsAlive
        {
            get { return _isAlive; }
            set
            {
                if (_isAlive != value)
                {
                    _isAlive = value;
                    if (IsAlive == false)
                    {
                        if (OnDeath != null) OnDeath();
                    }
                }
            }
        }

        /// <summary>
        /// Is the entity initialized (and can be displayed)
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return initialized;
            }
        }

        /// <summary>
        /// Screen location
        /// </summary>
        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }

        /// <summary>
        /// Orientation
        /// </summary>
        public double Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        /// <summary>
        /// Spritesheet source rectangle
        /// </summary>
        public virtual Rectangle SrcRect
        {
            get { return sRect; }
            set
            {
                sRect = value;
                //Update dst
                ComputeDstRect();
            }
        }

        /// <summary>
        /// Display rectangle
        /// </summary>
        public virtual Rectangle DstRect
        {
            get { return dRect; }
            set { dRect = value; }
        }

        /// <summary>
        /// Flip
        /// </summary>
        public SpriteEffects Flip
        {
            get { return flip; }
            set { flip = value; }
        }

        /// <summary>
        /// Scale
        /// </summary>
        public Vector2 Scale
        {
            get { return this.scale; }
            set
            {
                this.scale = value;

                ComputeDstRect();
            }
        }

        /// <summary>
        /// Test if the sprite is on screen with camera parameters
        /// </summary>
        public bool IsOnScreen()
        {
            var camera = Application.GameStateManager.CurrentGameState.SceneCamera;
            return camera.VisibilityRectangle.Intersects(DstRect);

        }

        /// <summary>
        /// Test if the sprite is on or near the screen so it should be updatable (otherwise it's too far and maybe useless)
        /// </summary>
        public bool IsUpdatable()
        {
            var camera = Application.GameStateManager.CurrentGameState.SceneCamera;
            return camera.AwarenessRectangle.Intersects(DstRect) || camera.AwarenessRectangle.Contains(Location.ToPoint());

        }

        /// <summary>
        /// Sprite Animation
        /// </summary>
        public SpriteAnimation Animation
        {
            get
            {
                return this.animation;
            }
        }

        #endregion
    }
}
