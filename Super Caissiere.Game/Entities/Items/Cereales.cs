using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.Physics;

namespace Super_Caissiere.Entities.Items
{
    [TextureContent(AssetName = "cereales", AssetPath = "gfxs/sprites/cereales", LoadOnStartup = false)]
    [Model3DContent(AssetName = "cereales_mod", AssetPath = "models/cereals", LoadOnStartup = false)]
    [Model3DContent(AssetName = "cereales_col", AssetPath = "models/cereals_col", LoadOnStartup = false)]
    public class Cereales : Product
    {
        public Cereales()
            : base("cereales", new Microsoft.Xna.Framework.Rectangle(0, 0, 130, 190), "cereales_mod", "cereales_col")
        {
            hitbox = new Hitbox(DstRect);
            SetSpriteOriginToMiddle();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override SuperCaissiere.Engine.World.Entity Clone()
        {
            return new Cereales();
        }

        public override float Price
        {
            get { return 2.99f; }
        }
    }
}
