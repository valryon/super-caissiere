using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Core;
using SuperCaissiere.Engine.Content;
using Microsoft.Xna.Framework;
using SuperCaissiere.Engine.Input.Devices;

namespace Super_Caissiere.States
{
    [TextureContent(AssetName = "background", AssetPath = "gfxs/credits/background", LoadOnStartup = true)]
    public class CreditsGameState : GameState
    {
        private bool shakeshakeshake;

        protected override void LoadContent()
        {
        }

        protected override void InternalLoad()
        {
            shakeshakeshake = true;
            SceneCamera.FadeOut(20, null, Color.Beige);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var key = Application.InputManager.GetDevice<KeyboardDevice>(SuperCaissiere.Engine.Input.LogicalPlayerIndex.One);
            if (key.GetState(SuperCaissiere.Engine.Input.MappingButtons.A).IsPressed)
            {
                ChangeCurrentGameState = true;
                NextGameState = Application.GameStateManager.GetGameState<HomeState>();
            }

            if (shakeshakeshake)
            {
                SpecialEffects.SpecialEffectsHelper.ShakeScreen(new Vector2(10, 10), 10);
                shakeshakeshake = false;
            }
        }

        public override void Draw(SuperCaissiere.Engine.Graphics.SpriteBatchProxy spriteBatch)
        {
            spriteBatch.Begin(SceneCamera);

            spriteBatch.Draw(Application.MagicContentManager.GetTexture("background"), SceneCamera.VisibilityRectangle, Color.White);

            spriteBatch.DrawString(Application.MagicContentManager.Font, "RESPONSABLES", new Vector2(100, 150), Color.Cornsilk);
            spriteBatch.DrawString(Application.MagicContentManager.Font, "Lapinou Fou - programmateur informatique", new Vector2(100, 200), Color.LightSteelBlue);
            spriteBatch.DrawString(Application.MagicContentManager.Font, "Valryon - programmateur informatique", new Vector2(100, 240), Color.Fuchsia);
            spriteBatch.DrawString(Application.MagicContentManager.Font, "Yaki_ - jeu concepteur et programmateur informatique", new Vector2(100, 280), Color.Gainsboro);
            spriteBatch.DrawString(Application.MagicContentManager.Font, "Lolyan - dessinateuse", new Vector2(100, 320), Color.LightCyan);

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
