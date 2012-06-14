using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Content;
using Microsoft.Xna.Framework;
using SuperCaissiere.Engine.World;

namespace Super_Caissiere.Entities
{
    [TextureContent(AssetName = "hand", AssetPath = "gfxs/sprites/hand", LoadOnStartup = false)]
    public class Hand : Entity
    {
        public Hand()
            : base("hand", new Vector2(150,150), new Rectangle(0, 0, 512, 512), Vector2.One)
        { }

        public override Entity Clone()
        {
            return new Hand();
        }
    }
}
