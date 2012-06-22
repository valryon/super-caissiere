using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.World;
using Microsoft.Xna.Framework;

namespace Super_Caissiere.Entities.Items
{
    public abstract class Product : Entity
    {
        public Product(string assetName, Rectangle src)
            : base(assetName, Vector2.Zero, src, Vector2.One)
        {

        }

        public abstract float Price
        { get; }


        public static Product GetRandomItem()
        {
            var list = (from t in typeof(Product).Assembly.GetTypes()
             where t.IsSubclassOf(typeof(Product))
             select t).ToList();

            var selectedType = list.GetRandomElement();

            // Instancie l'objet à partir du type
            return (Product)selectedType.GetConstructor(new Type[0]).Invoke(null);
        }
    }
}
