using Microsoft.Xna.Framework;
using SuperCaissiere.Engine.Physics;

namespace SuperCaissiere.Engine.World
{
    /// <summary>
    /// Grid case.
    /// </summary>
    public struct Tile
    {
        public Rectangle SourceRect;
        public Rectangle DestRect;
        public string Spritesheet;

        public Tile(string assetName, Rectangle srcRect)
            : this()
        {
            SourceRect = srcRect;
            Spritesheet = assetName;
        }

        public Tile Clone()
        {
            return new Tile(Spritesheet, SourceRect);
        }
    }
}
