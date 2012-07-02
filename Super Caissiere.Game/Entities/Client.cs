using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.World;
using SuperCaissiere.Engine.Content;
using Microsoft.Xna.Framework;
using SuperCaissiere.Engine.Core;
using Super_Caissiere.Entities.Items;

namespace Super_Caissiere.Entities
{

   


    public class Client : Entity
    {

        public static string[] client_event = {   "Bonjour,\n quel mauvais temps aujourd'hui!" ,
                                                  ""
                                              };


        private Queue<Product> m_items;
        private ClientBody body;
        private ClientHead head;

        public Client(Vector2 location,Vector2 location_produit)
            : base(null, location, Rectangle.Empty, Vector2.One)
        {
            int randomNumberFromGod = Application.Random.GetRandomInt(156475);

            if (randomNumberFromGod % 2 == 1) IsWoman = true;

            body = new ClientBody(this);
            head = new ClientHead(this, Application.Random.GetRandomInt(0,2));

            // Produits du client
            m_items = new Queue<Product>();
            Vector2 delta = new Vector2(0, 0);

            for (int i = 0; i < Application.Random.GetRandomInt(10) + 1; i++)
            {
                var item = Product.GetRandomItem();
                item.Location = location_produit+delta;
                item.Location += new Vector2(0,Application.Random.GetRandomInt(-10, 10));
                m_items.Enqueue(item);
                delta += new Vector2(90, 0);
            }

        }

        public override void Update(GameTime gameTime)
        {
            body.Update(gameTime);
            head.Update(gameTime);
           
            base.Update(gameTime);
        }

        public override void Draw(SuperCaissiere.Engine.Graphics.SpriteBatchProxy spriteBatch)
        {
            body.Draw(spriteBatch);
            head.Draw(spriteBatch);
        }

        public bool IsWoman
        {
            get;
            private set;
        }

        public Queue<Product> Items
        {
            get
            {
                return m_items;
            }
        }

        public override Entity Clone()
        {
            return new Client(Location,Location);
        }
    }

    [TextureContent(AssetName = "corps", AssetPath = "gfxs/sprites/corps", LoadOnStartup = false)]
    public class ClientBody : Entity
    {
        private Client client;

        public ClientBody(Client c)
            : base("corps", Vector2.Zero, new Rectangle(c.IsWoman ? 128 : 0, 0, 128, c.IsWoman ? 256 : 220), Vector2.One*2)
        {
            client = c;
            updateLocation();
        }

        public override void Update(GameTime gameTime)
        {
            updateLocation();
            base.Update(gameTime);
        }


        private void updateLocation()
        {
            Location = client.Location + new Vector2(0, 155);
        }

        public override Entity Clone()
        {
            return new ClientBody(client);
        }
    }

    [TextureContent(AssetName = "tetes", AssetPath = "gfxs/sprites/tetes", LoadOnStartup = false)]
    public class ClientHead : Entity
    {
        private Client client;
        private int type;
        public ClientHead(Client c, int t)
            : base("tetes", Vector2.Zero, new Rectangle(c.IsWoman ? 80 : 0, 80*t, 80, 80), Vector2.One*2)
        {
            type = t;
            client = c;
            updateLocation();
        }
        public override void Update(GameTime gameTime)
        {
            updateLocation();
            base.Update(gameTime);
        }

        private void updateLocation()
        {
            Location = client.Location + new Vector2(25,0);
        }

        public override Entity Clone()
        {
            return new ClientHead(client, type);
        }
    }

}
