using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.Physics;

namespace Super_Caissiere.Entities.Items
{
    [TextureContent(AssetName = "cassoulet", AssetPath = "gfxs/sprites/cassoulet", LoadOnStartup = false)]
    [Model3DContent(AssetName = "cassoulet_mod", AssetPath = "models/cassoulet", LoadOnStartup = false)]
    [Model3DContent(AssetName = "cassoulet_col", AssetPath = "models/cassoulet_col", LoadOnStartup = false)]
    public class Cassoulet : Product
    {
        public Cassoulet()
            : base("cassoulet", new Microsoft.Xna.Framework.Rectangle(0, 0, 120, 113), "cassoulet_mod", "cassoulet_col")
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
            return new Chili();
        }

        public override float Price
        {
            get { return 2.08f; }
        }
    }
}
