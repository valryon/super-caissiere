using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Core;
using Microsoft.Xna.Framework;
using SuperCaissiere.Engine.Input.Devices;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.World;

namespace Super_Caissiere.States
{
    [TextureContent(AssetName = "badend", AssetPath = "gfxs/gameover/badend", LoadOnStartup = true)]
    [TextureContent(AssetName = "happyend", AssetPath = "gfxs/gameover/happyend", LoadOnStartup = true)]
    public class BackgroundVomitif : Entity
    {
        private bool win = false;

        public BackgroundVomitif(bool _win)
            : base(_win ? "happyend" : "badend", new Vector2(400, 300), new Rectangle(0, 0, 800, 600), new Vector2(1.25f, 1.25f))
        {
            win = _win;
            SetSpriteOriginToMiddle();
        }

        public override void Update(GameTime gameTime)
        {
            if (win) rotation += 0.01f;
            base.Update(gameTime);
        }

        public override Entity Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class EndgameState : GameState
    {
        public bool Win { get; set; }

        private BackgroundVomitif backgroundVomitif;
        private float zoomDelta;

        protected override void LoadContent()
        {
            zoomDelta = 0.025f;
        }

        protected override void InternalLoad()
        {
            backgroundVomitif = new BackgroundVomitif(Win);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            backgroundVomitif.Update(gameTime);

            var key = Application.InputManager.GetDevice<KeyboardDevice>(SuperCaissiere.Engine.Input.LogicalPlayerIndex.One);
            if (key.GetState(SuperCaissiere.Engine.Input.MappingButtons.A).IsPressed)
            {
                ChangeCurrentGameState = true;
                NextGameState = Application.GameStateManager.GetGameState<HomeState>();
            }

            if (Win == false)
            {
                SceneCamera.Zoom += zoomDelta;

                if (SceneCamera.Zoom < 0.8f) zoomDelta = -zoomDelta;
                if (SceneCamera.Zoom > 1.2f) zoomDelta = -zoomDelta;
            }
        }

        public override void Draw(SuperCaissiere.Engine.Graphics.SpriteBatchProxy spriteBatch)
        {
            spriteBatch.Begin(SceneCamera);
            backgroundVomitif.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.BeginNoCamera();

            if (Win)
            {
                spriteBatch.DrawString(Application.MagicContentManager.Font, "C'est la fin de la journée, dormir un peu et vous revoir demain !", new Vector2(70, 300), Color.GhostWhite);
            }
            else
            {
                spriteBatch.DrawString(Application.MagicContentManager.Font, "Vous avez échoué dans votre tâche caisse, nous ne serons jamais rencontre à nouveau", new Vector2(40, 300), Color.GhostWhite);
            }

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
