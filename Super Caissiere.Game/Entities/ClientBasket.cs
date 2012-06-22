using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.World;
using Microsoft.Xna.Framework;
using Super_Caissiere.Entities.Items;
using SuperCaissiere.Engine.Core;

namespace Super_Caissiere.Entities
{
    [TextureContent(AssetName = "panier", AssetPath = "gfxs/sprites/panier", LoadOnStartup = false)]
    public class ClientBasket : Entity
    {
        public ClientBasket()
            : base("panier", new Vector2(0, 260), new Rectangle(0, 0, 256, 256), Vector2.One)
        {
            Items = new List<Product>();

            Rectangle dim = new Rectangle((int)Location.X + 50, (int)Location.Y + 100, DstRect.Width - 100, DstRect.Height - 100);
            hitbox = new SuperCaissiere.Engine.Physics.Hitbox(dim);
        }

        public void AddItem(Product item)
        {
            item.Location = hitbox.Dimensions.Center.ToVector2();
            item.Rotation = Application.Random.GetRandomFloat(0, 2 * Math.PI);

            Items.Add(item);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public override void Draw(SuperCaissiere.Engine.Graphics.SpriteBatchProxy spriteBatch)
        {
            // On dessine deux fois le panier pour avoir de la profondeur !
            base.Draw(spriteBatch);

            Items.ForEach(i => i.Draw(spriteBatch));

            // On dessine deux fois le panier pour avoir de la profondeur !
            //base.Draw(spriteBatch);

            hitbox.Draw(spriteBatch);
        }

        public override Entity Clone()
        {
            return new ClientBasket();
        }

        public List<Product> Items { get; private set; }
    }
}
