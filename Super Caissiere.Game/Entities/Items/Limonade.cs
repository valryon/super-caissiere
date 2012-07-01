using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.Physics;

namespace Super_Caissiere.Entities.Items
{
    [TextureContent(AssetName = "limonade", AssetPath = "gfxs/sprites/limonade", LoadOnStartup = false)]
    [Model3DContent(AssetName = "limonade_mod", AssetPath = "models/limonade", LoadOnStartup = false)]
    [Model3DContent(AssetName = "limonade_col", AssetPath = "models/limonade_col", LoadOnStartup = false)]
    public class Limonade : Product
    {
        public Limonade()
            : base("limonade", new Microsoft.Xna.Framework.Rectangle(0, 0, 95, 200), "limonade_mod", "limonade_col")
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
            return new Limonade();
        }

        public override float Price
        {
            get { return 1.45f; }
        }
    }
}
