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

namespace Super_Caissiere.States
{
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


        protected override void LoadContent()
        {
        }

        protected override void InternalLoad()
        {
            m_cashier = new Cashier();
            m_hand = new Hand();
            m_clientList = new Queue<Client>();
            m_basket = new ClientBasket();
            m_time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // Mise à jour des entités
            m_cashier.Update(gameTime);
            m_hand.Update(gameTime);
            m_basket.Update(gameTime);

            // Ajouter un client s'il n'y en a plus
            if (m_clientList.FirstOrDefault() == null)
            {
                m_clientList.Enqueue(new Client(new Vector2(500, 170), new Vector2(500, 400)));

            }

            foreach (Client cli in m_clientList.ToList())
            {
                cli.Update(gameTime);
                foreach (Product produits in cli.Items.ToList())
                {
                    produits.Update(gameTime);
                }
            }

            // Mise à jour du temps
            m_time = m_time.AddMinutes(gameTime.ElapsedGameTime.TotalSeconds);

            // Gestion entrées joueur
            handleInput();

            base.Update(gameTime);
        }

        private void handleInput()
        {
            // Appui sur espace : ACTION
            var key = Application.InputManager.GetDevice<KeyboardDevice>(SuperCaissiere.Engine.Input.LogicalPlayerIndex.One);

            // Possible uniquement si on ne fait rien d'autre
            if (key.GetState(SuperCaissiere.Engine.Input.MappingButtons.A).IsPressed && m_isAnimatingScanner == false)
            {
                // Pas de produit devant le joueur : on en met un
                if (m_currentProduct == null)
                {
                    m_clientProductsAnimationComplete = false;

                    m_currentProduct = m_clientList.First().Items.First();

                    Timer.Create(0.02F, true, (t =>
                    {
                        m_currentProduct.Location += new Vector2(-5, 0);
                        if (m_currentProduct.Location.X < 300)
                        {
                            t.Stop();
                        }
                    }));
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
            }
        }

        public override void Draw(SuperCaissiere.Engine.Graphics.SpriteBatchProxy spriteBatch)
        {
            spriteBatch.Begin(SceneCamera);
            // les clients
            foreach (Client cli in m_clientList.ToList())
            {
                cli.Draw(spriteBatch);
            }

            // La caisse
            m_cashier.Draw(spriteBatch);

            foreach (Client cli in m_clientList.ToList())
            {
                foreach (Product produits in cli.Items.ToList())
                {
                    produits.Draw(spriteBatch);
                }
            }





            // La main en dernier
            m_hand.Draw(spriteBatch);
            if (m_scannerZone != Rectangle.Empty)
            {
                spriteBatch.DrawRectangle(m_scannerZone, Color.Red * m_scannerColor);
            }
            spriteBatch.DrawString(Application.MagicContentManager.Font, m_time.ToString(), new Vector2(10, 10), Color.Chartreuse);

            //panier.Draw(spriteBatch);
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
