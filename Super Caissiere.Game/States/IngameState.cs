using SuperCaissiere.Engine.Core;
using Super_Caissiere.Entities;

namespace Super_Caissiere.States
{
    public class IngameState : GameState
    {
        private Cashier m_cashier;
        private Hand m_hand;

        protected override void LoadContent()
        {
        }

        protected override void InternalLoad()
        {
            m_cashier = new Cashier();
            m_hand = new Hand();
        }

        public override void Draw(SuperCaissiere.Engine.Graphics.SpriteBatchProxy spriteBatch)
        {
            spriteBatch.Begin(SceneCamera);

            m_cashier.Draw(spriteBatch);
            m_hand.Draw(spriteBatch);

            spriteBatch.End();
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
