using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.Physics;

namespace Super_Caissiere.Entities.Items
{
    [TextureContent(AssetName = "lessive", AssetPath = "gfxs/sprites/lessive", LoadOnStartup = false)]
    [Model3DContent(AssetName = "lessive_mod", AssetPath = "models/lessive", LoadOnStartup = false)]
    [Model3DContent(AssetName = "lessive_col", AssetPath = "models/lessive_col", LoadOnStartup = false)]
    public class Lessive : Product
    {
        public Lessive()
            : base("lessive", new Microsoft.Xna.Framework.Rectangle(0, 0, 120, 135), "lessive_mod", "lessive_col")
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
            return new Lessive();
        }

        public override float Price
        {
            get { return 2.08f; }
        }
    }
}
