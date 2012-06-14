using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Core;

namespace Super_Caissiere.States
{
    public class HomeState : GameState
    {
        protected override void LoadContent()
        {
        }

        protected override void InternalLoad()
        {
            ChangeCurrentGameState = true;
            NextGameState = Application.GameStateManager.GetGameState<IngameState>();
        }

        public override void Draw(SuperCaissiere.Engine.Graphics.SpriteBatchProxy spriteBatch)
        {
        }

        public override bool ChangeCurrentGameState
        {
            get;
            protected set;
        }

        public override GameState NextGameState
        {
            get;
            protected set;
        }
    }
}
