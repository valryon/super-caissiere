using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.World;
using Microsoft.Xna.Framework;

namespace Super_Caissiere.Entities.Items
{
    public abstract class ItemBase : Entity
    {
        public ItemBase(string assetName, Rectangle src)
            : base(assetName, Vector2.Zero, src, Vector2.One)
        {

        }

        public abstract float Price
        { get; }


        public static ItemBase GetRandomItem()
        {
            var list = (from t in typeof(ItemBase).Assembly.GetTypes()
             where t.IsSubclassOf(typeof(ItemBase))
             select t).ToList();

            var selectedType = list.GetRandomElement();

            // Instancie l'objet à partir du type
            return (ItemBase)selectedType.GetConstructor(new Type[0]).Invoke(null);
        }
    }
}
