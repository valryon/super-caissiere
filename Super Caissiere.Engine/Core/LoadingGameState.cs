using System;
using SuperCaissiere.Engine.Graphics;
using SuperCaissiere.Engine.Particules;
using Microsoft.Xna.Framework;

namespace SuperCaissiere.Engine.Core
{
    /// <summary>
    /// Screens of the game : menu, ingame, game over, etc.
    /// </summary>
    public abstract class LoadingGameState : GameState
    {
        public LoadingGameState()
        {
        }


        /// <summary>
        /// Load content for this state
        /// </summary>
        protected override void LoadContent() { }

        /// <summary>
        /// Custom loading routine
        /// </summary>
        protected override void InternalLoad() { }

        /// <summary>
        /// Update the game state
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            SceneCamera.Update(gameTime);
            ParticuleManager.Update(gameTime);
        }

        public void SetNextGameState(GameState nextGameState)
        {
            NextGameState = nextGameState;
        }

        public bool IsNextGameStateLoadingComplete {get;set;}

        /// <summary>
        /// Render the game state
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatchProxy spriteBatch) { }

        /// <summary>
        /// If true, game state will be changed in the next frame
        /// </summary>
        public override bool ChangeCurrentGameState { get; protected set; }

        /// <summary>
        /// If ChangeCurrentGameState is true, the current game state will become this one
        /// </summary>
        public override GameState NextGameState { get; protected set; }
    }
}
