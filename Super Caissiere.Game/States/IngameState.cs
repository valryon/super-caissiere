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
        private DateTime time;
        private Queue<Client> liste_client;
        private ItemBase produit_courant;
        private bool boobool;
        private bool boobool2;
        private Rectangle scanner_zone;
        private float scanner_color;
        private Interpolator scanner_interpolator;
        private ClientBasket panier;
        

        protected override void LoadContent()
        {
        }

        protected override void InternalLoad()
        {
            m_cashier = new Cashier();
            m_hand = new Hand();
            liste_client = new Queue<Client>();
            panier = new ClientBasket();
            time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            m_cashier.Update(gameTime);
            m_hand.Update(gameTime);
            if (liste_client.FirstOrDefault() == null)
            {
                liste_client.Enqueue(new Client(new Vector2(500, 170), new Vector2(500, 400)));

            }
            foreach (Client cli in liste_client.ToList())
            {
                cli.Update(gameTime);
                foreach (ItemBase produits in cli.Items.ToList())
                {
                    produits.Update(gameTime);
                }
            }

            time = time.AddSeconds(gameTime.ElapsedGameTime.TotalSeconds);

            //appuyer sur espace
            var key = Application.InputManager.GetDevice<KeyboardDevice>(SuperCaissiere.Engine.Input.LogicalPlayerIndex.One);
            if (key.GetState(SuperCaissiere.Engine.Input.MappingButtons.A).IsPressed && boobool2==false)
            {
                if (produit_courant == null)
                {
                    boobool = false;
                    produit_courant = liste_client.First().Items.First();
                    Timer.Create(0.02F, true, (t =>
                    {
                        produit_courant.Location += new Vector2(-5, 0);
                        if (produit_courant.Location.X < 300)
                        {
                            t.Stop();
                        }
                    }));
                }
                else
                {
                    if (boobool == false)
                    {
                        Timer.Create(0.02F, true, (t =>
                        {
                            foreach (ItemBase item in liste_client.First().Items)
                            {
                                if (item != produit_courant)
                                {
                                    item.Location += new Vector2(-5, 0);
                                    if (item.Location.X < 450)
                                    {
                                        t.Stop();
                                        boobool = true;
                                    }

                                }
                            }
                        }));
                    }
                }
            }

            var mouse = Application.InputManager.GetDevice<MouseDevice>(SuperCaissiere.Engine.Input.LogicalPlayerIndex.One);
            if (mouse.GetState(SuperCaissiere.Engine.Input.MappingButtons.A).IsPressed)
            {
               
                scanner_color = 1;
                int larg = 50;
                int haut = 100;
                scanner_zone = new Rectangle(m_hand.DstRect.Left + larg, m_hand.DstRect.Top + haut, m_hand.DstRect.Width - (larg*2), m_hand.DstRect.Height - (haut*2));
                if (produit_courant != null) { 
                    if(scanner_zone.Intersects(produit_courant.DstRect)){
                        boobool2 = true;
                        Timer.Create(0.02F, true, (t =>
                        {
                            produit_courant.Location += new Vector2(-5, 0);
                            if (produit_courant.Location.X < 100)
                            {
                                t.Stop();
                                boobool2 = false;
                                panier.AddItem(produit_courant);
                                liste_client.First().Items.Dequeue();
                                produit_courant = null;

                            }
                        }));
                    }
                 }
                // Timer.Create(0.02F, true, (t => {
                if (scanner_interpolator != null)
                {
                    scanner_interpolator.Stop();
                    scanner_interpolator = null;
                }
                   scanner_interpolator= Interpolator.Create(1.0F, 0F, 0.35F, (i => {
                        scanner_color = i.Value;
                    }), (i => {
                        scanner_zone = Rectangle.Empty;
                    }));
              //  }));

            }
            panier.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SuperCaissiere.Engine.Graphics.SpriteBatchProxy spriteBatch)
        {
            spriteBatch.Begin(SceneCamera);
            // les clients
            foreach (Client cli in liste_client.ToList())
            {
                cli.Draw(spriteBatch);
            }

            // La caisse
            m_cashier.Draw(spriteBatch);

            foreach (Client cli in liste_client.ToList())
            {
                foreach (ItemBase produits in cli.Items.ToList())
                {
                    produits.Draw(spriteBatch);
                }
            }





            // La main en dernier
            m_hand.Draw(spriteBatch);
            if (scanner_zone != Rectangle.Empty)
            {
                spriteBatch.DrawRectangle(scanner_zone, Color.Red * scanner_color);
            }
            spriteBatch.DrawString(Application.MagicContentManager.Font, time.ToString(), new Vector2(10, 10), Color.Chartreuse);

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
