using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Xna.Framework;
using Super_Caissiere.Entities;
using SuperCaissiere.Engine.Core;
using Super_Caissiere.Entities.Items;

namespace Super_Caissiere.States
{
    public class IngameState : GameState
    {
        private Vector2 caisseStart = new Vector2(500, 460);
        private const int spaceBetweenClient = 200;

        private Cashier m_cashier;
        private Hand m_hand;

        private Queue<Client> m_clients;
        private ItemBase m_currentItem;
        private Client m_currentClient;
        private ClientBasket m_basket;

        private float m_cooldown;

        protected override void LoadContent()
        {
        }

        protected override void InternalLoad()
        {
            m_cashier = new Cashier();
            m_hand = new Hand();
            m_basket = new ClientBasket();

            m_clients = new Queue<Client>();

            m_cooldown = 150f;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // Mise à jour des éléments joueur
            m_cashier.Update(gameTime);
            m_hand.Update(gameTime);

            // Client en cours existe ?
            if (m_currentClient != null)
            {
                m_currentClient.Update(gameTime);
            }
            else
            {
                // Le premier de la file devient le client en cours
                m_currentClient = m_clients.FirstOrDefault();

                // S'il y a avait un client on le fait passer devant
                if (m_currentClient != null)
                {
                    // TODO Animation
                    m_currentClient.Location = new Vector2(40, 170);

                    // TODO On décale tous les suivants
                }
            }

            // On ajoute régulièrement des nouveaux clients
            m_cooldown -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (m_cooldown <= 0)
            {
                // Dans la limite des stocks disponibles
                if (m_clients.Count < 3)
                {
                    // On calcule la position au passage
                    var client = new Client(new Vector2(300 + ((m_clients.Count) * 150), 170));
                    m_clients.Enqueue(client);

                    // On place les objets sur le taps
                    int lastClientLastItem = (int)m_clients.Last().Items.Last().Location.X;

                    for (int i = 0; i < client.Items.Count; i++)
                    {
                        var item = client.Items[i];
                        item.Location = caisseStart + new Vector2(lastClientLastItem + 50 * i, 0);
                    }

                    // On définit le temps avant le prochain client
                    m_cooldown = Application.Random.GetRandomFloat(500, 3000);
                }
            }

            // Mise à jour des clients et de leurs objets
            m_clients.ToList().ForEach(c =>
            {
                c.Update(gameTime);

                c.Items.ForEach(i =>
                {
                    i.Update(gameTime);
                });
            });

            // Y a-t-il un objet en train de voler
            if (m_currentItem == null)
            {
                // Si non on essaye de récupérer le premier du tapis
                var firstClient = m_clients.FirstOrDefault();
                if (firstClient != null)
                {
                    m_currentItem = firstClient.Items.FirstOrDefault();

                    if (m_currentItem == null)
                    {
                        // TODO On a fini de scanner, il faut faire payer le client

                        // Client suivant
                        m_clients.Dequeue();
                        m_currentClient = null;

                        m_basket.Clear();
                    }
                }
            }
            else
            {
                // Si oui on le déplace jusqu'au panier
                m_currentItem.Location += new Vector2(-5, 0);

                Vector2 depth;
                if (m_currentItem.Hitbox.Dimensions.GetIntersectionDepth(m_basket.Hitbox.Dimensions, out depth))
                {
                    m_basket.AddItem(m_currentItem);
                    m_currentClient.Items.Remove(m_currentItem);
                    m_currentItem = null;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SuperCaissiere.Engine.Graphics.SpriteBatchProxy spriteBatch)
        {
            spriteBatch.Begin(SceneCamera);

            // Les clients
            m_clients.ToList().ForEach(i => i.Draw(spriteBatch));

            if (m_currentClient != null)
            {
                m_currentClient.Draw(spriteBatch);
            }

            // La caisse
            m_cashier.Draw(spriteBatch);

            if (m_currentClient != null)
            {
                m_basket.Draw(spriteBatch);
            }

            // Les produits : un tas difforme pour chaque client
            foreach (Client c in m_clients)
            {
                foreach (ItemBase item in c.Items)
                {
                    item.Draw(spriteBatch);
                }
            }

            // La main en dernier
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
