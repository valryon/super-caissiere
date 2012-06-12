using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SuperCaissiere.Engine.Input
{
    /// <summary>
    /// Gamepad vibrations =)
    /// </summary>
    public class Rumble
    {
        private int gamepadIndex;
        private Vector2 rumbleValue;

        public Rumble(int idx)
        {
            gamepadIndex = idx;
        }

        /// <summary>
        /// Decrease and set vibrations
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (gamepadIndex == -1) return;

            if (rumbleValue.X > 0f)
            {
                rumbleValue.X -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (rumbleValue.X < 0f) rumbleValue.X = 0f;
            }

            if (rumbleValue.Y > 0f)
            {
                rumbleValue.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (rumbleValue.Y < 0f) rumbleValue.Y = 0f;
            }

            GamePad.SetVibration((PlayerIndex)gamepadIndex, rumbleValue.X, rumbleValue.Y);
        }

        public float Left
        {
            get
            {
                return rumbleValue.X;
            }
            set
            {
                rumbleValue.X = value;
            }
        }

        public float Right
        {
            get
            {
                return rumbleValue.Y;
            }
            set
            {
                rumbleValue.Y = value;
            }
        }

        public int GamePadIndex
        {
            get { return gamepadIndex; }
            set { gamepadIndex = value; }
        }
    }
}
