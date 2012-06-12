using System;
using SuperCaissiere.Engine.Graphics;
using SuperCaissiere.Engine.Particules;
using Microsoft.Xna.Framework;

namespace SuperCaissiere.Engine.Core
{
    /// <summary>
    /// Screens of the game : menu, ingame, game over, etc.
    /// </summary>
    public abstract class GameState
    {
        public GameState()
        {
            SceneCamera = new Camera();
            ParticuleManager = new ParticuleManager();
            IsActive = false;
        }

        /// <summary>
        /// Each scene get its own camera
        /// </summary>
        public Camera SceneCamera { get; set; }

        /// <summary>
        /// Particule Manager
        /// </summary>
        public ParticuleManager ParticuleManager { get; set; }

        /// <summary>
        /// Throw the loading result when it is complete
        /// </summary>
        public event Action<GameState, bool, Exception> LoadingComplete;

        /// <summary>
        /// Is the game state currently activated ?
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Initialize the game state. Automatically called before changing gamestate.
        /// </summary>
        public void Load()
        {
            try
            {
                ChangeCurrentGameState = false;
                NextGameState = null;

                LoadContent();

                InternalLoad();

                if (LoadingComplete != null)
                {
                    LoadingComplete(this, true, null);
                }
            }
            catch (Exception exc)
            {
                LoadingComplete(this, false, exc);
            }
        }

        /// <summary>
        /// Load content for this state
        /// </summary>
        protected abstract void LoadContent();

        /// <summary>
        /// Custom loading routine
        /// </summary>
        protected abstract void InternalLoad();

        /// <summary>
        /// Update the game state
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            SceneCamera.Update(gameTime);
            ParticuleManager.Update(gameTime);
        }

        /// <summary>
        /// Render the game state
        /// </summary>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(SpriteBatchProxy spriteBatch);

        /// <summary>
        /// If true, game state will be changed in the next frame
        /// </summary>
        public abstract bool ChangeCurrentGameState { get; protected set; }

        /// <summary>
        /// If ChangeCurrentGameState is true, the current game state will become this one
        /// </summary>
        public abstract GameState NextGameState { get; protected set; }

        public virtual bool UseLoading { get; set; }
    }
}
