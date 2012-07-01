using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Xna.Framework;
using Super_Caissiere.Entities;
using SuperCaissiere.Engine.Core;
using Super_Caissiere.Entities.Items;
using Microsoft.Xna.Framework.Input;
using SuperCaissiere.Engine.Input.Devices;
using SuperCaissiere.Engine.Utils;
using SuperCaissiere.Engine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using SuperCaissiere.Engine.Content;

namespace Super_Caissiere.States
{
    [TextureContent(AssetName = "ingamebg", AssetPath = "gfxs/ingame/background", LoadOnStartup = true)]
    [TextureContent(AssetName = "ingamebgpause", AssetPath = "gfxs/ingame/background_pause", LoadOnStartup = true)]
    public class IngameState : GameState
    {

        private Cashier m_cashier;
        private Hand m_hand;
        private DateTime m_time;
        private Queue<Client> m_clientList;
        private Product m_currentProduct;
        private bool m_clientProductsAnimationComplete;
        private bool m_isAnimatingScanner;
        private Rectangle m_scannerZone;
        private float m_scannerColor;
        private Interpolator m_scannerInterpolator;
        private ClientBasket m_basket;
        private Model3DRenderer m_render;

        private bool m_ending;
        private int m_hp;
        private bool m_pauseMidi, m_pauseMidiAnimation;

        protected override void LoadContent()
        {
        }

        protected override void InternalLoad()
        {
            m_hp = 100;

            m_cashier = new Cashier();
            m_hand = new Hand();
            m_clientList = new Queue<Client>();
            m_basket = new ClientBasket();
            m_time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
            Matrix world, view, projection;
            world = Matrix.Identity;
            view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Application.Graphics.GraphicsDevice.Viewport.AspectRatio, 1, 10);
            m_render = new Model3DRenderer(Application.Graphics.GraphicsDevice, Application.SpriteBatch, projection, view, world);

            m_ending = false;
            m_pauseMidi = false;
            m_pauseMidiAnimation = false;

            SceneCamera.FadeOut(40, null, Color.Chocolate);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // Mise à jour du temps
            float delta = 3;
            delta = 1; // DEBUG
            m_time = m_time.AddMinutes(gameTime.ElapsedGameTime.TotalSeconds * delta);
            m_render.Update(gameTime);
            if (m_pauseMidi == false)
            {
                // Mise à jour des entités
                m_cashier.Update(gameTime);
                m_hand.Update(gameTime);
                m_basket.Update(gameTime);
                m_render.Update(gameTime);

                // Ajouter un client s'il n'y en a plus
                if (m_clientList.FirstOrDefault() == null)
                {
                    m_clientList.Enqueue(new Client(new Vector2(500, 170), new Vector2(500, 450)));
                }

                foreach (Client cli in m_clientList.ToList())
                {
                    cli.Update(gameTime);
                    foreach (Product produits in cli.Items.ToList())
                    {
                        produits.Update(gameTime);
                    }
                }

                manageEnd();

                // Gestion entrées joueur
                handleInput();
            }
            // Pause du midi
            else
            {
                if (m_time.Hour > 13)
                {
                    if (m_pauseMidiAnimation)
                    {
                        m_pauseMidiAnimation = false;

                        SceneCamera.FadeIn(30, () =>
                        {
                            m_pauseMidi = false;
                            SceneCamera.FadeOut(30, null, Color.White);
                        }, Color.HotPink);
                    }
                }
            }

            base.Update(gameTime);
        }

