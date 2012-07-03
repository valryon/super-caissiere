using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.Physics;

namespace Super_Caissiere.Entities.Items
{
    [TextureContent(AssetName = "burger", AssetPath = "gfxs/sprites/burger", LoadOnStartup = false)]
    [Model3DContent(AssetName = "burger_mod", AssetPath = "models/burger", LoadOnStartup = false)]
    [Model3DContent(AssetName = "burger_col", AssetPath = "models/burger_col", LoadOnStartup = false)]
    public class Burger : Product
    {
        public Burger()
            : base("burger", new Microsoft.Xna.Framework.Rectangle(0, 0, 120, 121), "burger_mod", "burger_col")
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
            return new Burger();
        }

        public override float Price
        {
            get { return 2.08f; }
        }
    }
}
