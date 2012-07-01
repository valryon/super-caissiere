using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperCaissiere.Engine.Core;

namespace Super_Caissiere.Entities.Items
{
    public abstract class Product : Entity
    {
        string m_model, m_collider;
        public bool IsCorrupted{get; set;}

        public Product(string assetName, Rectangle src, string model_name, string collider_name)
            : base(assetName, Vector2.Zero, src, Vector2.One)
        {
            m_model = model_name;
            m_collider = collider_name;
            IsCorrupted = (Application.Random.GetRandomFloat(0, 1) > 0.8f);
        }

        public abstract float Price
        { get; }

        public Model getModel()
        {
            return Application.MagicContentManager.GetModel(m_model).Model3D;
        }

        public Model getCollider()
        {
            return Application.MagicContentManager.GetModel(m_collider).Model3D;
        }

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