        private void manageEnd()
        {
            bool endGame = false;
            bool isWin = true;

            // Pause du midi
            if ((m_time.Hour > 11 && m_time.Hour < 14) && (m_pauseMidiAnimation == false))
            {
                m_pauseMidiAnimation = true;

                SceneCamera.FadeIn(30, () =>
                {
                    m_pauseMidi = true;
                    SceneCamera.FadeOut(30, null, Color.White);
                }, Color.HotPink);

            }
            // Fin de la journée
            else if (m_time.Hour > 17)
            //else if (m_time.Second > 5) // Debug T.T
            {
                endGame = true;
                isWin = true;
            }

            // Virée ?
            if (m_hp <= 0)
            {
                endGame = true;
                isWin = false;
            }

            if (endGame)
            {
                if (m_ending == false)
                {
                    m_ending = true;
                    SceneCamera.FadeIn(120, () =>
                    {
                        var state = (EndgameState)Application.GameStateManager.GetGameState<EndgameState>();
                        state.Win = isWin;

                        ChangeCurrentGameState = true;
                        NextGameState = state;
                    }, Color.Black);
                }
            }
        }

        private void handleInput()
        {
            // Appui sur espace : ACTION
            var key = Application.InputManager.GetDevice<KeyboardDevice>(SuperCaissiere.Engine.Input.LogicalPlayerIndex.One);

            //ici on fait tourner les serviettes :3
            m_render.resetRotate();
            if (key.ThumbStickLeft.Y < 0) m_render.rotateX(-0.05f); //up
            if (key.ThumbStickLeft.Y > 0) m_render.rotateX(0.05f); //down
            if (key.ThumbStickLeft.X < 0) m_render.rotateY(-0.05f); //left
            if (key.ThumbStickLeft.X > 0) m_render.rotateY(0.05f);  //right

            if (key.GetState(SuperCaissiere.Engine.Input.MappingButtons.A).IsDown) m_hp--;

            // Possible uniquement si on ne fait rien d'autre
            if (key.GetState(SuperCaissiere.Engine.Input.MappingButtons.A).IsPressed && m_isAnimatingScanner == false)
            {
                // Pas de produit devant le joueur : on en met un
                if (m_currentProduct == null)
                {
                    m_clientProductsAnimationComplete = false;

                    var firstClient = m_clientList.FirstOrDefault();

                    if (firstClient != null)
                    {
                        m_currentProduct = firstClient.Items.FirstOrDefault();

                        if (m_currentProduct != null)
                        {
                            Timer.Create(0.02F, true, (t =>
                            {
                                m_currentProduct.Location += new Vector2(-5, 0);
                                if (m_currentProduct.Location.X < 300)
                                {
                                    t.Stop();
                                    m_render.setModel(m_currentProduct.getModel(), m_currentProduct.getCollider());
                                    m_render.isActive = true;
                                }
                            }));
                        }
                    }
                }
                else
                {
                    // On décale les produits qui sont sur le tapis
                    if (m_clientProductsAnimationComplete == false)
                    {
                        Timer.Create(0.02F, true, (t =>
                        {
                            foreach (Product item in m_clientList.First().Items)
                            {
                                if (item != m_currentProduct)
                                {
                                    item.Location += new Vector2(-5, 0);
                                    if (item.Location.X < 450)
                                    {
                                        t.Stop();
                                        m_clientProductsAnimationComplete = true;
                                    }

                                }
                            }
                        }));
                    }
                }
            }

            // Clic sur la souris = SCAN
            var mouse = Application.InputManager.GetDevice<MouseDevice>(SuperCaissiere.Engine.Input.LogicalPlayerIndex.One);
            if (m_render.isActive && mouse.GetState(SuperCaissiere.Engine.Input.MappingButtons.A).IsPressed)
            {
                
                if (m_render.isClicked((int)mouse.MouseLocation.X, (int)mouse.MouseLocation.Y))
                {
                    Console.Beep();
                    Timer.Create(0.02F, true, (t =>
                    {
                        // Déplacement du produit dans le panier final
                        m_currentProduct.Location += new Vector2(-5, 0);

                        if (m_currentProduct.Location.X < 100)
                        {
                            t.Stop();
                            m_isAnimatingScanner = false;
                            m_basket.AddItem(m_currentProduct);
                            m_clientList.First().Items.Dequeue();
                            m_currentProduct = null;

                        }
                    }));
                    m_render.isActive = false;

                }

            }
           /*
            if (mouse.GetState(SuperCaissiere.Engine.Input.MappingButtons.A).IsPressed)
            {
                // On regarde s'il y a collision
                m_scannerColor = 1;
                int larg = 50;
                int haut = 100;

                m_scannerZone = new Rectangle(m_hand.DstRect.Left + larg, m_hand.DstRect.Top + haut, m_hand.DstRect.Width - (larg * 2), m_hand.DstRect.Height - (haut * 2));

                // Avec le produit courant
                if (m_currentProduct != null)
                {
                    if (m_scannerZone.Intersects(m_currentProduct.DstRect))
                    {
                        // Collision
                        m_isAnimatingScanner = true;

                        Timer.Create(0.02F, true, (t =>
                        {
                            // Déplacement du produit dans le panier final
                            m_currentProduct.Location += new Vector2(-5, 0);

                            if (m_currentProduct.Location.X < 100)
                            {
                                t.Stop();
                                m_isAnimatingScanner = false;
                                m_basket.AddItem(m_currentProduct);
                                m_clientList.First().Items.Dequeue();
                                m_currentProduct = null;

                            }
                        }));
                    }
                }
           
                // Animation du scanner
                if (m_scannerInterpolator != null)
                {
                    m_scannerInterpolator.Stop();
                    m_scannerInterpolator = null;
                }
                m_scannerInterpolator = Interpolator.Create(1.0F, 0F, 0.35F, (i =>
                {
                    m_scannerColor = i.Value;
                }), (i =>
                {
                    m_scannerZone = Rectangle.Empty;
                }));
            }*/
        }

