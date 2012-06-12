using SuperCaissiere.Engine.Core;
using SuperCaissiere.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCaissiere.Engine.Particules
{
    /// <summary>
    /// 2D simple particule
    /// </summary>
    public abstract class Particule
    {
         /// <summary>
        /// State
        /// </summary>
        public bool IsAlive { get; set; }

        /// <summary>
        /// Draw in background
        /// </summary>
        public bool IsBackground { get; set; }

        /// <summary>
        /// Use additive blending
        /// </summary>
        public bool IsAdditive { get; set; }

        private Vector2 location;
        protected Vector2 trajectory;
        private float timeToLive;
        protected Rectangle src;
        protected float scale;
        protected Color color;
        protected float rotation;
        protected float alpha;
        protected Vector2 particuleOrigin;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="src"></param>
        /// <param name="ttl">milliseconds</param>
        /// <param name="scale"></param>
        /// <param name="color"></param>
        public Particule(Vector2 location, Vector2 trajectory, Rectangle src, float ttl, float scale, Color color, bool background)
        {
            this.location = location;
            this.trajectory = trajectory;
            this.src = src;
            this.timeToLive = ttl;
            this.scale = scale;
            this.color = color;
            this.rotation = 0f;
            this.alpha = 1f;
            this.IsBackground = background;

            particuleOrigin = new Vector2(src.Width / 2, src.Height / 2); //Milieu du sprite
        }

        public virtual void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Update ttl
            TimeToLive -= elapsedTime;
            IsAlive = TimeToLive > 0;

            //Trajectory
            Location += trajectory * elapsedTime;
        }

        public virtual void Draw(SpriteBatchProxy spriteBatch)
        {
            spriteBatch.Draw(Application.MagicContentManager.GetTexture("particules"), Location, src, color * alpha, rotation, particuleOrigin, scale, SpriteEffects.None, 1.0f);
        }

        /// <summary>
        /// Temps de vie restant
        /// </summary>
        public float TimeToLive
        {
            get { return timeToLive; }
            set { timeToLive = value; }
        }

        /// <summary>
        /// Position
        /// </summary>
        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }
    }
}
