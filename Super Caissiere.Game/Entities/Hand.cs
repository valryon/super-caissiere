using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Content;
using Microsoft.Xna.Framework;
using SuperCaissiere.Engine.World;
using SuperCaissiere.Engine.Core;
using SuperCaissiere.Engine.Input.Devices;

namespace Super_Caissiere.Entities
{
    [TextureContent(AssetName = "hand", AssetPath = "gfxs/sprites/hand", LoadOnStartup = false)]
    public class Hand : Entity
    {
        public Hand()
            : base("hand", new Vector2(150, 150), new Rectangle(0, 0, 256, 256), Vector2.One)
        { }

        public override void Update(GameTime gameTime)
        {
            var mouse = Application.InputManager.GetDevice<MouseDevice>(SuperCaissiere.Engine.Input.LogicalPlayerIndex.One);
            Location = (new Vector2(mouse.MouseLocation.X, mouse.MouseLocation.Y) - new Vector2(DstRect.Width / 2, DstRect.Height / 2));

            base.Update(gameTime);
        }

        public override Entity Clone()
        {
            return new Hand();
        }
    }
}
