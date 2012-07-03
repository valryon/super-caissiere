using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.Physics;

namespace Super_Caissiere.Entities.Items
{
    [TextureContent(AssetName = "chili", AssetPath = "gfxs/sprites/chili", LoadOnStartup = false)]
    [Model3DContent(AssetName = "chili_mod", AssetPath = "models/chili", LoadOnStartup = false)]
    [Model3DContent(AssetName = "chili_col", AssetPath = "models/chili_col", LoadOnStartup = false)]
    public class Chili : Product
    {
        public Chili()
            : base("chili", new Microsoft.Xna.Framework.Rectangle(0, 0, 120, 121), "chili_mod", "chili_col")
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
