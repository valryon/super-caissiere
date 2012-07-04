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
    [TextureContent(AssetName = "creditsbg", AssetPath = "gfxs/credits/background", LoadOnStartup = true)]
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

            spriteBatch.Draw(Application.MagicContentManager.GetTexture("creditsbg"), SceneCamera.VisibilityRectangle, Color.White);
            spriteBatch.DrawRectangle(new Rectangle(100, 150, 450, 250), Color.GreenYellow);
            spriteBatch.DrawString(Application.MagicContentManager.Font, "RESPONSABLES", new Vector2(100, 150), Color.Red);
            spriteBatch.DrawString(Application.MagicContentManager.Font, "Monsieur_Lapinou - programmateur informatik", new Vector2(100, 200), Color.LightSteelBlue);
            spriteBatch.DrawString(Application.MagicContentManager.Font, "Valryon - programmateur informatik", new Vector2(100, 240), Color.DarkSalmon);
            spriteBatch.DrawString(Application.MagicContentManager.Font, "Yaki_ - jeu concepteur et programmateur informatik et acteur studio", new Vector2(100, 280), Color.DarkGoldenrod);
            spriteBatch.DrawString(Application.MagicContentManager.Font, "Lolyan - dessinateuse", new Vector2(100, 320), Color.PapayaWhip);
            spriteBatch.DrawString(Application.MagicContentManager.Font, "Glory - idée de commencement", new Vector2(100, 360), Color.Brown);
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
