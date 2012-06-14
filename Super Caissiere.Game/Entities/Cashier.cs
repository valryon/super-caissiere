using Microsoft.Xna.Framework;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.World;

namespace Super_Caissiere.Entities
{
    [TextureContent(AssetName = "cashier", AssetPath = "gfxs/sprites/cashier", LoadOnStartup = false)]
    public class Cashier : Entity
    {
        public Cashier()
            : base("cashier", Vector2.Zero, new Rectangle(0, 0, 800, 600), Vector2.One)
        {

        }

        public override Entity Clone()
        {
            return new Cashier();
        }
    }
}
