using System;
using System.Collections.Generic;
using SuperCaissiere.Engine.Core;
using SuperCaissiere.Engine.Graphics;
using SuperCaissiere.Engine.Physics;
using Microsoft.Xna.Framework;

namespace SuperCaissiere.Engine.World
{
    /// <summary>
    /// Grid of the level, containing static elements
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// Default tile size
        /// </summary>
        public static Vector2 DefaultTileSize = new Vector2(32, 32);

        /// <summary>
        /// Size of each tile (width/height), in pixels
        /// </summary>
        public Vector2 TileSize { get; set; }

        //private Tile[][] _grid;
        private Dictionary<Vector2, Tile> _grid;

        /// <summary>
        /// 
        /// </summary>
        public Grid()
            : this(DefaultTileSize)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public Grid(Vector2 tileSize)
        {
            TileSize = tileSize;
            _grid = new Dictionary<Vector2, Tile>();
        }

        /// <summary>
        /// Update the grid and its tiles
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draw the gris and its element
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatchProxy spriteBatch)
        {
            foreach (Vector2 loc in _grid.Keys)
            {
                Tile tile = _grid[loc];

                if (Application.GameStateManager.CurrentGameState.SceneCamera.VisibilityRectangle.Intersects(tile.DestRect))
                {
                    if (String.IsNullOrEmpty(tile.Spritesheet))
                    {
                        spriteBatch.Draw(Application.MagicContentManager.EmptyTexture, tile.DestRect, tile.SourceRect, Color.Black);
                    }
                    else
                    {
                        spriteBatch.Draw(Application.MagicContentManager.GetTexture(tile.Spritesheet), tile.DestRect, tile.SourceRect, Color.White);
                    }
                }
            }
        }

        /// <summary>
        /// Get a tile at the grid coordinate
        /// </summary>
        /// <returns></returns>
        public bool GetTile(Vector2 gridLocation, out Tile tile)
        {
            return _grid.TryGetValue(gridLocation, out tile);
        }

        /// <summary>
        /// Set a tile at the given coordinates
        /// </summary>
        /// <param name="tile"></param>
        public void SetTile(Vector2 gridLocation, Tile tile)
        {
            tile.DestRect = new Rectangle((int)gridLocation.X * (int)TileSize.X, (int)gridLocation.Y * (int)TileSize.Y, (int)TileSize.X, (int)TileSize.Y);
            _grid[gridLocation] = tile;
        }

        /// <summary>
        /// Delete given tile content
        /// </summary>
        public void ClearTile(Vector2 gridLocation)
        {
            Tile tile;
            if (GetTile(gridLocation, out tile))
            {
                _grid.Remove(gridLocation);
            }
        }

        public Grid Clone()
        {
            var clone = new Grid(TileSize);

            foreach (Vector2 loc in _grid.Keys)
            {
                clone.SetTile(loc, _grid[loc].Clone());
            }

            return clone;
        }

        public Dictionary<Vector2, Tile> Tiles { get { return _grid; } }

        #region Grid indexes manipulations

        /// <summary>
        /// Convert screen coordinates in grid ones
        /// </summary>
        /// <param name="absolutePoint"></param>
        /// <returns></returns>
        public static Vector2 WorldToGrid(Point absolutePoint, Vector2 tileSize)
        {
            return WorldToGrid(absolutePoint.ToVector2(), tileSize);
        }

        /// <summary>
        /// Convert screen coordinates in grid ones
        /// </summary>
        /// <param name="absolutePoint"></param>
        /// <returns></returns>
        public static Vector2 WorldToGrid(Vector2 absolutePoint, Vector2 tileSize)
        {
            return new Vector2((int)(absolutePoint.X / tileSize.X), (int)(absolutePoint.Y / tileSize.Y));
        }

        /// <summary>
        /// Convert grid location in screen coordinates
        /// </summary>
        /// <param name="gridPoint"></param>
        /// <returns></returns>
        public static Vector2 GridToWorld(Vector2 gridPoint, Vector2 tileSize)
        {
            return new Vector2((int)(gridPoint.X * tileSize.X), (int)(gridPoint.Y * tileSize.Y));
        }

        /// <summary>
        /// Round screen location to fit a grid coordinate
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector2 RoundToGrid(Vector2 point, Vector2 tileSize)
        {
            var gridLoc = WorldToGrid(point, tileSize);
            return GridToWorld(gridLoc, tileSize);
        }

        #endregion
    }
}
