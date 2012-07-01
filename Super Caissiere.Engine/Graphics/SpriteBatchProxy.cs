using SuperCaissiere.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCaissiere.Engine.Graphics
{
    /// <summary>
    /// Proxy SpriteBatch, managing automatically resolution & camera 
    /// </summary>
    public class SpriteBatchProxy
    {
        /// <summary>
        /// The XNA spritebatch
        /// </summary>
        private SpriteBatch _spriteBatch;

        private Camera _currentCamera;

        /// <summary>
        /// Create a new custom spritebatch using proxy pattern
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="context"></param>
        public SpriteBatchProxy(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        #region Useful methods

        /// <summary> 
        /// Draw a given rectangle in the specified color
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        public void DrawRectangle(Rectangle rect, Color color)
        {
            _spriteBatch.Draw(Application.MagicContentManager.EmptyTexture, rect, color);
        }

        #endregion

        //********************************** BEGIN/END/CLEAR PROXIES ****************************

        /// <summary>
        /// Render without any camera (For HUD only !)
        /// </summary>
        public void BeginNoCamera()
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Resolution.ResolutionMatrix);
            _currentCamera = null;
        }

        /// <summary>
        /// Using the given camera, start rendering a scene
        /// </summary>
        public void Begin(Camera camera)
        {
            this.Begin(camera, SpriteSortMode.Deferred, BlendState.AlphaBlend);
        }

        /// <summary>
        /// Using the given camera, start rendering a scene with Parallax
        /// </summary>
        public void Begin(Camera camera, Vector2 parallax)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.ApplyParallax(parallax));
        }

        /// <summary>
        /// Using the given camera and parameters, start rendering a scene
        /// </summary>
        /// <param name="spriteSortMode"></param>
        /// <param name="blendState"></param>
        public void Begin(Camera camera, SpriteSortMode spriteSortMode, BlendState blendState)
        {
            _spriteBatch.Begin(spriteSortMode, blendState, SamplerState.PointClamp, null, null, null, camera.CameraMatrix);
            _currentCamera = camera;
        }

        /// <summary>
        /// 
        /// </summary>
        public void End()
        {
            _spriteBatch.End();

            if (_currentCamera != null)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Resolution.ResolutionMatrix);
                _currentCamera.DrawFade(this);
                _spriteBatch.End();
            }


        }

        /// <summary>
        /// Clear all the drawable zone (= the window)
        /// </summary>
        /// <param name="color"></param>
        public void ClearDevice(Color color)
        {
            _spriteBatch.GraphicsDevice.Clear(color);
        }

        /// <summary>
        /// Clear only the viewport zone
        /// </summary>
        /// <param name="color"></param>
        public void ClearViewport(Color color)
        {
            _spriteBatch.Begin();
            DrawRectangle(new Rectangle(0, 0, _spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height),
                                    color);
            _spriteBatch.End();
        }

        //********************************** DRAW PROXIES ****************************

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="destinationRectangle"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            _spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="spriteEffects"></param>
        /// <param name="layerDepth"></param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects, float layerDepth)
        {
            _spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, spriteEffects, layerDepth);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="spriteEffects"></param>
        /// <param name="layerDepth"></param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth)
        {
            _spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, spriteEffects, layerDepth);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="destinationRectangle"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="spriteEffects"></param>
        /// <param name="layerDepth"></param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects spriteEffects, float layerDepth)
        {
            _spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, spriteEffects, layerDepth);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="destinationRectangle"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="color"></param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color)
        {
            _spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="destinationRectangle"></param>
        /// <param name="color"></param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            _spriteBatch.Draw(texture, destinationRectangle, color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="destinationRectangle"></param>
        /// <param name="color"></param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle destinationRectangle, Color color)
        {
            _spriteBatch.Draw(texture, position, destinationRectangle, color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="destinationRectangle"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="color"></param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            _spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteFont"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="spriteEffects"></param>
        /// <param name="layerDepth"></param>
        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth)
        {
            _spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, spriteEffects, layerDepth);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteFont"></param>
        /// <param name="text"></param>
        /// <param name="location"></param>
        /// <param name="color"></param>
        public void DrawString(SpriteFont spriteFont, string text, Vector2 location, Color color)
        {
            _spriteBatch.DrawString(spriteFont, text, location, color);
        }
    }
}