        public override void Draw(SuperCaissiere.Engine.Graphics.SpriteBatchProxy spriteBatch)
        {
            spriteBatch.Begin(SceneCamera);

            if (m_pauseMidi == false)
            {
                spriteBatch.Draw(Application.MagicContentManager.GetTexture("ingamebg"), SceneCamera.VisibilityRectangle, Color.White);

                // les clients
                foreach (Client cli in m_clientList.ToList())
                {
                    cli.Draw(spriteBatch);
                }

                // La caisse
                m_cashier.Draw(spriteBatch);

                foreach (Client cli in m_clientList.ToList())
                {
                    var list = cli.Items.ToList();
                    for(int  i= list.Count;i>0; i--){
                        list[i-1].Draw(spriteBatch);
                    }
                }

                // Le panier
                m_basket.Draw(spriteBatch);
                spriteBatch.End();
                //draw models3D

                m_render.Draw();

                spriteBatch.Begin(SceneCamera);

                // La main en dernier
                m_hand.Draw(spriteBatch);
                if (m_scannerZone != Rectangle.Empty)
                {
                    spriteBatch.DrawRectangle(m_scannerZone, Color.Red * m_scannerColor);
                }
                //panier.Draw(spriteBatch);

                Color color = Color.White;

                if (m_hp < 90) color = Color.Yellow;
                if (m_hp < 80) color = Color.YellowGreen;
                if (m_hp < 70) color = Color.Orange;
                if (m_hp < 60) color = Color.OrangeRed;
                if (m_hp < 60) color = Color.Red;
                if (m_hp < 50) color = Color.RosyBrown;
                if (m_hp < 40) color = Color.Purple;
                if (m_hp < 30) color = Color.PowderBlue;
                if (m_hp < 20) color = Color.MintCream;
                if (m_hp < 10) color = Color.Black;

                spriteBatch.DrawString(Application.MagicContentManager.Font, "Patron niveau bohneur : " + m_hp + " %", new Vector2(10, 30), color);
            }
            // Pause du midi
            else
            {
                spriteBatch.Draw(Application.MagicContentManager.GetTexture("ingamebgpause"), SceneCamera.VisibilityRectangle, Color.White);
            }

            spriteBatch.DrawString(Application.MagicContentManager.Font, m_time.ToString(), new Vector2(10, 10), Color.Chartreuse);

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
