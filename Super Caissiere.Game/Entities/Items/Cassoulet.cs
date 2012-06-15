using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.Physics;

namespace Super_Caissiere.Entities.Items
{
    [TextureContent(AssetName = "cassoulet", AssetPath = "gfxs/sprites/cassoulet", LoadOnStartup = false)]
    public class Cassoulet : ItemBase
    {
        public Cassoulet()
            : base("cassoulet", new Microsoft.Xna.Framework.Rectangle(0, 0, 92, 128))
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
            return new Cassoulet();
        }

        public override float Price
        {
            get { return 2.99f; }
        }
    }
}
