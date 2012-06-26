using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Core;
using SuperCaissiere.Engine.Content;
using Microsoft.Xna.Framework;
using Super_Caissiere.SpecialEffects;
using SuperCaissiere.Engine.Utils;

namespace Super_Caissiere.States
{
    [TextureContent(AssetName = "caddie", AssetPath = "gfxs/menu/caddie", LoadOnStartup = true)]
    [TextureContent(AssetName = "menu", AssetPath = "gfxs/menu/menu", LoadOnStartup = true)]
    [TextureContent(AssetName = "title", AssetPath = "gfxs/menu/title", LoadOnStartup = true)]
    [TextureContent(AssetName = "subtitle1", AssetPath = "gfxs/menu/subtitle1", LoadOnStartup = true)]
    [TextureContent(AssetName = "subtitle2", AssetPath = "gfxs/menu/subtitle2", LoadOnStartup = true)]
    public class HomeState : GameState
    {
        private Vector2 m_titleLoc, m_sub1Loc, m_sub2Loc;
        private Rectangle m_titleDst, m_sub1Dst, m_sub2Dst, m_caddieDst;
        private Vector2 m_caddieLoc;
        private int m_selectedOption = 0;

        private bool m_isAnimationCompleted;

        protected override void LoadContent()
        {
        }

        protected override void InternalLoad()
        {
            m_isAnimationCompleted = false;

            m_titleLoc = new Vector2(50, -200);
            m_titleDst = new Rectangle(0, -200, 600, 100);

            m_sub1Loc = new Vector2(-600, 120);
            m_sub1Dst = new Rectangle(0, 0, 600, 100);

            m_sub2Loc = new Vector2(800, 180);
            m_sub2Dst = new Rectangle(0, 0, 600, 100);

            m_caddieLoc = new Vector2();
            m_caddieDst = new Rectangle(0, 0, 78, 82);

            NextGameState = Application.GameStateManager.GetGameState<IngameState>();

            SceneCamera.FadeIn(20, () =>
            {
                SceneCamera.FadeOut(20, () =>
                {
                    Interpolator.Create(-200, 40, 0.4f, (i) =>
                    {
                        m_titleLoc.Y = i.Value;
                    }, (i) =>
                    {
                        Interpolator.Create(-600, 0, 0.4f, (i2) =>
                        {
                            m_sub1Loc.X = i2.Value;
                        }, (i2) =>
                        {
                            Interpolator.Create(800, 200, 0.4f, (i3) =>
                            {
                                m_sub2Loc.X = i3.Value;
                            }, (i3) =>
                            {
                                Timer.Create(0.5f, false, (t) =>
                                {
                                    m_isAnimationCompleted = true;
                                });
                            });
                        });
                    });

                }, Color.White);
            }, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            m_titleDst.X = (int)m_titleLoc.X;
            m_titleDst.Y = (int)m_titleLoc.Y;
            m_sub1Dst.X = (int)m_sub1Loc.X;
            m_sub1Dst.Y = (int)m_sub1Loc.Y;
            m_sub2Dst.X = (int)m_sub2Loc.X;
            m_sub2Dst.Y = (int)m_sub2Loc.Y;
            m_caddieDst.X = (int)m_caddieLoc.X;
            m_caddieDst.Y = (int)m_caddieLoc.Y;

            if (m_isAnimationCompleted)
            {
                ChangeCurrentGameState = true;
            }

            base.Update(gameTime);
        }

        public override void Draw(SuperCaissiere.Engine.Graphics.SpriteBatchProxy spriteBatch)
        {
            spriteBatch.Begin(SceneCamera);

            spriteBatch.Draw(Application.MagicContentManager.GetTexture("menu"), SceneCamera.VisibilityRectangle, Color.White);

            spriteBatch.Draw(Application.MagicContentManager.GetTexture("title"), m_titleDst, Color.White);
            spriteBatch.Draw(Application.MagicContentManager.GetTexture("subtitle1"), m_sub1Dst, Color.White);
            spriteBatch.Draw(Application.MagicContentManager.GetTexture("subtitle2"), m_sub2Dst, Color.White);

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
