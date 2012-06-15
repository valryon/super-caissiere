using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.Physics;

namespace Super_Caissiere.Entities.Items
{
    [TextureContent(AssetName = "cacolac", AssetPath = "gfxs/sprites/cacolac", LoadOnStartup = false)]
    public class Cacolac : ItemBase
    {
        public Cacolac()
            : base("cacolac", new Microsoft.Xna.Framework.Rectangle(0, 0, 128, 128))
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
            return new Cacolac();
        }

        public override float Price
        {
            get { return 2.99f; }
        }
    }
}
