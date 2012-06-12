using Microsoft.Xna.Framework;

namespace SuperCaissiere.Engine.Graphics
{
    /// <summary>
    /// Animation features (change frame with delay)
    /// </summary>
    public class SpriteAnimation
    {
        /// <summary>
        /// Repeat animation
        /// </summary>
        public static int InfiniteAnimation = -1;

        private Rectangle animatedSRect;

        /// <summary>
        /// First keyframe x position
        /// </summary>
        private int initX;

        /// <summary>
        /// Current keyframe index 
        /// </summary>
        private int currentFrame;

        /// <summary>
        /// Total keyframes number 
        /// </summary>
        private int totalFrameNumber;

        /// <summary>
        /// Timer
        /// </summary>
        private double frameTime;

        /// <summary>
        /// Time left before next keyframe
        /// </summary>
        private double frameCooldown;

        /// <summary>
        /// Sprite window  
        /// </summary>
        private Vector2 spriteBox;

        /// <summary>
        /// Loop count left. Use -1 for infinite
        /// </summary>
        private int currentpass;
        private int _pass; 

        /// <summary>
        /// Animation is over
        /// </summary>
        public bool Over { get; set; }

        /// <summary>
        /// Is the animation is over, stay on the last keyframe
        /// </summary>
        public bool KeepLastFrame { get; set; }

        /// <summary>
        /// New animation engine
        /// </summary>
        /// <param name="sRect"></param>
        /// <param name="frameNumber"></param>
        /// <param name="frameSpeed"></param>
        /// <param name="firstFrameX"></param>
        /// <param name="boxSize"></param>
        /// <param name="loopNumber"></param>
        public SpriteAnimation(Rectangle sRect, int frameNumber, float frameSpeed, int firstFrameX, Vector2 boxSize, int loopNumber)
        {
            this.currentFrame = 0;
            this.frameTime = 0;
            this.totalFrameNumber = frameNumber;
            this.frameCooldown = frameSpeed;
            this.initX = firstFrameX;
            this.currentpass = _pass =loopNumber;


            this.animatedSRect = sRect;

            //Sprites should be well displayed
            this.spriteBox = boxSize;

            this.KeepLastFrame = false;
        }

        /// <summary>
        /// New animation
        /// </summary>
        /// <remarks></remarks>
        public SpriteAnimation(Rectangle sRect, int frameNumber, float frameSpeed, int loopNumber)
            : this(sRect, frameNumber, frameSpeed, sRect.X, new Vector2(sRect.Width, sRect.Height), loopNumber)
        {
        }

        /// <summary>
        /// New animation
        /// </summary>
        /// <remarks></remarks>
        public SpriteAnimation(Rectangle sRect, int frameNumber, float frameSpeed)
            : this(sRect, frameNumber, frameSpeed, sRect.X, new Vector2(sRect.Width, sRect.Height), InfiniteAnimation)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //Update current frame
            if ((frameCooldown > 0) && (Over == false))
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - frameTime > frameCooldown)
                {
                    CurrentFrame++;

                    if (CurrentFrame >= totalFrameNumber)
                    {
                        if (currentpass != InfiniteAnimation)
                        {
                            currentpass--;

                            if (currentpass <= 0) Over = true;
                        }

                        if (KeepLastFrame)
                        {
                            CurrentFrame = totalFrameNumber;
                        }
                        else
                        {
                            CurrentFrame = 0;
                        }
                    }

                    frameTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            animatedSRect.X = initX + ((int)spriteBox.X * CurrentFrame);
        }

        /// <summary>
        /// Restart animation
        /// </summary>
        public void Reset()
        {
            CurrentFrame = 0;
            Over = false;
            currentpass = _pass;
        }

        /// <summary>
        /// Loop count left
        /// </summary>
        public int Pass
        {
            get
            {
                return currentpass;
            }
            set
            {
                currentpass = value;
                if (currentpass > 0) Over = false;
            }
        }

        /// <summary>
        /// sRect to use to have animation in a sprite
        /// </summary>
        public Rectangle AnimatedRect
        {
            get
            {
                return animatedSRect;
            }
        }

        /// <summary>
        /// Time between two keyframes
        /// </summary>
        public double FrameCooldown
        {
            get
            {
                return this.frameCooldown;
            }
            set
            {
                this.frameCooldown = value;
            }
        }

        /// <summary>
        /// Current keyframe
        /// </summary>
        public int CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }
    }
}
