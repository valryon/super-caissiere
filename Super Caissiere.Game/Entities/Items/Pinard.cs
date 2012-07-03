using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.Physics;

namespace Super_Caissiere.Entities.Items
{
    [TextureContent(AssetName = "pinard", AssetPath = "gfxs/sprites/pinard", LoadOnStartup = false)]
    [Model3DContent(AssetName = "pinard_mod", AssetPath = "models/pinard", LoadOnStartup = false)]
    [Model3DContent(AssetName = "pinard_col", AssetPath = "models/pinard_col", LoadOnStartup = false)]
    public class Pinard : Product
    {
        public Pinard()
            : base("pinard", new Microsoft.Xna.Framework.Rectangle(0, 0, 99, 180), "pinard_mod", "pinard_col")
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
            return new Pinard();
        }

        public override float Price
        {
            get { return 2.08f; }
        }
    }
}
