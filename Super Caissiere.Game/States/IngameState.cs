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
using SuperCaissiere.Engine.UI;

namespace Super_Caissiere.States
{
    [TextureContent(AssetName = "ingamebg", AssetPath = "gfxs/ingame/background", LoadOnStartup = true)]
    [TextureContent(AssetName = "boss", AssetPath = "gfxs/sprites/boss", LoadOnStartup = true)]
    [TextureContent(AssetName = "rank", AssetPath = "gfxs/ingame/rank", LoadOnStartup = true)]
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
        private Rectangle m_bossZone;
        private float m_scannerColor;
        private Interpolator m_scannerInterpolator;
        private ClientBasket m_basket;
        private Model3DRenderer m_render;
        private BarCodeQTE m_barCodeQte;
        private int m_scanTime;
        private TextBox m_textbox;

        private bool m_manualMode = false;

        private bool m_scanning = false;
        private float m_timerScan;

        private Rectangle m_rank;
        private bool m_rankActive = false;

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
            //m_barCodeQte = new BarCodeQTE(
            m_ending = false;
            m_pauseMidi = false;
            m_pauseMidiAnimation = false;
            m_bossZone = new Rectangle(Application.Graphics.GraphicsDevice.Viewport.Width - 80, 30, 80, 120);
            SceneCamera.FadeOut(40, null, Color.Chocolate);
            m_barCodeQte = new BarCodeQTE();
            m_rank = new Rectangle(0, 0, 300, 150);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (m_textbox == null || !m_textbox.IsModal)
            {
                if (m_textbox != null)
                {
                    

                }
                // Mise à jour du temps
                float delta = 3;
                delta = 1; // DEBUG
                m_time = m_time.AddMinutes(gameTime.ElapsedGameTime.TotalSeconds * delta);

                if (m_scanning) m_timerScan += gameTime.ElapsedGameTime.Milliseconds;
               
                m_render.Update(gameTime);
                if (m_pauseMidi == false)
                {
                    // Mise à jour des entités
                    m_cashier.Update(gameTime);
                    m_hand.Update(gameTime);
                    m_basket.Update(gameTime);

                    if (m_currentProduct != null)
                    {
                        if (!m_manualMode)
                            m_render.Update(gameTime);
                        else
                            m_barCodeQte.Update(gameTime);
                    }
                    // Ajouter un client s'il n'y en a plus
                    if (m_clientList.FirstOrDefault() == null)
                    {
                        m_clientList.Enqueue(new Client(new Vector2(500, 50), new Vector2(500, 450)));
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

            }
            else
            {
                m_textbox.Update(gameTime);
                var key = Application.InputManager.GetDevice<KeyboardDevice>(SuperCaissiere.Engine.Input.LogicalPlayerIndex.One);
                if (key.GetState(SuperCaissiere.Engine.Input.MappingButtons.A).IsPressed)
                    m_textbox = null;

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

            if (m_scanning && !m_barCodeQte.isActive() && key.GetState(SuperCaissiere.Engine.Input.MappingButtons.B).IsPressed)
            {
                m_manualMode = true;
                m_barCodeQte.start();
            }

            if (m_scanning)
            {
                //ici on fait tourner les serviettes :3
                m_render.resetRotate();
                if (key.ThumbStickLeft.Y < 0) m_render.rotateX(-0.05f); //up
                if (key.ThumbStickLeft.Y > 0) m_render.rotateX(0.05f); //down
                if (key.ThumbStickLeft.X < 0) m_render.rotateY(-0.05f); //left
                if (key.ThumbStickLeft.X > 0) m_render.rotateY(0.05f);  //right
            }
            if (m_textbox == null && m_scanning)
                if (key.GetState(SuperCaissiere.Engine.Input.MappingButtons.A).IsPressed) m_hp--;

            // Possible uniquement si on ne fait rien d'autre
            if (key.GetState(SuperCaissiere.Engine.Input.MappingButtons.A).IsDown )
            {
                if (m_currentProduct == null)
                {
                    var firstClient = m_clientList.FirstOrDefault();
                    if (firstClient != null)
                    {
                        foreach (Product p in firstClient.Items)
                        {
                            p.Location+=new Vector2(-2,0);
                            if (p.Location.X < 450)
                            {
                                m_currentProduct = p;
                                Timer.Create(0.02F, true, (t =>
                                {
                                    m_currentProduct.Location += new Vector2(-5, 0);
                                    if (m_currentProduct.Location.X < 300)
                                    {
                                        t.Stop();
                                        m_render.setModel(m_currentProduct.getModel(), m_currentProduct.getCollider());
                                        m_render.isActive = true;
                                        m_manualMode = false;
                                        m_scanTime = 0;
                                        m_scanning = true;
                                        m_timerScan = 0;
                                    }
                                }));

                            }
                        }
                    }
                }
                
               
            }

            // Clic sur la souris = SCAN
            if (m_scanning)
            {
                var mouse = Application.InputManager.GetDevice<MouseDevice>(SuperCaissiere.Engine.Input.LogicalPlayerIndex.One);
                bool validate = false;
                if (mouse.GetState(SuperCaissiere.Engine.Input.MappingButtons.A).IsPressed)
                {
                    if (m_render.isClicked((int)mouse.MouseLocation.X, (int)mouse.MouseLocation.Y))
                    {
                        m_scanTime++;
                        if (m_scanTime > 10)
                        {
                            m_textbox = new TextBox("Astus:\n Ce code bare semble être corrompu, presser \n'Ctrl' pour commuté au mode manuel", true);
                        }
                        if (!m_currentProduct.IsCorrupted) validate = true;
                    }
                }
                else if (m_manualMode &&  m_barCodeQte.isValidated)
                {
                    validate = true;
                }

                if (validate)
                {
                    Console.Beep();
                    m_scanning = false;
                    displayRank(m_timerScan);

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
                            m_scanTime = 0;
                        }
                    }));
                    m_render.isActive = false;
                }
            }
        }

        private void displayRank(float _time)
        {
            int d = 0;
            int off=0;

            if (_time < 500)
            {
                d = 10;
            }
            else if (_time < 1000)
            {
                off=1;
                d = 5;
            }
            else if (_time < 2000)
            {
                off=2;
                d = 0;
            }
            else if (_time < 3000)
            {
                off=3;
                d = -5;
            }
            else if (_time < 5000)
            {
                off=4;
                d = -10;
            }
            else
            {
                off=5;
                d = -20;
            }
            m_hp += d;
            m_rankActive = true;
            m_rank.Y = 150*off; 
            Timer.Create(2, true, (t =>
            {
                m_rankActive = false;
            }));

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
                    for (int i = list.Count; i > 0; i--)
                    {
                        list[i - 1].Draw(spriteBatch);
                    }
                }

                // Le panier
                m_basket.Draw(spriteBatch);

                //draw models3D
                if (m_currentProduct != null)
                {
                    if (!m_manualMode)
                    {
                        spriteBatch.End();
                        m_render.Draw();
                        spriteBatch.Begin(SceneCamera);
                    }
                    else
                    {
                        m_barCodeQte.Draw(spriteBatch);
                    }
                }

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
                int d = 0;
                if (m_hp < 70) d = 1;
                if (m_hp < 30) d = 2;
                spriteBatch.DrawString(Application.MagicContentManager.Font, "Bonheur de patron : " + m_hp + " %", new Vector2(Application.Graphics.GraphicsDevice.Viewport.Width - 230, 5), color);
                Rectangle r = new Rectangle(d * 133, 0, 133, 200);

                spriteBatch.Draw(Application.MagicContentManager.GetTexture("boss"), m_bossZone, r, Color.White);
                int size = (int)((m_hp / 100f) * 120f);
                spriteBatch.DrawRectangle(new Rectangle(Application.Graphics.GraphicsDevice.Viewport.Width - 100, 150 - size, 10, size), color);
                if (m_rankActive)
                {
                    spriteBatch.Draw(Application.MagicContentManager.GetTexture("rank"), new Rectangle((Application.Graphics.GraphicsDevice.Viewport.Width - 300) / 2, (Application.Graphics.GraphicsDevice.Viewport.Height - 150) / 2, 300, 150), m_rank, Color.White);
                }

            }
            // Pause du midi
            else
            {
                spriteBatch.Draw(Application.MagicContentManager.GetTexture("ingamebgpause"), SceneCamera.VisibilityRectangle, Color.White);
            }

            spriteBatch.DrawString(Application.MagicContentManager.Font, m_time.ToString(), new Vector2(10, 10), Color.Chartreuse);
            if (m_textbox != null) m_textbox.Draw(spriteBatch);
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
